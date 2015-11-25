@echo off
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
   set VsTestConsole="C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\MSTest.exe"
)

%VsTestConsole% /testcontainer:'.\Source\Cyclops.Tests\bin\%config%\Cyclops.Tests.dll' /category:myget

REM Package
mkdir Build
call %nuget% pack "Source\Cyclops\Cyclops.csproj" -symbols -o Build -p Configuration=%config% %version%

