using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Providers.EntityFramework;
using System;

namespace Hagrid.Core.Providers.EntityFramework.Context
{
    public class CustomerConnection : EFConnection<CustomerContext>, IConnection, IDisposable
    {
        public CustomerConnection(CustomerContext context) : base(context) { }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}