namespace Hagrid.Core.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.THAccountMetadata",
                c => new
                    {
                        Code_Metadata = c.Guid(nullable: false),
                        Code_Account = c.Guid(nullable: false),
                        Value_Metadata = c.String(),
                        SaveDate_Metadata = c.DateTime(nullable: false),
                        UpdateDate_Metadata = c.DateTime(nullable: false),
                        Code_MetadataField = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Metadata)
                .ForeignKey("dbo.THAccount", t => t.Code_Account, cascadeDelete: true)
                .ForeignKey("dbo.THMetadataField", t => t.Code_MetadataField, cascadeDelete: true)
                .Index(t => t.Code_Account)
                .Index(t => t.Code_MetadataField);
            
            CreateTable(
                "dbo.THAccount",
                c => new
                    {
                        Code_Account = c.Guid(nullable: false),
                        FacebookId_Account = c.String(maxLength: 25),
                        Login_Account = c.String(nullable: false, maxLength: 255),
                        Password_Account = c.String(nullable: false, maxLength: 500),
                        Email_Account = c.String(nullable: false, maxLength: 255),
                        Document_Account = c.String(maxLength: 20),
                        QtyWrongsPassword_Account = c.Int(),
                        LockedUp_Account = c.DateTime(),
                        IsResetPasswordNeeded_Account = c.Boolean(),
                        Status_Account = c.Boolean(nullable: false),
                        Removed_Account = c.Boolean(nullable: false),
                        DataFingerprint_Account = c.String(),
                        SaveDate_Account = c.DateTime(nullable: false),
                        UpdateDate_Account = c.DateTime(nullable: false),
                        OtherPasswordType_Account = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Account)
                .Index(t => t.Login_Account, name: "IX_Account_Login")
                .Index(t => new { t.Email_Account, t.Password_Account, t.Removed_Account, t.Status_Account }, name: "IX_Account_Email")
                .Index(t => t.IsResetPasswordNeeded_Account, name: "IX_Account_IsResetPasswordNeeded");
            
            CreateTable(
                "dbo.THAccountApplicationStore",
                c => new
                    {
                        Code_AccountApplicationStore = c.Guid(nullable: false, identity: true),
                        Code_Account = c.Guid(nullable: false),
                        Code_ApplicationStore = c.Guid(nullable: false),
                        SaveDate_AccountApplicationStore = c.DateTime(nullable: false),
                        UpdateDate_AccountApplicationStore = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_AccountApplicationStore)
                .ForeignKey("dbo.THAccount", t => t.Code_Account)
                .ForeignKey("dbo.THApplicationStore", t => t.Code_ApplicationStore)
                .Index(t => t.Code_Account)
                .Index(t => t.Code_ApplicationStore);
            
            CreateTable(
                "dbo.THApplicationStore",
                c => new
                    {
                        Code_ApplicationStore = c.Guid(nullable: false),
                        Code_Application = c.Guid(nullable: false),
                        Code_Store = c.Guid(nullable: false),
                        ConfClient_ApplicationStore = c.Guid(),
                        ConfSecret_ApplicationStore = c.String(maxLength: 32),
                        JSClient_ApplicationStore = c.Guid(),
                        JSAllowedOrigins_ApplicationStore = c.String(),
                        Status_ApplicationStore = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code_ApplicationStore)
                .ForeignKey("dbo.THApplication", t => t.Code_Application)
                .ForeignKey("dbo.THStore", t => t.Code_Store)
                .Index(t => t.Code_Application)
                .Index(t => t.Code_Store)
                .Index(t => new { t.ConfClient_ApplicationStore, t.ConfSecret_ApplicationStore }, unique: true, name: "IX_ApplicationStore_ConfClient")
                .Index(t => new { t.JSClient_ApplicationStore, t.Status_ApplicationStore }, unique: true, name: "IX_ApplicatioStore_JSClient");
            
            CreateTable(
                "dbo.THApplication",
                c => new
                    {
                        Code_Application = c.Guid(nullable: false),
                        Name_Application = c.String(nullable: false, maxLength: 50),
                        AuthType_Application = c.Int(nullable: false),
                        MemberType_Application = c.Int(nullable: false),
                        RefreshTokenLifeTimeInMinutes = c.Int(nullable: false),
                        Status_Application = c.Boolean(nullable: false),
                        SaveDate_Application = c.DateTime(nullable: false),
                        UpdateDate_Application = c.DateTime(nullable: false),
                        Object_Application = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Application)
                .Index(t => t.Name_Application, name: "IX_Application_Name")
                .Index(t => t.Status_Application, name: "IX_Application_Status");
            
            CreateTable(
                "dbo.THResource",
                c => new
                    {
                        Code_Resource = c.Guid(nullable: false),
                        InternalCode_Resource = c.String(maxLength: 50),
                        Code_Application = c.Guid(nullable: false),
                        Name_Resource = c.String(nullable: false, maxLength: 50),
                        Description_Resource = c.String(maxLength: 300),
                        Operations_Resource = c.Int(nullable: false),
                        Type_Resource = c.Int(nullable: false),
                        SaveDate_Resource = c.DateTime(nullable: false),
                        UpdateDate_Resource = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Resource)
                .ForeignKey("dbo.THApplication", t => t.Code_Application, cascadeDelete: true)
                .Index(t => t.Code_Application);
            
            CreateTable(
                "dbo.THPermission",
                c => new
                    {
                        Code_Permission = c.Guid(nullable: false),
                        Code_Role = c.Guid(nullable: false),
                        Code_Resource = c.Guid(nullable: false),
                        Operations_Permission = c.Int(nullable: false),
                        Status_Permission = c.Boolean(nullable: false),
                        SaveDate_Permission = c.DateTime(nullable: false),
                        UpdateDate_Permission = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Permission)
                .ForeignKey("dbo.THResource", t => t.Code_Resource, cascadeDelete: true)
                .ForeignKey("dbo.THRole", t => t.Code_Role, cascadeDelete: true)
                .Index(t => t.Code_Role)
                .Index(t => t.Code_Resource);
            
            CreateTable(
                "dbo.THRole",
                c => new
                    {
                        Code_Role = c.Guid(nullable: false),
                        Code_Store = c.Guid(nullable: false),
                        Name_Role = c.String(nullable: false, maxLength: 50),
                        Description_Role = c.String(nullable: false, maxLength: 300),
                        Status_Role = c.Boolean(nullable: false),
                        Type_Role = c.Int(nullable: false),
                        SaveDate_Role = c.DateTime(nullable: false),
                        UpdateDate_Role = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Role)
                .ForeignKey("dbo.THStore", t => t.Code_Store, cascadeDelete: true)
                .Index(t => t.Code_Store);
            
            CreateTable(
                "dbo.THAccountRole",
                c => new
                    {
                        Code_AccountRole = c.Guid(nullable: false),
                        Code_Account = c.Guid(nullable: false),
                        Code_Role = c.Guid(nullable: false),
                        Status_AccountRole = c.Boolean(nullable: false),
                        SaveDate_AccountRole = c.DateTime(nullable: false),
                        UpdateDate_AccountRole = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_AccountRole)
                .ForeignKey("dbo.THAccount", t => t.Code_Account, cascadeDelete: true)
                .ForeignKey("dbo.THRole", t => t.Code_Role, cascadeDelete: true)
                .Index(t => t.Code_Account)
                .Index(t => t.Code_Role);
            
            CreateTable(
                "dbo.THRestriction",
                c => new
                    {
                        Code_Restriction = c.Guid(nullable: false),
                        Code_Role = c.Guid(nullable: false),
                        SaveDate_Restriction = c.DateTime(nullable: false),
                        UpdateDate_Restriction = c.DateTime(nullable: false),
                        Object_Requisition = c.String(nullable: false),
                        Type_Restriction = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Restriction)
                .ForeignKey("dbo.THRole", t => t.Code_Role, cascadeDelete: true)
                .Index(t => t.Code_Role);
            
            CreateTable(
                "dbo.THStore",
                c => new
                    {
                        Code_Store = c.Guid(nullable: false),
                        Name_Store = c.String(nullable: false, maxLength: 50),
                        Cnpj_Store = c.String(maxLength: 14),
                        IsMain_Store = c.Boolean(nullable: false),
                        Status_Store = c.Boolean(nullable: false),
                        SaveDate_Store = c.DateTime(nullable: false),
                        UpdateDate_Store = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Store)
                .Index(t => t.Name_Store, name: "IX_Store_NameStore")
                .Index(t => t.Status_Store, name: "IX_Store_Status");
            
            CreateTable(
                "dbo.THStoreAddress",
                c => new
                    {
                        Code = c.Guid(nullable: false, identity: true),
                        Code_Store = c.Guid(nullable: false),
                        AddressIdentifier_StoreAddress = c.String(maxLength: 50),
                        ContactName_StoreAddress = c.String(nullable: false, maxLength: 100),
                        ZipCode_StoreAddress = c.String(nullable: false, maxLength: 100),
                        Street_StoreAddress = c.String(nullable: false, maxLength: 300),
                        Number_StoreAddress = c.String(nullable: false, maxLength: 50),
                        Complement_StoreAddress = c.String(maxLength: 100),
                        District_StoreAddress = c.String(nullable: false, maxLength: 100),
                        City_StoreAddress = c.String(nullable: false, maxLength: 100),
                        State_StoreAddress = c.String(nullable: false, maxLength: 2),
                        PhoneNumber1_StoreAddress = c.String(nullable: false, maxLength: 40),
                        PhoneNumber2_StoreAddress = c.String(maxLength: 40),
                        PhoneNumber3_StoreAddress = c.String(maxLength: 40),
                        Status_StoreAddress = c.Boolean(nullable: false),
                        SaveDate_StoreAddress = c.DateTime(nullable: false),
                        UpdateDate_StoreAddress = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.THStore", t => t.Code_Store, cascadeDelete: true)
                .Index(t => t.Code_Store);
            
            CreateTable(
                "dbo.THStoreMetadata",
                c => new
                    {
                        Code_Metadata = c.Guid(nullable: false),
                        Code_Store = c.Guid(nullable: false),
                        Value_Metadata = c.String(),
                        SaveDate_Metadata = c.DateTime(nullable: false),
                        UpdateDate_Metadata = c.DateTime(nullable: false),
                        Code_MetadataField = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Metadata)
                .ForeignKey("dbo.THMetadataField", t => t.Code_MetadataField, cascadeDelete: true)
                .ForeignKey("dbo.THStore", t => t.Code_Store, cascadeDelete: true)
                .Index(t => t.Code_Store)
                .Index(t => t.Code_MetadataField);
            
            CreateTable(
                "dbo.THMetadataField",
                c => new
                    {
                        Code_MetadataField = c.Guid(nullable: false),
                        JsonId_MetadataField = c.String(nullable: false, maxLength: 50),
                        Name_MetadataField = c.String(nullable: false, maxLength: 256),
                        Type_MetadataField = c.Int(nullable: false),
                        Format_MetadataField = c.Int(nullable: false),
                        Validator_MetadataField = c.String(),
                        SaveDate_MetadataField = c.DateTime(nullable: false),
                        UpdateDate_MetadataField = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_MetadataField)
                .Index(t => t.JsonId_MetadataField, name: "IX_MetadataField_JsonId")
                .Index(t => t.Name_MetadataField, name: "IX_MetadataField_Name");
            
            CreateTable(
                "dbo.THStoreAccount",
                c => new
                    {
                        Code_StoreAccount = c.Guid(nullable: false),
                        Code_Account = c.Guid(nullable: false),
                        Code_Store = c.Guid(nullable: false),
                        SaveDate_StoreAccount = c.DateTime(nullable: false),
                        UpdateDate_StoreAccount = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_StoreAccount)
                .ForeignKey("dbo.THAccount", t => t.Code_Account, cascadeDelete: true)
                .ForeignKey("dbo.THStore", t => t.Code_Store, cascadeDelete: true)
                .Index(t => t.Code_Account)
                .Index(t => t.Code_Store);
            
            CreateTable(
                "dbo.THBlacklist",
                c => new
                    {
                        Code_Blacklist = c.Guid(nullable: false),
                        Code_Account = c.Guid(nullable: false),
                        Code_Store = c.Guid(),
                        SaveDate_Blacklist = c.DateTime(nullable: false),
                        UpdateDate_Blacklist = c.DateTime(nullable: false),
                        Blocked_Blacklist = c.Boolean(nullable: false),
                        Object_Blacklist = c.String(),
                    })
                .PrimaryKey(t => t.Code_Blacklist)
                .ForeignKey("dbo.THStore", t => t.Code_Store)
                .ForeignKey("dbo.THAccount", t => t.Code_Account, cascadeDelete: true)
                .Index(t => t.Code_Account)
                .Index(t => t.Code_Store);
            
            CreateTable(
                "dbo.THCustomer",
                c => new
                    {
                        Code_Customer = c.Guid(nullable: false, identity: true),
                        Email_Customer = c.String(),
                        Password_Customer = c.String(),
                        NewsLetter_Customer = c.Boolean(nullable: false),
                        Address_Customer = c.String(),
                        Code_Store = c.Guid(),
                        Status_Customer = c.Boolean(nullable: false),
                        Removed_Customer = c.Boolean(nullable: false),
                        SaveDate_Customer = c.DateTime(nullable: false),
                        UpdateDate_Customer = c.DateTime(nullable: false),
                        CompanyName_Customer = c.String(),
                        TradeName_Customer = c.String(),
                        CNPJ_Customer = c.String(),
                        IE_Customer = c.String(),
                        IM_Customer = c.String(),
                        FirstName_Customer = c.String(),
                        LastName_Customer = c.String(),
                        CPF_Customer = c.String(),
                        RG_Customer = c.String(maxLength: 50),
                        BirthDate_Customer = c.DateTime(),
                        Sexo_Customer = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Code_Account = c.Guid(),
                        Type_Customer = c.Int(),
                    })
                .PrimaryKey(t => t.Code_Customer)
                .ForeignKey("dbo.THAccount", t => t.Code_Account)
                .Index(t => t.Code_Account);
            
            CreateTable(
                "dbo.THAuditLogs",
                c => new
                    {
                        Code_AuditLogs = c.Guid(nullable: false),
                        Type_AuditLogs = c.Int(nullable: false),
                        ReferenceEntity_AuditLogs = c.String(),
                        ReferenceCode_AuditLogs = c.String(),
                        OldData_AuditLogs = c.String(),
                        NewData_AuditLogs = c.String(),
                        Code_Account = c.Guid(),
                        Code_ApplicationStore = c.Guid(nullable: false),
                        SaveDate_AuditLogs = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_AuditLogs);
            
            CreateTable(
                "dbo.THStoreCreditCard",
                c => new
                    {
                        Code_StoreCreditCard = c.Guid(nullable: false),
                        StoreCode_StoreCreditCard = c.Guid(nullable: false),
                        CNPJ_StoreCreditCard = c.String(nullable: false, maxLength: 14),
                        StoreName_StoreCreditCard = c.String(nullable: false, maxLength: 300),
                        Number_StoreCreditCard = c.String(nullable: false, maxLength: 300),
                        Holder_StoreCreditCard = c.String(nullable: false, maxLength: 300),
                        ExpMonth_StoreCreditCard = c.String(nullable: false, maxLength: 300),
                        ExpYear_StoreCreditCard = c.String(nullable: false, maxLength: 300),
                        SecurityCode_StoreCreditCard = c.String(nullable: false, maxLength: 300),
                        Document_StoreCreditCard = c.String(nullable: false, maxLength: 14),
                        SaveDate_StoreCreditCard = c.DateTime(nullable: false),
                        UpdateDate_StoreCreditCard = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_StoreCreditCard);
            
            CreateTable(
                "dbo.THCustomerImport",
                c => new
                    {
                        AccountCode_CustomerImport = c.Guid(nullable: false),
                        Email_CustomerImport = c.String(),
                        Password_CustomerImport = c.String(),
                        NewsLetter_CustomerImport = c.Boolean(nullable: false),
                        Code_Store = c.Guid(),
                        Status_CustomerImport = c.Boolean(nullable: false),
                        Removed_CustomerImport = c.Boolean(nullable: false),
                        SaveDate_CustomerImport = c.DateTime(nullable: false),
                        UpdateDate_CustomerImport = c.DateTime(nullable: false),
                        Address_CustomerImport = c.String(),
                        QtyWrongsPassword_CustomerImport = c.Int(),
                        LockedUp_CustomerImport = c.DateTime(),
                        CompanyName_CustomerImport = c.String(),
                        TradeName_CustomerImport = c.String(),
                        CNPJ_CustomerImport = c.String(),
                        IE_CustomerImport = c.String(),
                        IM_CustomerImport = c.String(),
                        FirstName_CustomerImport = c.String(),
                        LastName_CustomerImport = c.String(),
                        CPF_CustomerImport = c.String(),
                        RG_CustomerImport = c.String(),
                        BirthDate_CustomerImport = c.DateTime(),
                        Sexo_CustomerImport = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Type_CustomerImport = c.Int(),
                    })
                .PrimaryKey(t => t.AccountCode_CustomerImport);
            
            CreateTable(
                "dbo.THRequisition",
                c => new
                    {
                        Code_Requisition = c.Guid(nullable: false),
                        Status_Requisition = c.Int(nullable: false),
                        RequisitionType = c.Int(nullable: false),
                        Removed_Requisition = c.Boolean(nullable: false),
                        SaveDate_Requisition = c.DateTime(nullable: false),
                        UpdateDate_Requisition = c.DateTime(nullable: false),
                        Object_Requisition = c.String(nullable: false),
                        Code_Store = c.Guid(nullable: false),
                        Type_Requisition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code_Requisition)
                .ForeignKey("dbo.THStore", t => t.Code_Store)
                .Index(t => t.Code_Store);
            
            CreateTable(
                "dbo.THRequisitionError",
                c => new
                    {
                        Code = c.Guid(nullable: false),
                        Object_RequisitionError = c.String(nullable: false),
                        Code_Requisition = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.THRequisition", t => t.Code_Requisition)
                .Index(t => t.Code_Requisition);
            
            CreateTable(
                "dbo.THResetSMSToken",
                c => new
                    {
                        Code_ResetSMSToken = c.String(nullable: false, maxLength: 128),
                        ZenviaCode_ResetSMSToken = c.Guid(nullable: false),
                        UsedDate_ResetSMSToken = c.DateTime(),
                        TokenType_ResetSMSToken = c.String(),
                        PhoneNumber_ResetSMSToken = c.String(),
                        UrlBack_ResetSMSToken = c.String(maxLength: 1024),
                        CodeSMS_ResetSMSToken = c.String(),
                        Code_ApplicationStore = c.Guid(),
                        GeneratedUtc_ResetSMSToken = c.DateTime(nullable: false),
                        Code_Owner = c.Guid(nullable: false),
                        ExpiresUtc_ResetSMSToken = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_ResetSMSToken);
            
            CreateTable(
                "dbo.THRefreshToken",
                c => new
                    {
                        Code_RefreshToken = c.String(nullable: false, maxLength: 200),
                        Ticket_RefreshToken = c.String(nullable: false),
                        Code_ClientApplication = c.Guid(nullable: false),
                        Code_ApplicationStore = c.Guid(),
                        GeneratedUtc_RefreshToken = c.DateTime(nullable: false),
                        Code_Owner = c.Guid(nullable: false),
                        ExpiresUtc_RefreshToken = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_RefreshToken);
            
            CreateTable(
                "dbo.THResetPasswordToken",
                c => new
                    {
                        Code_ResetPasswordToken = c.String(nullable: false, maxLength: 128),
                        Code_ClientApplication = c.Guid(nullable: false),
                        UrlBack_ResetPasswordToken = c.String(maxLength: 1024),
                        Code_ApplicationStore = c.Guid(),
                        GeneratedUtc_ResetPasswordToken = c.DateTime(nullable: false),
                        Code_Owner = c.Guid(nullable: false),
                        ExpiresUtc_ResetPasswordToken = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_ResetPasswordToken);
            
            CreateTable(
                "dbo.THTransferToken",
                c => new
                    {
                        Code_TransferToken = c.String(nullable: false, maxLength: 128),
                        Name_TransferToken = c.String(nullable: false),
                        ClientId_TransferToken = c.Guid(nullable: false),
                        OwnerCode_TransferToken = c.Guid(nullable: false),
                        ExpiresUtc_TransferToken = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Code_TransferToken);
            
            CreateTable(
                "dbo.THPasswordLog",
                c => new
                    {
                        Code_PasswordLog = c.Guid(nullable: false),
                        Code_Account = c.Guid(nullable: false),
                        Event_Account = c.Int(nullable: false),
                        SaveDate_Account = c.DateTime(nullable: false),
                        Code_Store = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Code_PasswordLog);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.THRequisition", "Code_Store", "dbo.THStore");
            DropForeignKey("dbo.THRequisitionError", "Code_Requisition", "dbo.THRequisition");
            DropForeignKey("dbo.THAccountMetadata", "Code_MetadataField", "dbo.THMetadataField");
            DropForeignKey("dbo.THAccountMetadata", "Code_Account", "dbo.THAccount");
            DropForeignKey("dbo.THCustomer", "Code_Account", "dbo.THAccount");
            DropForeignKey("dbo.THBlacklist", "Code_Account", "dbo.THAccount");
            DropForeignKey("dbo.THBlacklist", "Code_Store", "dbo.THStore");
            DropForeignKey("dbo.THAccountApplicationStore", "Code_ApplicationStore", "dbo.THApplicationStore");
            DropForeignKey("dbo.THApplicationStore", "Code_Store", "dbo.THStore");
            DropForeignKey("dbo.THApplicationStore", "Code_Application", "dbo.THApplication");
            DropForeignKey("dbo.THPermission", "Code_Role", "dbo.THRole");
            DropForeignKey("dbo.THRole", "Code_Store", "dbo.THStore");
            DropForeignKey("dbo.THStoreAccount", "Code_Store", "dbo.THStore");
            DropForeignKey("dbo.THStoreAccount", "Code_Account", "dbo.THAccount");
            DropForeignKey("dbo.THStoreMetadata", "Code_Store", "dbo.THStore");
            DropForeignKey("dbo.THStoreMetadata", "Code_MetadataField", "dbo.THMetadataField");
            DropForeignKey("dbo.THStoreAddress", "Code_Store", "dbo.THStore");
            DropForeignKey("dbo.THRestriction", "Code_Role", "dbo.THRole");
            DropForeignKey("dbo.THAccountRole", "Code_Role", "dbo.THRole");
            DropForeignKey("dbo.THAccountRole", "Code_Account", "dbo.THAccount");
            DropForeignKey("dbo.THPermission", "Code_Resource", "dbo.THResource");
            DropForeignKey("dbo.THResource", "Code_Application", "dbo.THApplication");
            DropForeignKey("dbo.THAccountApplicationStore", "Code_Account", "dbo.THAccount");
            DropIndex("dbo.THRequisitionError", new[] { "Code_Requisition" });
            DropIndex("dbo.THRequisition", new[] { "Code_Store" });
            DropIndex("dbo.THCustomer", new[] { "Code_Account" });
            DropIndex("dbo.THBlacklist", new[] { "Code_Store" });
            DropIndex("dbo.THBlacklist", new[] { "Code_Account" });
            DropIndex("dbo.THStoreAccount", new[] { "Code_Store" });
            DropIndex("dbo.THStoreAccount", new[] { "Code_Account" });
            DropIndex("dbo.THMetadataField", "IX_MetadataField_Name");
            DropIndex("dbo.THMetadataField", "IX_MetadataField_JsonId");
            DropIndex("dbo.THStoreMetadata", new[] { "Code_MetadataField" });
            DropIndex("dbo.THStoreMetadata", new[] { "Code_Store" });
            DropIndex("dbo.THStoreAddress", new[] { "Code_Store" });
            DropIndex("dbo.THStore", "IX_Store_Status");
            DropIndex("dbo.THStore", "IX_Store_NameStore");
            DropIndex("dbo.THRestriction", new[] { "Code_Role" });
            DropIndex("dbo.THAccountRole", new[] { "Code_Role" });
            DropIndex("dbo.THAccountRole", new[] { "Code_Account" });
            DropIndex("dbo.THRole", new[] { "Code_Store" });
            DropIndex("dbo.THPermission", new[] { "Code_Resource" });
            DropIndex("dbo.THPermission", new[] { "Code_Role" });
            DropIndex("dbo.THResource", new[] { "Code_Application" });
            DropIndex("dbo.THApplication", "IX_Application_Status");
            DropIndex("dbo.THApplication", "IX_Application_Name");
            DropIndex("dbo.THApplicationStore", "IX_ApplicatioStore_JSClient");
            DropIndex("dbo.THApplicationStore", "IX_ApplicationStore_ConfClient");
            DropIndex("dbo.THApplicationStore", new[] { "Code_Store" });
            DropIndex("dbo.THApplicationStore", new[] { "Code_Application" });
            DropIndex("dbo.THAccountApplicationStore", new[] { "Code_ApplicationStore" });
            DropIndex("dbo.THAccountApplicationStore", new[] { "Code_Account" });
            DropIndex("dbo.THAccount", "IX_Account_IsResetPasswordNeeded");
            DropIndex("dbo.THAccount", "IX_Account_Email");
            DropIndex("dbo.THAccount", "IX_Account_Login");
            DropIndex("dbo.THAccountMetadata", new[] { "Code_MetadataField" });
            DropIndex("dbo.THAccountMetadata", new[] { "Code_Account" });
            DropTable("dbo.THPasswordLog");
            DropTable("dbo.THTransferToken");
            DropTable("dbo.THResetPasswordToken");
            DropTable("dbo.THRefreshToken");
            DropTable("dbo.THResetSMSToken");
            DropTable("dbo.THRequisitionError");
            DropTable("dbo.THRequisition");
            DropTable("dbo.THCustomerImport");
            DropTable("dbo.THStoreCreditCard");
            DropTable("dbo.THAuditLogs");
            DropTable("dbo.THCustomer");
            DropTable("dbo.THBlacklist");
            DropTable("dbo.THStoreAccount");
            DropTable("dbo.THMetadataField");
            DropTable("dbo.THStoreMetadata");
            DropTable("dbo.THStoreAddress");
            DropTable("dbo.THStore");
            DropTable("dbo.THRestriction");
            DropTable("dbo.THAccountRole");
            DropTable("dbo.THRole");
            DropTable("dbo.THPermission");
            DropTable("dbo.THResource");
            DropTable("dbo.THApplication");
            DropTable("dbo.THApplicationStore");
            DropTable("dbo.THAccountApplicationStore");
            DropTable("dbo.THAccount");
            DropTable("dbo.THAccountMetadata");
        }
    }
}
