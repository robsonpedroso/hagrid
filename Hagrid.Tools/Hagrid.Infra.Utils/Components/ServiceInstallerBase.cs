using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Text;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// Service Installer Base
    /// </summary>
    [RunInstaller(true)]
    public class ServiceInstallerBase : Installer
    {
        private IContainer components = null;
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
        private static string exmple = "Usage:\ninstallutil /i /username=<user_name> /password=<user_password> NameRobot.exe";

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected void InitializeComponent(string serviceName, string displayName, string description)
        {
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();

            this.serviceInstaller.ServiceName = serviceName;
            this.serviceInstaller.DisplayName = displayName;
            this.serviceInstaller.Description = description;

            this.serviceInstaller.StartType = ServiceStartMode.Automatic;

            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.serviceInstaller});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            string userName = this.Context.Parameters["username"];
            if (userName == null)
            {
                Console.WriteLine(exmple);
                throw new InstallException("Missing parameter 'username'");
            }

            string userPass = this.Context.Parameters["password"];
            if (userPass == null)
            {
                Console.WriteLine(exmple);
                throw new InstallException("Missing parameter 'password'");
            }

            this.serviceProcessInstaller.Username = userName;
            this.serviceProcessInstaller.Password = userPass;

            var path = new StringBuilder(Context.Parameters["assemblypath"]);
            if (path[0] != '"')
            {
                path.Insert(0, '"');
                path.Append('"');
            }
            path.Append(" --service");
            Context.Parameters["assemblypath"] = path.ToString();
            base.Install(stateSaver);
        }
    }
}
