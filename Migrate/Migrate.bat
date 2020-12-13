
@ECHO OFF

echo Init Migrate

migrate.exe Rakuten.Accounts.Core.Infrastructure.dll /startupConfigurationFile="..\\web.config" /verbose

echo Completed

pause