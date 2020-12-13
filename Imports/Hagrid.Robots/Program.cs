using Hagrid.Robots.Services;
using System.Collections.Generic;
using System.ServiceProcess;

namespace Hagrid.Robots
{
    static class Program
    {
        static void Main()
        {

#if DEBUG
            var service = new ClientsImport();
            service.StartDebug(null);

            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            var servicesToRun = new List<ServiceBase>();

            switch (RobotConfig.ServiceName)
            {
                case ClientsImport._serviceName:
                    servicesToRun.Add(new ClientsImport());
                    break;
            }

            ServiceBase.Run(servicesToRun.ToArray());
#endif
        }
    }
}
