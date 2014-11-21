#!/bin/bash

if [ ! -d "tools" ]; then
	echo "downloading FAKE"
	mono --runtime=v4.0 tools/NuGet/nuget.exe install FSharp.CLI -OutputDirectory tools -ExcludeVersion
	mono --runtime=v4.0 tools/NuGet/nuget.exe install Fake -OutputDirectory tools -ExcludeVersion -Prerelease 
	mono --runtime=v4.0 tools/NuGet/nuget.exe install FParsec -OutputDirectory tools -ExcludeVersion 
fi


cd tools/build/

fsharpi build.fsx $@

