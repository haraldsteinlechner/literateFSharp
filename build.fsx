#I @"tools/FAKE/tools/"
#r @"FakeLib.dll"

open Fake
open Fake.FscHelper

Target "flit" (fun () -> 
    
    ["flit.fs";] |> Fsc (fun p -> { p with FscTarget = Exe; 
                                           References = [ @"tools\FSharp.CLI\lib\net45\FSharp.CLI.dll" ] 
                                  } )
)

Target "Build" (fun () ->
  let buildMode = getBuildParamOrDefault "buildMode" "Release"
  let setParams defaults =
          { defaults with
              Verbosity = Some(Quiet)
              Targets = ["Build"]
              Properties =
                  [
                      "Optimize", "True"
                      "DebugSymbols", "True"
                      "Configuration", buildMode
                  ]
           }
  build setParams "./LiterateFSharp.sln"
        |> DoNothing
)

Target "RestorePackages" (fun () ->

    let packageConfigs = !!"**/packages.config" 

    let defaultNuGetSources = RestorePackageHelper.RestorePackageDefaults.Sources
    for pc in packageConfigs do
        RestorePackage (fun p -> { p with OutputPath = "packages" }) pc


)

RunTargetOrDefault "flit"