#if INTERACTIVE
#r @"tools/FSharp.CLI/lib/net45/FSharp.CLI.dll"
#else
#endif

open FSharp.CLI

type LiterateCLI = {
    
    [<NoSwitch>]
    targets : list<string>

    [<Description("output file (either .fs | .tex) ")>]
    [<Switches("o", "output")>]
    output : string
}

let usage, parser = usageAndParser<LiterateCLI>



let extractCode (settings : LiterateCLI) = 
    failwith ""
let extractTex (settings : LiterateCLI) = 
    failwith ""

[<EntryPoint>]
let main args = 
    let pUsage () = printfn "usage %s" usage; 2
    match parser args with
        | Some settings -> 
            if settings.output.EndsWith ".fs"
            then extractCode settings
            elif settings.output.EndsWith ".tex"
            then extractTex settings
            else pUsage () 
        | None -> pUsage () 