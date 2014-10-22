#r @"tools/FSharp.CLI/lib/net45/FSharp.CLI.dll"

open FSharp.CLI

type LiterateCLI = {
    
    [<NoSwitch>]
    targets : list<string>

    [<Description("output file (either .fs | .tex) ")>]
    [<Switches("o", "output")>]
    output : Option<string>
}

let usage, parser = usageAndParser<LiterateCLI>