@ECHO OFF

REM The following directory is for .NET 4.0
set DOTNETFX2=C:\Windows\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%
set usernameCurrent=<USER_NAME> (Ex.: XXXX\NetworkService)
set passwordCurrent=<PASSWORD>

echo Installing Single Win Service...
echo ---------------------------------------------------
installutil /username=%usernameCurrent% /password=%passwordCurrent% <DIR_ROBOT> (Ex.: E:\Robots\hagrid.Bots\hagrid.Bots.exe)
echo ---------------------------------------------------
echo Completed Install single with successfully.
pause

