param(
    [string]$packageVersion = $null,
    [string]$configuration = "Release"
)

. ".\build.common.ps1"

function restorePackages{
    _WriteOut -ForegroundColor $ColorScheme.Banner "nuget, gitlink restore"
    
    New-Item -Force -ItemType directory -Path $packagesFolder
    _DownloadNuget $packagesFolder
    nuget restore
    nuget install gitlink -SolutionDir "$rootFolder" -ExcludeVersion
}

function nugetPack{
    _WriteOut -ForegroundColor $ColorScheme.Banner "Nuget pack"
    
    New-Item -Force -ItemType directory -Path $rootFolder\_output

    if(!(Test-Path Env:\nuget )){
        $env:nuget = nuget
    }
    if(!(Test-Path Env:\PackageVersion )){
        $env:PackageVersion = "1.0.0.0"
    }

    #&$env:nuget pack $rootFolder\Source\Cyclops\Cyclops.csproj -o _output -p Configuration=$configuration -Version $env:PackageVersion
    #&$env:nuget pack $rootFolder\Source\Cyclops.DependencyInjection\Cyclops.DependencyInjection.csproj -o _output -p Configuration=$configuration -Version $env:PackageVersion
    nuget pack $rootFolder\Source\Cyclops\Cyclops.csproj -o _output -p Configuration=$configuration -Version $env:PackageVersion
    nuget pack $rootFolder\Source\Cyclops.DependencyInjection\Cyclops.DependencyInjection.csproj -o _output -p Configuration=$configuration -Version $env:PackageVersion
}

function nugetPublish{

    if(Test-Path Env:\BuildRunner ){
        _WriteOut -ForegroundColor $ColorScheme.Banner "Nuget publish"
        &$env:nuget push .\_output\* $env:nugetapikey
    }

}

function buildSolution{

    _WriteOut -ForegroundColor $ColorScheme.Banner "Build Solution"
    & $msbuild "$rootFolder\$solutionName.sln" /p:Configuration=$configuration

    &"$rootFolder\packages\gitlink\lib\net45\GitLink.exe" $rootFolder -u $sourceUrl
}

function executeTests{

    Write-Host "Execute Tests"
    if(!(Test-Path Env:\VsTestConsole )){
        $env:VsTestConsole = "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
    }
    & $env:VsTestConsole .\Source\Cyclops.Tests\bin\$configuration\Cyclops.Tests.dll /TestCaseFilter:"TestCategory!=SqlIntegration"

}

# Initialization
$rootFolder = Split-Path -parent $script:MyInvocation.MyCommand.Path
$rootFolder = Join-Path $rootFolder .
$packagesFolder  = Join-Path $rootFolder packages
$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

# Solution
$solutionName = "Cyclops"
$sourceUrl = "https://github.com/neutmute/Cyclops"

_WriteOut -ForegroundColor $ColorScheme.Banner "-= $solutionName Build =-"
_WriteConfig "rootFolder" $rootFolder

restorePackages

buildSolution

executeTests

nugetPack

nugetPublish