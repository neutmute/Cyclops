param(
    [string]$packageVersion = $null,
    [string]$configuration = "Release",
    [string]$target = "build"
)

# Initialization
$rootFolder = Split-Path -parent $script:MyInvocation.MyCommand.Path
$rootFolder = Join-Path $rootFolder .
$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

New-Item -Force -ItemType directory -Path $rootFolder\_output

# Solution
$solutionName = "cyclops"

Write-Host "Build Solution"
& $msbuild "$rootFolder\$solutionName.sln" /p:Configuration=$configuration

Write-Host "Execute Tests"
if(!(Test-Path Env:\VsTestConsole )){
    $env:VsTestConsole = "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
}
& $env:VsTestConsole .\Source\Cyclops.Tests\bin\$configuration\Cyclops.Tests.dll /TestCaseFilter:"TestCategory!=SqlIntegration"

Write-Host "Nuget pack"
if(!(Test-Path Env:\nuget )){
    $env:nuget = "$rootFolder\.nuget\nuget.exe"
}
if(!(Test-Path Env:\version )){
    $env:version = "1.0.0.0"
}

&$env:nuget pack $rootFolder\Source\Cyclops\Cyclops.csproj -o _output -p Configuration=$configuration -Version $env:version
&$env:nuget pack $rootFolder\Source\Cyclops.DependencyInjection\Cyclops.DependencyInjection.csproj -o _output -p Configuration=$configuration -Version $env:version

if(Test-Path Env:\myget ){
    Write-Host "Nuget publish"
    &$env:nuget push .\_output\* $env:nugetapikey
}

