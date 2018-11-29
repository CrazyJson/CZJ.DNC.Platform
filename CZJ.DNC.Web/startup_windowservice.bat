@echo off
set ASPNETCORE_ENVIRONMENT=Production
sc create SysconfigService binPath= "\"C:\program files\dotnet\dotnet.exe\" \"%~dp0CZJ.DNC.Web.dll\"" DisplayName= "SysconfigService" start= auto
sc start SysconfigService 
pause