@echo off

IF exist tools ( echo skipping package download ) ELSE ( 
echo downloading packages
REM mklink .\.git\hooks\pre-commit .\pre-commit
"nuget\nuget.exe" "install" "FSharp.CLI" "-OutputDirectory" "tools" "-ExcludeVersion" "-Prerelease"
)
