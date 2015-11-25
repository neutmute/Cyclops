rem @echo off
REM myget build script

set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

REM Build
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Cyclops.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM test
if "%VsTestConsole%" == "" (
   set VsTestConsole="C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
)

%VsTestConsole% .\Source\Cyclops.Tests\bin\%config%\Cyclops.Tests.dll /TestCaseFilter:"TestCategory=myget"

REM Package
mkdir Build
call %nuget% pack "Source\Cyclops\Cyclops.csproj" -symbols -o Build -p Configuration=%config% %version%

REM Publish
%nuget% setApiKey %nugetapikey%
%nuget% push .\build\*

