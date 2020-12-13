using System;
using System.Configuration;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.ValueObjects
{
    public class Config
    {
        public static EnvironmentType Environment = ConfigurationManager.AppSettings["Environment"].ToEnum<EnvironmentType>(true);

        public static string ConnectionStringName = ConfigurationManager.AppSettings["ConnectionStringName"].AsString();

        public static string TokenExpirationTimeInMinutes = ConfigurationManager.AppSettings["TokenExpirationTimeInMinutes"].AsString();

        public static string[] AdminApplications = ConfigurationManager.AppSettings["AdminApplications"].Split(new char[] { ',' }, StringSplitOptions.None);

        public static string AccountApiBaseURL = ConfigurationManager.AppSettings["AccountApiBaseURL"];

        public static Guid AccountAdminClientId = ConfigurationManager.AppSettings["AccountAdminClientId"].AsGuid();

        public static string AccountAdminSecret = ConfigurationManager.AppSettings["AccountAdminSecret"];

        public static Guid CustomerDashboardClientId = ConfigurationManager.AppSettings["CustomerDashboardClientId"].AsGuid();

        public static string CustomerDashboardSecret = ConfigurationManager.AppSettings["CustomerDashboardSecret"];

        public static string JwtTokenKey = ConfigurationManager.AppSettings["JwtTokenKey"];

        public static byte[] JwtTokenKeyByte = System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtTokenKey"]);

        public static bool ProcessImportCustomerFull = ConfigurationManager.AppSettings["ProcessImportCustomerFull"].AsBool(false);

        public static int ProcessImportNumberRecordsPerCommit = ConfigurationManager.AppSettings["ProcessImportNumberRecordsPerCommit"].AsInt(1000);

        public static int ProcessImportMaxDegreeOfParallelism = ConfigurationManager.AppSettings["ProcessImportMaxDegreeOfParallelism"].AsInt(8);

        public static int TransferTokenExpiresUtc = ConfigurationManager.AppSettings["TransferTokenExpiresUtc"].AsInt(90);        

        public static string ResetPasswordSMSMessage = ConfigurationManager.AppSettings["ResetPasswordSMSMessage"];

        public static int SecondsToRegeneratePasswordRecovery = ConfigurationManager.AppSettings["SecondsToRegeneratePasswordRecovery"].AsInt(30);

        public static int MaximumConsecutiveWrongLoginAttempts = ConfigurationManager.AppSettings["MaximumConsecutiveWrongLoginAttempts"].AsInt(5);

        public static int LockedTimeInMinutes = ConfigurationManager.AppSettings["LockedTimeInMinutes"].AsInt(30);

        public static string AccountsSiteURL = ConfigurationManager.AppSettings["AccountsSiteURL"].AsString();

        public static string DirRoot = ConfigurationManager.AppSettings["DirRoot"].AsString("");

        public static string UrlRoot = ConfigurationManager.AppSettings["UrlRoot"].AsString("");

        public static string DirImages = ConfigurationManager.AppSettings["DirImages"].AsString("");

        public static double ResetPasswordSMSExpiresUtc = ConfigurationManager.AppSettings["ResetPasswordSMSExpiresUtc"].AsDouble();

        public static int ChangePassworkTokenExpirationTimeInSeconds = ConfigurationManager.AppSettings["ChangePassworkTokenExpirationTimeInSeconds"].AsInt(120);

        public static int ChangePasswordTokenExpirationTimeInHours = ConfigurationManager.AppSettings["ChangePasswordTokenExpirationTimeInHours"].AsInt(24);

        public static string PermissionsShowConfSecret = ConfigurationManager.AppSettings["PermissionsShowConfSecret"].AsString("");

        public static string[] AllowedRedirectUris = ConfigurationManager.AppSettings["AllowedRedirectUris"].Split('|');

        public static int ClientsImportIntervalInMinutes = ConfigurationManager.AppSettings["ClientsImportIntervalInMinutes"].AsInt();

        public static string PermissionNameRole = "Administradores";

        public static string PermissionNameResourceData = "Default-Dados-Usuarios";
    }
}
