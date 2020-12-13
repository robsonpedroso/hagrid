using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hagrid.Core.Domain.Services.Requisitions
{
    public class CustomerImportFileService : IRequisitionProcessingService
    {
        private ICustomerImportRepository customerImportRepository;
        private readonly IIORepository ioRepository;
        private ICustomerImportFileInfraService customerImportFileInfraService;
        private IRequisitionRepository requisitionRepository;
        private IAccountRepository accountRepository;

        public CustomerImportFileService(ICustomerImportRepository customerImportRepository, IIORepository ioRepository, ICustomerImportFileInfraService customerImportFileInfraService, IRequisitionRepository requisitionRepository, IAccountRepository accountRepository)
        {
            this.customerImportRepository = customerImportRepository;
            this.ioRepository = ioRepository;
            this.customerImportFileInfraService = customerImportFileInfraService;
            this.requisitionRepository = requisitionRepository;
            this.accountRepository = accountRepository;
        }

        public void ImportCustomerToImportDb(Requisition requisition)
        {
            throw new NotImplementedException();
        }

        public void ClearImportDb(Requisition requisition)
        {
            throw new NotImplementedException();
        }

        public ApplicationStore GetApplicationStore(Guid storeCode)
        {
            throw new NotImplementedException();
        }

        public object[] GetAccounts(Requisition requisition, int skip, int take)
        {
            var fileRequisition = requisition as FileRequisition;
            var listClients = new List<CustomerImport>();

            string[] lines = ioRepository.ReadAllLines(
                fileRequisition.Code.ToString(),
                fileRequisition.Dir,
                fileRequisition.FileExtension,
                Encoding.Default
            );

            return lines;
        }

        public bool SaveAccount(Requisition requisition, Object account, ApplicationStore applicationStore, IConnection connection)
        {
            var fileRequisition = requisition as FileRequisition;

            var line = (string)account;

            var properties = line.Split(new char[] { '\t' });
            var message = new List<string>();
            bool isValid = true;

            try
            {
                var result = customerImportFileInfraService.ValidCustomer(properties, fileRequisition);

                if (result.Item1)
                {
                    CustomerImport customer = result.Item3;
                    var clientValid = customer.isValid();

                    if (clientValid.Item1)
                    {
                        try
                        {
                            if (customer is CompanyImport)
                            {
                                var company = (CompanyImport)customer;
                            }
                            else
                            {
                                var person = (PersonImport)customer;
                            }
                        }
                        catch (Exception ex)
                        {
                            isValid = false;
                            message.Add(ex.Message);
                        }

                        if (isValid)
                        {
                            if (!IsMemberExists(customer))
                            {
                                customerImportRepository.Save(customer);
                            }
                            else
                            {
                                isValid = false;
                                message.Add("Usuário já existe");
                            }
                        }
                    }
                    else
                    {
                        isValid = false;
                        message.AddRange(clientValid.Item2);
                    }
                }
                else
                {
                    isValid = false;
                    message.AddRange(result.Item2);
                }


            }
            catch (Exception ex)
            {
                isValid = false;
                message.Add(string.Format("{0}", ex.TrimMessage()));
            }

            if (!isValid)
            {
                requisition.RequisitionErrors.Add(new RequisitionError()
                {
                    Code = Guid.NewGuid(),
                    ErrorMessages = message,
                    Email = properties.Count() > ConstantsFileImport.Email ? properties[ConstantsFileImport.Email] : string.Empty,
                    Name = properties.Count() > ConstantsFileImport.FirstName ? properties[ConstantsFileImport.FirstName] : string.Empty
                });
            }

            return isValid;
        }

        private bool IsMemberExists(CustomerImport client)
        {
            CustomerImport customerImport = null;
            client.HandleCustomer();

            if (client.Email.IsValidEmail())
                customerImport = customerImportRepository.GetByEmail(client.Email).FirstOrDefault();

            if ((customerImport.IsNull() || customerImport.AccountCode.IsEmpty()) && client.DisplayDocument.IsValidCPF())
                customerImport = customerImportRepository.GetByCPF(client.DisplayDocument).FirstOrDefault();

            if ((customerImport.IsNull() || customerImport.AccountCode.IsEmpty()) && client.DisplayDocument.IsValidCNPJ())
                customerImport = customerImportRepository.GetByCNPJ(client.DisplayDocument).FirstOrDefault();

            if (!customerImport.IsNull() && !customerImport.AccountCode.IsEmpty())
                return true;

            var account = new Account()
            {
                Email = client.Email,
                Document = client.DisplayDocument,
                Login = client.Email
            };

            return accountRepository.IsMemberExists(account); ;
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() {
                customerImportRepository,
                requisitionRepository,
                accountRepository
            };
        }

        private void SetConnection(Infra.Contracts.IDomainService domainService)
        {
            var conn = accountRepository.Connection;
            domainService.GetRepositories().ForEach(r => r.Connection = conn);
        }

        #endregion
    }
}
