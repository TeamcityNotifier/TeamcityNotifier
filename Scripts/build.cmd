@echo off
powershell -ExecutionPolicy Unrestricted -Command .\psake.ps1 -properties @{"publish"=$false;"teamcity"=$false;};
pause
