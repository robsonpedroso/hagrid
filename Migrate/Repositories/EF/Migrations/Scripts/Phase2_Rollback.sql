IF  EXISTS (SELECT * FROM DBRKBackOffice.sys.indexes WHERE object_id = OBJECT_ID(N'[DBRKBackOffice].[dbo].[TRKUser]') AND name = N'idx_user_code_account') DROP INDEX [idx_user_code_account] ON [DBRKBackOffice].[dbo].[TRKUser] WITH ( ONLINE = OFF )
GO
ALTER TABLE DBRKBackOffice.dbo.TRKUser DROP COLUMN Code_Account
GO
