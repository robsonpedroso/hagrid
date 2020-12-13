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
			Where rkApp.Status_Application = 1 and rkStore.Status_Store = 1  and rkAppStore.Status_ApplicationStore = 1 and rkApp.MemberType_Application = 1 and rkApp.Name_Application <> 'Hagrid-UI-Login'

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

			PRINT ' - Inserindo a Role com o ID: ' + cast(@idRole as varchar(max)) + '; Nome: ' + SUBSTRING('Role Default-Access '+ @ApplicationName, 0 , 90);
			Insert Into [dbo].[THRole] values (@idRole, @StoreCode, SUBSTRING('Role Default-Access '+ @ApplicationName, 0 , 90) , SUBSTRING('Role Default-Access '+ @ApplicationName + ' - ' + @StoreName, 0 , 500) , 1, 1, getdate(), getdate());
			
			SELECT TOP 1 @idResource = Code_Resource FROM [THResource] rkResource (NOLOCK) WHERE rkResource.Code_Application = @ApplicationCode
			
			IF (@idResource is null)
			BEGIN
				Set @idResource = NewId()
				PRINT ' - Inserindo o Recurso com o ID: ' + cast(@idResource as varchar(max)) + '; Nome: ' + SUBSTRING('Resource Default-Access', 0, 90);
				Insert into [dbo].[THResource] values (@idResource,'001', @ApplicationCode, SUBSTRING('Res Default-Access', 0, 90), SUBSTRING('RES Default-Access ' + @ApplicationName, 0, 500),1, 1, getdate(), getdate());
			END

			PRINT ' - Inserindo a permissão com o ID: ' + cast(@idPermission as varchar(max));
			Insert Into [dbo].[THPermission] values (@idPermission, @idRole, @idResource, 1, 1, getdate(), getdate());
			
			
			IF (LOWER(@ApplicationName) = 'hagrid-ui-admin' AND LOWER(@StoreName) = 'hagrid')
			BEGIN
			
				Set @idResourceDadosUser = NewId()
				Set @idPermissionDadosUser = NewId()
				
				PRINT ' - Inserindo o Recurso de alteração de dados do usuário com o ID: ' + cast(@idResourceDadosUser as varchar(max)) + '; Nome: ' + SUBSTRING('Resource Default-Dados-Usuarios', 0, 90);
				Insert into [dbo].[THResource] values (@idResourceDadosUser,'001', @ApplicationCode, SUBSTRING('Res Default-Dados-Usuarios', 0, 90), SUBSTRING('RES Default-Dados-Usuarios ' + @ApplicationName, 0, 500), 4,1, getdate(), getdate());

				PRINT ' - Inserindo a permissão de alteração de dados do usuário com o ID: ' + cast(@idPermissionDadosUser as varchar(max));
				Insert Into [dbo].[THPermission] values (@idPermissionDadosUser, @idRole, @idResourceDadosUser, 4, 1, getdate(), getdate());

			END
			
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