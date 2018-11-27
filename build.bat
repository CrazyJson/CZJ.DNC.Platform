@echo off
ECHO. 1.package restore
dotnet restore

ECHO. 2.project publish
dotnet publish -c release

ECHO. 3.copy file
for /f "tokens=1-3 delims=/ " %%i in ("%date%") do set curDate=%%i%%j%%k
set pulishDir=%cd%\DailyBuild\Publish_%curDate%

set destinationDir=%cd%\CZJ.DNC.Web\bin\Release\netcoreapp2.0\publish

xcopy %destinationDir% %pulishDir%\ /s/d/r/y

ECHO. 4.delete file
rd/s/q %cd%\CZJ.DNC.Web\bin

pause



