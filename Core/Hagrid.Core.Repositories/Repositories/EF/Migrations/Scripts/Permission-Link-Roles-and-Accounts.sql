SET NOCOUNT ON;

Declare @idRole uniqueidentifier
Declare @idAccount uniqueidentifier

DECLARE permissionAccountCursor CURSOR  
    FOR 
		SELECT AccAppStore.Code_Account, rkRole.Code_Role
			FROM [DBRKAccounts].[dbo].[TRKAccountApplicationStore] (nolock) AccAppStore
				Inner Join [DBRKAccounts].[dbo].[TRKApplicationStore] (nolock) AppStore on AccAppStore.Code_ApplicationStore = AppStore.Code_ApplicationStore
				Inner Join [DBRKAccounts].[dbo].[TRKRole] (nolock) rkRole on rkRole.Code_Store = AppStore.Code_Store
				Inner Join [DBRKAccounts].[dbo].[TRKPermission] (nolock) rkPermission on rkPermission.Code_Role = rkRole.Code_Role
				Inner Join [DBRKAccounts].[dbo].[TRKResource] (nolock) rkResource on rkResource.Code_Resource = rkPermission.Code_Resource and rkResource.Code_Application = AppStore.Code_Application
			Where AppStore.Status_ApplicationStore = 1 and rkrole.Status_Role = 1 and rkPermission.Status_Permission = 1 

	OPEN permissionAccountCursor

	FETCH NEXT FROM permissionAccountCursor
	INTO @idAccount, @idRole

	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		
		PRINT 'Account: ' + cast(@idAccount as varchar(max)) + '; Role: ' + cast(@idRole as varchar(max))
		PRINT ' - Verificando se existe a Role e o Recurso para a loja e a aplicação '

		IF (Not Exists(Select 1 from [DBRKAccounts].[dbo].TRKAccountRole rkAccRole (nolock) 
						WHERE rkAccRole.Code_Account = @idAccount and rkAccRole.Code_Role = @idRole))
		BEGIN
			PRINT ' - Não existe vinculo de usuário para o grupo'

			PRINT ' - Efetuando o link do Account com o Role'
			Insert Into [DBRKAccounts].[dbo].TRKAccountRole values (newid(), @idAccount, @idRole, 1, getdate(), getdate());

		END
		ELSE
		BEGIN 
			PRINT ' - Já existe'
		END

		PRINT ' ------------------------------------------------------------------------ '

		FETCH NEXT FROM permissionAccountCursor
		INTO @idAccount, @idRole

	END
CLOSE permissionAccountCursor;
DEALLOCATE permissionAccountCursor;


SET NOCOUNT OFF;