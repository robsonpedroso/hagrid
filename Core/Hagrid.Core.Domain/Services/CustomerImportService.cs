using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using System.Configuration;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Enums;

namespace Hagrid.Core.Domain.Services
{
    public class CustomerImportService : ICustomerImportService
    {
        private ICustomerImportRepository customerImportRepository;
        private IPasswordPolicy passwordPolicy;
        private readonly IIORepository ioRepository;
        private ICustomerImportFileInfraService customerImportInfraService;
        private IRequisitionRepository requisitionRepository;

        public CustomerImportService(ICustomerImportRepository customerImportRepository, IPasswordPolicy passwordPolicy, IIORepository ioRepository, ICustomerImportFileInfraService customerImportInfraService, IRequisitionRepository requisitionRepository)
        {
            this.customerImportRepository = customerImportRepository;
            this.passwordPolicy = passwordPolicy;
            this.ioRepository = ioRepository;
            this.customerImportInfraService = customerImportInfraService;
            this.requisitionRepository = requisitionRepository;
        }

        public bool Exists(string username, Guid storeCode)
        {
            CustomerImport customerImport = Get(username, storeCode);

            if (!customerImport.IsNull() && !customerImport.AccountCode.IsEmpty())
                return true;
            else
                return false;
        }

        public CustomerImport Get(string username, Guid storeCode)
        {
            CustomerImport customerImport = null;

            if (username.IsValidEmail())
                customerImport = customerImportRepository.GetByEmail(username, storeCode);
            else if (username.IsValidCPF())
                customerImport = customerImportRepository.GetByCPF(username, storeCode);
            else if (username.IsValidCNPJ())
                customerImport = customerImportRepository.GetByCNPJ(username, storeCode);

            return customerImport;
        }

        public void RemoveMember(string email, string document)
        {
            var customers = new List<CustomerImport>();

            customers.AddRange(customerImportRepository.GetByEmail(email));

            if (document.IsValidCPF())
                customers.AddRange(customerImportRepository.GetByCPF(document));
            else if (document.IsValidCNPJ())
                customers.AddRange(customerImportRepository.GetByCNPJ(document));


            customers.ForEach(customer =>
            {
                customer.Password = string.Empty;
                customer.Removed = true;
                customer.UpdateDate = DateTime.Now;
                customerImportRepository.Update(customer);
            });
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() { customerImportRepository, requisitionRepository };
        }

        #endregion
    }
}
