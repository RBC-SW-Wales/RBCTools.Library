
@echo Off
set config=%1
if "%config%" == "" (
   set config=Debug
)

set msbuildPath= %WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild

echo.
echo ** Building the solution **
echo.

%msbuildPath% src\RbcConsole.Installer.sln

pause
