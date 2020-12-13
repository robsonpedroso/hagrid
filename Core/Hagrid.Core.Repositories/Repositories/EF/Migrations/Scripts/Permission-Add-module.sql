--Hagrid-UI-Admin: localhost: '0068CD01-B53F-4764-AA47-38E943625011'

declare @codeApp uniqueidentifier = '0068CD01-B53F-4764-AA47-38E943625011'

insert into THResource values (NEWID(), @codeApp, 'Usuários', 'Usuários', 15, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Usuários - Permissões Aplicações', 'Usuários - Permissões Aplicações', 11, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Usuários - Bloqueios', 'Usuários - Bloqueios', 11, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Usuários - Importação', 'Usuários - Importação', 11, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Lojas - Permissões', 'Lojas - Permissões', 1, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Lojas', 'Lojas', 7, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Metadados', 'Metadados', 31, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Grupos', 'Grupos', 31, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Permissões', 'Permissões', 31, GETDATE(), GETDATE())
insert into THResource values (NEWID(), @codeApp, 'Módulos', 'Módulos', 31, GETDATE(), GETDATE())