@ECHO OFF

REM The following directory is for .NET 4.0
set DOTNETFX2=C:\Windows\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%

echo Installing Win Service...
echo ---------------------------------------------------
installutil -u <DIR_ROBOT> (Ex.: E:\Robots\hagrid.Bots\hagrid.Bots.exe)
echo ---------------------------------------------------
echo Done.
pause

