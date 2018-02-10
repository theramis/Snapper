param(
    [switch] $release
)

if ($release) { $config = "Release" } else { $config = "Debug" }

Write-Host "Build Configuration: $config"

$solutionFile = [System.IO.Path]::GetFullPath( (Join-Path $PSScriptRoot '..\Snapper.sln') )

dotnet msbuild $solutionFile "/p:Configuration=$config" "/consoleloggerparameters:Summary"

Write-Host "Build Complete." -ForegroundColor "Green"