SET NOCOUNT ON;

Declare @idRole uniqueidentifier
Declare @idResource uniqueidentifier
Declare @idPermission uniqueidentifier
Declare @idResourceDadosUser uniqueidentifier
Declare @idPermissionDadosUser uniqueidentifier
Declare @StoreCode uniqueidentifier
Declare @ApplicationCode uniqueidentifier
Declare @StoreName varchar(100)
Declare @ApplicationName varchar(100)

DECLARE permissionCursor CURSOR  
    FOR 
		SELECT rkApp.Code_Application, rkApp.Name_Application, rkStore.Code_Store, rkStore.Name_Store
			FROM [dbo].[THApplication] (NOLOCK) rkApp
			INNER JOIN [dbo].[THApplicationStore] (NOLOCK) rkAppStore on rkApp.Code_Application = rkAppStore.Code_Application
			INNER JOIN [dbo].[THStore] (NOLOCK) rkStore on rkStore.Code_Store = rkAppStore.Code_Store 
			Where rkApp.Status_Application = 1 and rkStore.Status_Store = 1  and rkAppStore.Status_ApplicationStore = 1 and rkApp.MemberType_Application = 1 and rkApp.Name_Application = 'TOP-Collective-Commerce'

	OPEN permissionCursor

	FETCH NEXT FROM permissionCursor
	INTO @ApplicationCode, @ApplicationName, @StoreCode, @StoreName

	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		
		PRINT 'Aplicação: ' + @ApplicationName + '; Para a Loja: ' + @StoreName
		PRINT ' - Verificando se existe a Role e o Recurso para a loja e a aplicação '

		IF (Not Exists(Select 1 from THPermission rkPermission (nolock) 
						INNER JOIN [THRole] rkRole (Nolock) on rkPermission.Code_Role = rkRole.Code_Role
						INNER JOIN [THResource] rkResource (Nolock) on rkPermission.Code_Resource = rkResource.Code_Resource
						WHERE rkRole.Code_Store = @StoreCode and rkResource.Code_Application = @ApplicationCode))
		BEGIN
			PRINT ' - Não existe Role para a Loja e nem o recurso'

			Set @idRole = NewId()
			Set @idPermission = NewId()
			Set @idResource = null

			PRINT ' - Inserindo a Role com o ID: ' + cast(@idRole as varchar(max)) + '; Nome: ' + SUBSTRING('Administradores '+ @ApplicationName, 0 , 90);
			Insert Into [dbo].[THRole] values (@idRole, @StoreCode, SUBSTRING('Administradores '+ @ApplicationName, 0 , 90) , SUBSTRING('Administradores '+ @ApplicationName + ' - ' + @StoreName, 0 , 500) , 1, getdate(), getdate(), 1);
			
			SELECT TOP 1 @idResource = Code_Resource FROM [THResource] rkResource (NOLOCK) WHERE rkResource.Code_Application = @ApplicationCode
			
			IF (@idResource is null)
			BEGIN
				Set @idResource = NewId()
				PRINT ' - Inserindo o Recurso com o ID: ' + cast(@idResource as varchar(max)) + '; Nome: ' + SUBSTRING('Administradores', 0, 90);
				Insert into [dbo].[THResource] values (@idResource, @ApplicationCode, SUBSTRING('Administradores ' + @ApplicationName, 0, 90), SUBSTRING('Administradores ' + @ApplicationName, 0, 500), 1, getdate(), getdate(), 1, null);
			END

			PRINT ' - Inserindo a permissão com o ID: ' + cast(@idPermission as varchar(max));
			Insert Into [dbo].[THPermission] values (@idPermission, @idRole, @idResource, 1, 1, getdate(), getdate());
			
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