param(
    [string]$packageVersion = $null,
    [string]$configuration = "Release"
)

. ".\build.common.ps1"

$solutionName = "Cyclops"
$sourceUrl = "https://github.com/neutmute/Cyclops"

function init {
    # Initialization
    $global:rootFolder = Split-Path -parent $script:MyInvocation.MyCommand.Path
    $global:rootFolder = Join-Path $rootFolder .
    $global:packagesFolder = Join-Path $rootFolder packages
    $global:outputFolder = Join-Path $rootFolder _output
    $global:msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

    _WriteOut -ForegroundColor $ColorScheme.Banner "-= $solutionName Build =-"
    _WriteConfig "rootFolder" $rootFolder
}

function restorePackages{
    _WriteOut -ForegroundColor $ColorScheme.Banner "nuget, gitlink restore"
    
    New-Item -Force -ItemType directory -Path $packagesFolder
    _DownloadNuget $packagesFolder
    nuget restore
    nuget install gitlink -SolutionDir "$rootFolder" -ExcludeVersion
}

function nugetPack{
    _WriteOut -ForegroundColor $ColorScheme.Banner "Nuget pack"
    
    New-Item -Force -ItemType directory -Path $outputFolder

    if(!(Test-Path Env:\nuget )){
        $env:nuget = nuget
    }
    if(!(Test-Path Env:\PackageVersion )){
        $env:PackageVersion = "1.0.0.0"
    }

    nuget pack $rootFolder\Source\Cyclops\Cyclops.csproj -o $outputFolder -p Configuration=$configuration -Version $env:PackageVersion
    nuget pack $rootFolder\Source\Cyclops.DependencyInjection\Cyclops.DependencyInjection.csproj -o $outputFolder -p Configuration=$configuration -Version $env:PackageVersion
}

function nugetPublish{

    if(Test-Path Env:\BuildRunner ){
        _WriteOut -ForegroundColor $ColorScheme.Banner "Nuget publish"
        &$env:nuget push .\_output\* $env:nugetapikey
    }
    else{
        _WriteOut -ForegroundColor Yellow "MyGet builder runner not detected. Skipping nuget publish"
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

init

restorePackages

buildSolution

executeTests

nugetPack

nugetPublish

Write-Host "Build $env:PackageVersion complete"