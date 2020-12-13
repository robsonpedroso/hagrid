using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface ICustomerService : IDomainService
    {
        void PrepareToAdd(Customer customer, Guid originStore);

        void PrepareToAddSimplified(Customer customer);

        Customer PrepareToUpdate(Store currentStore, Customer customer, Customer newCustomer);

        Customer GetMember(Guid code);

        Customer GetMember(string document);

        void UpdateMember(Customer member);
    }
}
