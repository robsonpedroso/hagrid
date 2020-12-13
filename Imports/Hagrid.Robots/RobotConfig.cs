using System.Configuration;
using Hagrid.Infra.Utils;
using System.Reflection;

namespace Hagrid.Robots
{
    public sealed class RobotConfig
    {
        /// <summary>
        /// Gets the ServiceName from System.Configuration.AppSettingsSection data for the current application's default configuration. 
        /// </summary>
        public static string ServiceName
        {
            get
            {
                var serviceName = GetConfigurationValue("ServiceName");
                return (!serviceName.IsNullOrWhiteSpace() ? serviceName : "*** Please specific Name your service on config ***");
            }
        }

        /// <summary>
        /// Gets the DisplayName from System.Configuration.AppSettingsSection data for the current application's default configuration. 
        /// </summary>
        public static string DisplayName
        {
            get
            {
                var displayName = GetConfigurationValue("DisplayName");
                return (!displayName.IsNullOrWhiteSpace() ? displayName : "*** Please specific Name your service on config ***");
            }
        }

        /// <summary>
        /// Gets the Description from System.Configuration.AppSettingsSection data for the current application's default configuration. 
        /// </summary>
        public static string Description
        {
            get
            {
                var description = GetConfigurationValue("Description");
                return (!description.IsNullOrWhiteSpace() ? description : string.Empty);
            }
        }

        /// <summary>
        ///  Opens the configuration file for the current application as a System.Configuration.Configuration object.
        /// </summary>
        /// <param name="key">key to get value</param>
        /// <returns></returns>
        private static string GetConfigurationValue(string key)
        {
            var service = Assembly.GetExecutingAssembly();

            var config = ConfigurationManager.OpenExeConfiguration(service.Location);

            if (!config.AppSettings.Settings[key].IsNull())
                return config.AppSettings.Settings[key].Value;
            else
                return ConfigurationManager.AppSettings[key];
        }
    }
}
