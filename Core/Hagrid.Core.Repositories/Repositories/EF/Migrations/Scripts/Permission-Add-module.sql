--Hagrid-UI-Admin: localhost: '0068CD01-B53F-4764-AA47-38E943625011'

declare @codeApp uniqueidentifier = '0068CD01-B53F-4764-AA47-38E943625011'

insert into THResource values (NEWID(), @codeApp, 'Usu�rios', 'Usu�rios', 15, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Usu�rios - Permiss�es Aplica��es', 'Usu�rios - Permiss�es Aplica��es', 11, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Usu�rios - Bloqueios', 'Usu�rios - Bloqueios', 11, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Usu�rios - Importa��o', 'Usu�rios - Importa��o', 11, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Lojas - Permiss�es', 'Lojas - Permiss�es', 1, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Lojas', 'Lojas', 7, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Metadados', 'Metadados', 31, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Grupos', 'Grupos', 31, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Permiss�es', 'Permiss�es', 31, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'M�dulos', 'M�dulos', 31, GETDATE(), GETDATE())