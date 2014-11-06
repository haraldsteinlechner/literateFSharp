#if INTERACTIVE
#r @"tools/FSharp.CLI/lib/net45/FSharp.CLI.dll"
#else
#endif

open FSharp.CLI
open System.IO
open System.Collections.Generic

type LiterateCLI = {
    
    [<NoSwitch>]
    source : string

    [<Description("output files (either .fs | .tex) ")>]
    [<Switches("o", "output")>]
    output : list<string>
}

let usage, parser = usageAndParser<LiterateCLI>

let extension (file : string) = Path.GetExtension file
let endsWith (suffix : string) (s : string) = s.EndsWith suffix

let toFile (fileName : string) (content : string) = File.WriteAllText (fileName,content)

let convertTo (sourceFile : string) (targetFile  : string) =
    "" 


[<EntryPoint>]
let main args = 
    let pUsage () = printfn "usage %s" usage; 2
    match parser args with
        | Some settings -> 
            let validExtension s = extension s = "fs" || extension s = "tex" 
            if settings.output |> Seq.exists (not << validExtension) 
            then
                printfn "unsupported file ending."
                pUsage()
            else
                for o in settings.output do
                    convertTo settings.source o |> toFile o
                0
        | None -> pUsage () 