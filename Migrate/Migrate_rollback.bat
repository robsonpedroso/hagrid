
@ECHO OFF

echo Init Migrate

migrate.exe Rakuten.Accounts.Core.Infrastructure.dll /startupConfigurationFile="..\\web.config" /targetMigration="0000000" /verbose

echo Completed