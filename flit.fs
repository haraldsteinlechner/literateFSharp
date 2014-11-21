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

(*_ 
\textbf{Hello World}
_*)

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