using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Application
{
    public class AccountBaseApplication : IBaseApplication
    {
        public IConnection Connection { get; set; }

        public INotifier Notifier { get; set; }

        protected readonly IComponentContext context;

        public AccountBaseApplication(IComponentContext context)
        {
            if (!context.IsNull())
            {
                if(context.TryResolve(typeof(IConnection), out var connection))
                    Connection = connection as IConnection;

                if (context.TryResolve(typeof(INotifier), out var notifier))
                    Notifier = notifier as INotifier;

                this.context = context;
            }
        }
    }
}