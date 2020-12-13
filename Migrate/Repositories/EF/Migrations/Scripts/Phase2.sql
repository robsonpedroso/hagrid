--Create Code_Account in DBRKBackOffice.dbo.TRKUser
ALTER TABLE DBRKBackOffice.dbo.TRKUser ADD Code_Account UNIQUEIDENTIFIER
GO

UPDATE DBRKBackOffice.dbo.TRKUser SET Code_Account = NEWID()
GO

CREATE NONCLUSTERED INDEX [idx_user_code_account] ON [DBRKBackOffice].[dbo].[TRKUser] ([Code_Account] ASC)
INCLUDE ( [Code_User],[Email_User],[Password_User],[Name_User],[Jurisdiction_User],[Code_Store],[CNPJ_Merchant],[Status_User],[Removed_User],[SaveDate_User],[UpdateDate_User]) 
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

--Load Account
INSERT dbo.TRKAccount 
SELECT Guid_Customer, Email_Customer, Password_Customer, Email_Customer, CPF_Customer, QtyWrongsPassword_Customer, LockedUp_Customer, Status_Customer, Removed_Customer, SaveDate_Customer, UpdateDate_Customer 
FROM dbo.TRKCustomer WHERE Removed_Customer = 0 AND Type_Customer = 0 AND Password_Customer <> '' AND Password_Customer IS NOT NULL
GO

INSERT dbo.TRKAccount 
SELECT Guid_Customer, Email_Customer, Password_Customer, Email_Customer, CNPJ_Customer, QtyWrongsPassword_Customer, LockedUp_Customer, Status_Customer, Removed_Customer, SaveDate_Customer, UpdateDate_Customer 
FROM dbo.TRKCustomer WHERE Removed_Customer = 0 AND Type_Customer = 1 AND Password_Customer <> '' AND Password_Customer IS NOT NULL
GO

INSERT dbo.TRKAccount 
SELECT Code_Account, Email_User, Password_User, Email_User, '', null, null, Status_User, Removed_User, SaveDate_User, UpdateDate_User 
FROM DBRKBackOffice.dbo.TRKUser WHERE Removed_User = 0 
GO

--Load Store
DECLARE @Code_RakutenStore UNIQUEIDENTIFIER

IF(@@SERVERNAME = 'RKSRVSHOPSQL01\RKSQLSHOP01') -- Dev
BEGIN
	SET @Code_RakutenStore = 'D8671847-06AD-4FAC-BE37-321616969F4C'
END
ELSE IF(@@SERVERNAME = 'RAK-HOM01') --Homologação
BEGIN
	SET @Code_RakutenStore = '9C426B2D-3A1C-40D8-886B-1C2F7DCB8A93'
END
ELSE 
BEGIN -- Produção
	SET @Code_RakutenStore = 'D8650798-150D-44F4-BBA9-51757470AEB7'
END

INSERT dbo.TRKStore VALUES(@Code_RakutenStore, 'Rakuten', 1, 1, GETDATE(), GETDATE())

-- Importas todas as lojas exceto a Rakuten Shopping pois ja foi cadastrada acima
INSERT dbo.TRKStore  SELECT Code_Store, Name_Store, 0, 1, GETDATE(), GETDATE() 
FROM DBRKShopping.dbo.TRKStore shop
WHERE shop.Removed_Store = 0 AND shop.Code_Store <> @Code_RakutenStore
GO

--Load TRKApllication
INSERT TRKApplication VALUES (NEWID(), 'MP-Shopping', 0, 0, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'MP-BackOffice', 0, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'MP-Logistics', 0, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'MP-Accounts', 0, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'EC-Admin', 1, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'EC-Parceiros', 1, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'EC-Afiliados', 1, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'EC-Televendas', 1, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'EC-Busca', 0, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'EC-Reports', 0, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'EC-Nexus', 0, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'EC-Loja', 1, 0, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'PAY-Payments', 0, 1, 300, 1, GETDATE(), GETDATE())
GO

INSERT TRKApplication VALUES (NEWID(), 'PAY-Gateway', 0, 1, 300, 1, GETDATE(), GETDATE())
GO


--Load TRKApplicationStore
--distribuidas que não estão no client application
INSERT TRKApplicationStore 
SELECT NEWID(), NEWID(), SUBSTRING(CAST(NEWID() AS NVARCHAR(40)) , 25, 12), NEWID(), NULL, 1, a.Code_Application, s.Code_Store 
FROM TRKApplication a, TRKStore s 
WHERE a.AuthType_Application = 1 AND s.Name_Store NOT IN (
		SELECT Name_ClientApplication FROM TRKClientApplication 
		WHERE Name_ClientApplication not in ('Rakuten Accounts Web','Rakuten Logistics Web','Rakuten Shopping','Champ Store','Client info update')
)
GO

--distribuidas que estão no client application
INSERT TRKApplicationStore 
SELECT NEWID(), ca.Code_ClientApplication, ca.Password_ClientApplication, NEWID(), NULL, 1, a.Code_Application, s.Code_Store 
FROM TRKApplication a, TRKStore s INNER JOIN TRKClientApplication ca ON s.Name_Store = ca.Name_ClientApplication 
WHERE ca.Type_ClientApplication = 1 AND a.AuthType_Application = 1 AND a.Name_Application = 'EC-Loja' order by ca.Code_ClientApplication
GO

--unificadas que não estão no client application e não são rakuten
INSERT TRKApplicationStore 
SELECT NEWID(), NEWID(), NULL, NEWID(), NULL, 1, a.Code_Application, s.Code_Store 
FROM TRKApplication a, TRKStore s 
WHERE a.AuthType_Application = 0 AND s.Name_Store != 'Rakuten' AND s.Name_Store NOT IN (
	SELECT Name_ClientApplication FROM TRKClientApplication 
	WHERE Name_ClientApplication not in ('Rakuten Accounts Web','Rakuten Logistics Web','Rakuten Shopping','Champ Store','Client info update')
)
GO

--unificadas que estão no client application
INSERT TRKApplicationStore 
SELECT NEWID(), NEWID(), NULL, NEWID(), NULL, 1, a.Code_Application, s.Code_Store 
FROM TRKApplication a, TRKStore s INNER JOIN TRKClientApplication ca ON s.Name_Store = ca.Name_ClientApplication 
WHERE ca.Type_ClientApplication = 1 AND a.AuthType_Application = 0 
ORDER BY ca.Code_ClientApplication
GO

--unificadas que são rakuten
INSERT TRKApplicationStore 
SELECT NEWID(), NEWID(), SUBSTRING(CAST(NEWID() AS NVARCHAR(40)) , 25, 12), NEWID(), NULL, 1, a.Code_Application, s.Code_Store 
FROM TRKApplication a, TRKStore s 
WHERE a.AuthType_Application = 0 AND s.Name_Store = 'Rakuten'
GO

--coloca a senha do shopping da rakuten no vínculo de loja rakuten e aplicação mp-shopping
UPDATE TRKApplicationStore 
SET
	ConfClient_ApplicationStore = (SELECT C.Code_ClientApplication FROM TRKClientApplication C WHERE Name_ClientApplication = 'Rakuten Shopping'),
	ConfSecret_ApplicationStore = (SELECT CA.Password_ClientApplication FROM TRKClientApplication CA WHERE Name_ClientApplication = 'Rakuten Shopping') 
WHERE Code_Store IN (SELECT Code_Store FROM TRKStore WHERE Name_Store = 'Rakuten') AND Code_Application IN (SELECT Code_Application FROM TRKApplication WHERE Name_Application = 'MP-Shopping')
GO

--coloca a senha do accounts JS da rakuten no vínculo de loja rakuten e aplicação mp-accounts
UPDATE TRKApplicationStore 
SET
	JSClient_ApplicationStore = (SELECT C.Code_ClientApplication FROM TRKClientApplication C WHERE Name_ClientApplication = 'Rakuten Accounts Web'),	
	JSAllowedOrigins_ApplicationStore = (SELECT CA.AllowedOrigins_ClientApplication FROM TRKClientApplication CA WHERE Name_ClientApplication = 'Rakuten Accounts Web')
WHERE Code_Store IN (SELECT Code_Store FROM TRKStore WHERE Name_Store = 'Rakuten') AND Code_Application IN (SELECT Code_Application FROM TRKApplication WHERE Name_Application = 'MP-Accounts')
GO

--coloca a senha do logistics confidential logistics jacascript da rakuten no vínculo de loja rakuten e aplicação mp-logistics
UPDATE TRKApplicationStore 
SET
	JSClient_ApplicationStore = (SELECT C.Code_ClientApplication FROM TRKClientApplication C WHERE Name_ClientApplication = 'Rakuten Logistics Web'),	
	JSAllowedOrigins_ApplicationStore = (SELECT CA.AllowedOrigins_ClientApplication FROM TRKClientApplication CA WHERE Name_ClientApplication = 'Rakuten Logistics Web') 
WHERE Code_Store IN (SELECT Code_Store FROM TRKStore WHERE Name_Store = 'Rakuten') AND Code_Application IN (SELECT Code_Application FROM TRKApplication WHERE Name_Application = 'MP-Logistics')
GO

IF(@@SERVERNAME = 'RKSRVSHOPSQL01\RKSQLSHOP01' OR @@SERVERNAME = 'RAK-HOM01')
BEGIN
	--coloca a senha do backoffice confidential backoffice jacascript da rakuten no vínculo de loja rakuten e aplicação mp-backoffice LOCAL E HOM
	UPDATE TRKApplicationStore 
	SET
		ConfClient_ApplicationStore = '3C9B5100-9FD0-452B-ACED-946F3AEE29B1',
		ConfSecret_ApplicationStore = '83601E3306FC'
	WHERE Code_Store IN (SELECT Code_Store FROM TRKStore WHERE Name_Store = 'Rakuten') AND Code_Application IN (SELECT Code_Application FROM TRKApplication WHERE Name_Application = 'MP-BackOffice')
END
ELSE
BEGIN
	--coloca a senha do backoffice confidential backoffice jacascript da rakuten no vínculo de loja rakuten e aplicação mp-backoffice = PRODUÇÃO
	UPDATE TRKApplicationStore 
	SET
		ConfClient_ApplicationStore = 'D9594398-04DD-407E-8761-360A86766C25',
		ConfSecret_ApplicationStore = '0B3748B20728'
	WHERE Code_Store IN (SELECT Code_Store FROM TRKStore WHERE Name_Store = 'Rakuten') AND Code_Application IN (SELECT Code_Application FROM TRKApplication WHERE Name_Application = 'MP-BackOffice')
END
GO

--Load TRKAccountApplicationStore
INSERT TRKAccountApplicationStore SELECT Guid_Customer, Code_ApplicationStore
FROM TRKCustomer C, TRKApplicationStore AST, TRKApplication A 
WHERE A.Name_Application IN ('MP-Shopping', 'EC-Loja') AND AST.Code_Application = A.Code_Application AND  C.Removed_Customer = 0 AND C.Password_Customer <> '' AND C.Password_Customer IS NOT NULL
GO

INSERT TRKAccountApplicationStore 
SELECT U.Code_Account, Code_ApplicationStore 
FROM DBRKBackOffice.dbo.TRKUser U, TRKApplicationStore AST, TRKApplication A
WHERE A.Name_Application IN ('MP-BackOffice') AND AST.Code_Application = A.Code_Application AND AST.Code_Store = U.Code_Store AND U.Removed_User = 0 
GO

INSERT TRKAccountApplicationStore 
SELECT U.Code_Account, Code_ApplicationStore 
FROM DBRKBackOffice.dbo.TRKUser U, TRKApplicationStore AST, TRKApplication A , TRKStore S 
WHERE A.Name_Application IN ('MP-BackOffice') AND AST.Code_Application = A.Code_Application AND AST.Code_Store = S.Code_Store AND S.Name_Store = 'Rakuten' AND (U.Code_Store IS NULL OR U.Code_Store = '00000000-0000-0000-0000-000000000000') AND U.Removed_User = 0 
GO

INSERT TRKAccountApplicationStore 
SELECT DISTINCT U.Code_Account, Code_ApplicationStore 
FROM DBRKBackOffice.dbo.TRKUser U, DBRKBackOffice.dbo.TRKFunctionality F, DBRKBackOffice.dbo.TRKUserGroup UG, DBRKBackOffice.dbo.TRKGroup G, TRKApplicationStore AST, TRKApplication A 
WHERE 
	A.Name_Application = 'PAY-Payments' 
	AND AST.Code_Application = A.Code_Application 
	AND AST.Code_Store = U.Code_Store 
	AND F.Name_Functionality = 'Módulo de Pagamentos' 
	AND F.Code_Group = UG.Code_Group AND F.Removed_Functionality = 0 
	AND UG.Code_User = U.Code_User 
	AND G.Removed_Group = 0 
	AND U.Code_Store != '00000000-0000-0000-0000-000000000000' 
	AND U.Code_Store = F.Code_Store
	AND U.Removed_User = 0
GO

INSERT INTO TRKAccountApplicationStore
SELECT U.Code_Account, APPS.Code_ApplicationStore
FROM DBRKBackOffice.dbo.TRKUser U WITH(NOLOCK)
	INNER JOIN DBRKBackOffice.dbo.TRKUserGroup UG WITH(NOLOCK)
		ON U.Code_User = UG.Code_User
	INNER JOIN TRKApplicationStore APPS WITH(NOLOCK)
		ON APPS.Code_Store = U.Code_Store 
	INNER JOIN TRKApplication A WITH(NOLOCK)
		ON APPS.Code_Application = A.Code_Application
WHERE 
	Code_Group = 2 
	AND A.Name_Application = 'PAY-Payments'
	AND Code_Account != '00000000-0000-0000-0000-000000000000'
	AND U.Removed_User = 0 
GO