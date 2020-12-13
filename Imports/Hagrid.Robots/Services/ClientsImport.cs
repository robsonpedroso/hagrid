
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.IoC;
using Hagrid.Core.IoC.Autofac;
using Hagrid.Infra.Logging.Slack;
using Hagrid.Infra.Utils;
using System;
using System.Timers;
using Autofac;
using Hagrid.Core.Application.Contracts;

namespace Hagrid.Robots.Services
{
    partial class ClientsImport : RobotBase
    {
        public const string _serviceName = "HagridRobots";
        public const string _displayName = "Hagrid Robots";
        public const string _description = "Service that import clients";

        private SlackMessager<ClientsImport> slack;

        public ClientsImport()
        {
            base.InitializeComponent();

            base.ServiceName = _serviceName;
            base.DisplayName = _displayName;
            base.Description = _description;

            Bootstrap.Go();
            slack = new SlackMessager<ClientsImport>();
        }

        protected override void OnStart(string[] args)
        {
            base.InitTimer(5);
        }

        protected override void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            using (var scope = Current.Container.BeginLifetimeScope())
            {
                try
                {
                    var app = scope.Resolve<IRequisitionApplication>();

                    slack.GenerateCorrelationID();

                    timer.Stop();
                    timer.Enabled = false;

                    slack.Info("Started to run Hagrid Robots.");

                    app.Process();

                    slack.Info("Finished to run Hagrid Robots.");
                }
                catch (Exception ex)
                {
                    slack.Error(ex);
                }
                finally
                {
                    base.InitTimer(Config.ClientsImportIntervalInMinutes * 60);
                }

            }
        }
    }
}
