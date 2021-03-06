SET NOCOUNT ON;

Declare @idRole uniqueidentifier
Declare @idResource uniqueidentifier
Declare @idPermission uniqueidentifier
Declare @StoreCode uniqueidentifier
Declare @ApplicationCode uniqueidentifier
Declare @StoreName varchar(100)
Declare @ApplicationName varchar(100)

DECLARE permissionCursor CURSOR  
    FOR 
		SELECT rkApp.Code_Application, rkApp.Name_Application, rkStore.Code_Store, rkStore.Name_Store
			FROM [DBRKAccounts].[dbo].[TRKApplication] (NOLOCK) rkApp
			INNER JOIN [DBRKAccounts].[dbo].[TRKApplicationStore] (NOLOCK) rkAppStore on rkApp.Code_Application = rkAppStore.Code_Application
			INNER JOIN [DBRKAccounts].[dbo].[TRKStore] (NOLOCK) rkStore on rkStore.Code_Store = rkAppStore.Code_Store 
			Where rkApp.Status_Application = 1 and rkStore.Status_Store = 1  and rkAppStore.Status_ApplicationStore = 1 and rkApp.MemberType_Application = 1

	OPEN permissionCursor

	FETCH NEXT FROM permissionCursor
	INTO @ApplicationCode, @ApplicationName, @StoreCode, @StoreName

	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		
		PRINT 'Aplicação: ' + @ApplicationName + '; Para a Loja: ' + @StoreName
		PRINT ' - Verificando se existe a Role e o Recurso para a loja e a aplicação '

		IF (Not Exists(Select 1 from TRKPermission rkPermission (nolock) 
						INNER JOIN [TRKRole] rkRole (Nolock) on rkPermission.Code_Role = rkRole.Code_Role
						INNER JOIN [TRKResource] rkResource (Nolock) on rkPermission.Code_Resource = rkResource.Code_Resource
						WHERE rkRole.Code_Store = @StoreCode and rkResource.Code_Application = @ApplicationCode))
		BEGIN
			PRINT ' - Não existe Role para a Loja e nem o recurso'

			Set @idRole = NewId()
			Set @idResource = NewId()
			Set @idPermission = NewId()

			PRINT ' - Inserindo a Role com o ID: ' + cast(@idRole as varchar(max)) + '; Nome: ' + SUBSTRING('Role RK-Default-Access ' + @StoreName + ' ' + @ApplicationName, 0 , 90);
			Insert Into [DBRKAccounts].[dbo].[TRKRole] values (@idRole, @StoreCode, SUBSTRING('Role RK-Default-Access ' + @StoreName + ' ' + @ApplicationName, 0 , 90) , SUBSTRING('Role RK-Default-Access ' + @StoreName + ' ' + @ApplicationName, 0 , 500) , 1, getdate(), getdate());

			PRINT ' - Inserindo o Recurso com o ID: ' + cast(@idResource as varchar(max)) + '; Nome: ' + SUBSTRING('Resource RK-Default-Access ' + @StoreName + ' ' + @ApplicationName, 0, 90);
			Insert into [DBRKAccounts].[dbo].[TRKResource] values (@idResource, @ApplicationCode, SUBSTRING('RES RK-Default-Access ' + @StoreName + ' ' + @ApplicationName, 0, 90), SUBSTRING('RES RK-Default-Access ' + @StoreName + ' ' + @ApplicationName, 0, 500), 1, getdate(), getdate());

			PRINT ' - Inserindo a permissão com o ID: ' + cast(@idPermission as varchar(max));
			Insert Into [DBRKAccounts].[dbo].[TRKPermission] values (@idPermission, @idRole, @idResource, 1, 1, getdate(), getdate());
			
		END
		ELSE
		BEGIN 
			PRINT ' - Já existe'
		END

		PRINT ' ------------------------------------------------------------------------ '

		FETCH NEXT FROM permissionCursor
		INTO @ApplicationCode, @ApplicationName, @StoreCode, @StoreName

	END
CLOSE permissionCursor;
DEALLOCATE permissionCursor;


SET NOCOUNT OFF;