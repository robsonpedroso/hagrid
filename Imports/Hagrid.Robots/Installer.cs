using Hagrid.Infra.Utils;
using System.ComponentModel;

namespace Hagrid.Robots
{
    [RunInstaller(true)]
    public class Installer : ServiceInstallerBase
    {
        public Installer()
        {
            InitializeComponent(RobotConfig.ServiceName, RobotConfig.DisplayName, RobotConfig.Description);
        }
    }
}
