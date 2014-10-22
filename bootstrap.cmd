@echo off

IF exist tools ( echo skipping package download ) ELSE ( 
echo downloading packages
REM mklink .\.git\hooks\pre-commit .\pre-commit
"nuget\nuget.exe" "install" "FSharp.CLI" "-OutputDirectory" "tools" "-ExcludeVersion" "-Prerelease"
"nuget\nuget.exe"  "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion" "-Prerelease"
"nuget\nuget.exe"  "install" "FParsec" "-OutputDirectory" "tools" "-ExcludeVersion" "-Prerelease"
)

"tools\FAKE\tools\Fake.exe" "build.fsx" "target=flit"