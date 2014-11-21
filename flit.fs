#if INTERACTIVE
#r @"tools/FSharp.CLI/lib/net45/FSharp.CLI.dll"
#I "packages/FParsec.1.0.1/lib/net40-client"
#r "FParsec.dll"
#r "FParsecCS.dll"
#else
#endif

open FSharp.CLI
open System.IO
open System.Collections.Generic
open FParsec

type LiterateCLI = {
    
    [<NoSwitch>]
    [<Description("literate fsharp file")>]
    source : string

    [<Description("output files (either .fs | .tex) ")>]
    [<Switches("o", "output")>]
    output : list<string>
}

let usage, parser = usageAndParser<LiterateCLI>

let extension (file : string) = Path.GetExtension file
let endsWith (suffix : string) (s : string) = s.EndsWith suffix

let toFile (fileName : string) (content : string) = File.WriteAllText (fileName,content)

let text = """
abc
def(*_ 
\textbf{Hello World}
_*)
urdar
abc(*_abc_*)cde""" 

let text2 = "(*_abc_*)(*_cdef_*)"

let parse f =
    between (pstring """(*_""") (pstring """_*)""") (CharParsers.charsTillString """_*)""" false System.Int32.MaxValue)

let literate f = many ((attempt <| (CharParsers.charsTillString """(*_""" false System.Int32.MaxValue .>>. parse f))) .>>. (manyTill anyChar eof) 
                 |>> (fun (xs,last) -> let code = List.concat [List.map fst xs; [System.String.Concat(last |> List.toArray)]] |> String.concat ""
                                       let comments = List.map snd xs |> String.concat ""
                                       code,comments )

let test p str =
    match run p str with
    | Success(result, _, _)   ->
        printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

let extractComments = test (literate (fun a b -> (a,b)))


let convertTo (sourceFile : string) (targetFile  : string) =
    printfn "convert: %s to %s" sourceFile targetFile
    if not <| File.Exists sourceFile 
    then printfn "could not find input file: %s" sourceFile
         None
    else 
        let str = File.ReadAllText sourceFile
        Some str


[<EntryPoint>]
let main args = 
    let pUsage () = printfn "usage %s" usage; 2
    match parser args with
        | Some settings -> 
            let validExtension s = extension s = ".fs" || extension s = ".tex" 
            let blub = settings.output |> Seq.toArray |> Array.map validExtension
            if settings.output |> Seq.forall validExtension
            then
                printfn "output files: %A" settings.output
                let rec run = function 
                              | x::xs -> match convertTo settings.source x with
                                            | None -> -1
                                            | Some v -> toFile x v; run xs
                              | [] -> 0
                run settings.output
            else
                printfn "unsupported file ending."
                pUsage()
        | None -> pUsage () 