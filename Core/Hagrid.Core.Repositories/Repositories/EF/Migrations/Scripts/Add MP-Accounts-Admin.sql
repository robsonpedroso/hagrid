USE DBRKAccounts

--Insere aplicação Hagrid-UI-Admin
DECLARE @Code_Application UNIQUEIDENTIFIER
SELECT @Code_Application = Code_Application FROM THApplication WHERE Name_Application = 'Hagrid-UI-Admin'

IF (@Code_Application IS NULL)
BEGIN
	SET @Code_Application = NEWID()	
	INSERT THApplication VALUES (@Code_Application, 'Hagrid-UI-Admin', 0, 1, 43200, 1, GETDATE(), GETDATE())
END


--Insere os confs da aplicação Hagrid-UI-Admin
DECLARE @Code_ApplicationStore UNIQUEIDENTIFIER
SELECT @Code_ApplicationStore = Code_ApplicationStore
FROM DBRKAccounts.dbo.THStore s(nolock) 
INNER JOIN DBRKAccounts.dbo.THApplicationStore aps (nolock) on aps.Code_Store = s.Code_Store	
WHERE Code_Application = @Code_Application AND Name_Store = 'Hagrid'

IF (@Code_ApplicationStore IS NULL)
BEGIN

	SET @Code_ApplicationStore = NEWID()
		
	INSERT THApplicationStore 
	SELECT 
		@Code_ApplicationStore, NEWID(), SUBSTRING(CAST(NEWID() AS NVARCHAR(40)) , 25, 12), NEWID(), 'http://localhost:4201', 1, a.Code_Application, s.Code_Store 
	FROM 
		THApplication a, THStore s 
	WHERE 
		a.AuthType_Application = 0 AND s.Name_Store = 'Hagrid' AND Code_Application = @Code_Application
END


---- Insere as permissões para nossos usuários
--DECLARE @Accounts TABLE (Code_Account UNIQUEIDENTIFIER, Email_Account VARCHAR(255))

--INSERT INTO @Accounts
--SELECT 
--	Code_Account, Email_Account
--FROM 
--	THAccount 
--WHERE 
--	Email_Account IN (
--		'robson.pedroso@Hagrid.com.br'
--	)
--	AND (Document_Account is null or Document_Account = '')

--INSERT INTO THAccountApplicationStore
--SELECT B.Code_Account, @Code_ApplicationStore FROM (
--	SELECT Code_Account, A.Email_Account
--	FROM @Accounts A
--	WHERE A.Code_Account NOT IN (SELECT Code_Account FROM THAccountApplicationStore WHERE Code_ApplicationStore = @Code_ApplicationStore)
--) B


-- lista os confs para colcoar no web.config
SELECT 
 Code_ApplicationStore, Name_Application, ConfClient_ApplicationStore, ConfSecret_ApplicationStore, JSClient_ApplicationStore, JSAllowedOrigins_ApplicationStore, s.Code_Store, Name_Store, ap.Code_Application, ap.Name_Application
FROM 
	DBRKAccounts.dbo.THStore s(nolock)
	INNER JOIN DBRKAccounts.dbo.THApplicationStore aps (nolock) on aps.Code_Store = s.Code_Store
	INNER JOIN DBRKAccounts.dbo.THApplication ap (nolock) on aps.Code_Application	= ap.Code_Application
WHERE
	Name_Application = 'Hagrid-UI-Admin'
AND 
	Name_Store = 'Hagrid'