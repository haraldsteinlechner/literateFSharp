#I @"tools/FAKE/tools/"
#r @"FakeLib.dll"

open Fake
open Fake.FscHelper

Target "flit" (fun () -> 
    
    ["flit.fs";] |> Fsc (fun p -> { p with FscTarget = Exe; 
                                           References = [ @"tools\FSharp.CLI\lib\net45\FSharp.CLI.dll" ] 
                                  } )
)

Target "RestorePackages" (fun () ->

    let packageConfigs = !!"**/packages.config" 

    let defaultNuGetSources = RestorePackageHelper.RestorePackageDefaults.Sources
    for pc in packageConfigs do
        RestorePackage (fun p -> { p with OutputPath = "packages" }) pc


)

RunTargetOrDefault "flit"