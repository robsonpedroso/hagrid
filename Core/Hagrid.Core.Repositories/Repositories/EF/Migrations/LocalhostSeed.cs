using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF.Migrations
{
    internal class LocalhostSeed
    {
        CustomerContext context;
        public LocalhostSeed(CustomerContext context)
        {
            this.context = context;
        }

        public void AddRole()
        {
            var allOperations = Operations.Approval | Operations.Edit | Operations.Insert | Operations.Remove | Operations.View;

            context.Roles.AddOrUpdate(x => x.Code,
                new Role()
                {
                    Code = "954f1276-201e-4d1c-a11e-079c896d7cb9".AsGuid(),
                    Description = "Administradores do Hagrid",
                    Name = "Administradores Hagrid-UI-Admin",
                    Status = true,
                    StoreCode = "D8671847-06AD-4FAC-BE37-321616969F4C".AsGuid(),
                    SaveDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Type = RoleType.StoreAdmin,
                    Permissions = new List<Permission>()
                    {
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Default-Dados-Usuarios",
                                Name = "Default-Dados-Usuarios",
                                InternalCode = "01-000",
                                SaveDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                Operations = Operations.Edit,
                                Type = ResourceType.ApplicationAccess
                            },
                            SaveDate = DateTime.Now,
                            UpdateDate =  DateTime.Now,
                            Operations = Operations.Edit,
                            Status = true
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Usuários",
                                Name = "Usuários",
                                InternalCode = "01-001",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Usuários - Permissões Aplicações",
                                Name = "Usuários - Permissões Aplicações",
                                InternalCode = "01-004",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Usuários - Bloqueios",
                                Name = "Usuários - Bloqueios",
                                InternalCode = "01-002",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Usuários - Importação",
                                Name = "Usuários - Importação",
                                InternalCode = "01-003",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Lojas - Permissões",
                                Name = "Lojas - Permissões",
                                InternalCode = "03-002",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Lojas",
                                Name = "Lojas",
                                InternalCode = "03-001",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Metadados",
                                Name = "Metadados",
                                InternalCode = "04-001",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Grupos",
                                Name = "Grupos",
                                InternalCode = "02-002",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Permissões",
                                Name = "Permissões",
                                InternalCode = "02-001",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        },
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Resource = new Resource()
                            {
                                Code = Guid.NewGuid(),
                                ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(),
                                Description = "Módulos",
                                Name = "Módulos",
                                InternalCode = "02-003",
                                SaveDate = DateTime.Now,
                                Operations = allOperations,
                                UpdateDate = DateTime.Now,
                                Type = ResourceType.ApplicationAccess
                            },
                            Operations = allOperations,
                            Status = true,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        }
                    }
                }
            );
        }

        public void AddApplication()
        {
            context.Applications.AddOrUpdate(x => x.Name,
                new Application(43200) { Code = "167658F7-57DA-47AC-A692-705F542D8593".AsGuid(), Name = "Hagrid-UI-Login", AuthType = AuthType.Unified, MemberType = MemberType.Consumer, Status = true, SaveDate = DateTime.Now, UpdateDate = DateTime.Now },
                new Application(300) { Code = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid(), Name = "Hagrid-UI-Admin", AuthType = AuthType.Unified, MemberType = MemberType.Merchant, Status = true, SaveDate = DateTime.Now, UpdateDate = DateTime.Now }
            );
        }

        public void AddStore()
        {
            context.Stores.AddOrUpdate(x => x.Code, new Store { Code = "D8671847-06AD-4FAC-BE37-321616969F4C".AsGuid(), Name = "Hagrid Company Ltda", IsMain = true, Status = true, Cnpj = "00000000000000", SaveDate = DateTime.Now, UpdateDate = DateTime.Now });
            context.Stores.AddOrUpdate(x => x.Code, new Store { Code = "C844BC13-001B-44CB-87C0-D43CAB14035A".AsGuid(), Name = "Zumbi", IsMain = false, Status = true, Cnpj = "91543174000156", SaveDate = DateTime.Now, UpdateDate = DateTime.Now });
        }

        public void AddApplicationStore()
        {
            context.ApplicationsStores.AddOrUpdate(x => x.Code,
                new ApplicationStore
                {
                    Code = "448BBD99-1E43-46EA-AA37-843C4250757F".AsGuid(),
                    ConfClient = "415FDF9E-8350-4D88-9982-E89431F87B5F".AsGuid(),
                    ConfSecret = "7F91462A2CFE",
                    JSClient = "184FD2B1-3AF3-4E4A-B9EE-E4B3A668B46F".AsGuid(),
                    JSAllowedOrigins = "http://localhost:4201", // separado por virgula
                    Status = true,
                    StoreCode = "D8671847-06AD-4FAC-BE37-321616969F4C".AsGuid(),
                    ApplicationCode = "0068CD01-B53F-4764-AA47-38E943625011".AsGuid()
                },

                new ApplicationStore
                {
                    Code = "48B83201-83DA-47D7-8373-42300A838FBD".AsGuid(),
                    ConfClient = "06A2C864-5434-450F-9B2F-7DA150D54436".AsGuid(),
                    ConfSecret = "232AE303F00A",
                    JSClient = "1B1EAD63-D787-4438-94C2-8240FAFBC647".AsGuid(),
                    JSAllowedOrigins = null,
                    Status = true,
                    StoreCode = "D8671847-06AD-4FAC-BE37-321616969F4C".AsGuid(),
                    ApplicationCode = "167658F7-57DA-47AC-A692-705F542D8593".AsGuid()
                },

                new ApplicationStore
                {
                    Code = "831151F4-FDCE-457A-9AE5-28E715C455EB".AsGuid(),
                    ConfClient = "F71775EB-FF36-4FD7-95DB-D1BF9D4570CB".AsGuid(),
                    ConfSecret = "E5C2C77E0F9C",
                    JSClient = "04CCCF35-534D-4BF9-A146-53638C054180".AsGuid(),
                    JSAllowedOrigins = "http://localhost:55777",
                    Status = true,
                    StoreCode = "D8671847-06AD-4FAC-BE37-321616969F4C".AsGuid(),
                    ApplicationCode = "167658F7-57DA-47AC-A692-705F542D8593".AsGuid()
                },

                new ApplicationStore
                {
                    Code = "7738C154-CCFE-4189-9739-1414B32DCFB0".AsGuid(),
                    ConfClient = "87C63207-5237-4FB1-A7F2-6C99C13492F0".AsGuid(),
                    ConfSecret = "14B32DCFB0",
                    JSClient = "A63DC839-6FF0-49FD-B97C-558F0C4BEE73".AsGuid(),
                    JSAllowedOrigins = null,
                    Status = true,
                    StoreCode = "C844BC13-001B-44CB-87C0-D43CAB14035A".AsGuid(),
                    ApplicationCode = "167658F7-57DA-47AC-A692-705F542D8593".AsGuid()
                });                
        }

        public void AddUser()
        {
            var applicationStoreCollection = context.ApplicationsStores.ToList();

            context.Accounts.AddOrUpdate(x => x.Code,
                AddUserZumbi(applicationStoreCollection),
                AddUserRobson(applicationStoreCollection),
                AdduserCompany(applicationStoreCollection)
            );
        }

        public void AddStoreAccount()
        {
            context.StoreAccount.AddOrUpdate(x => x.Code, new StoreAccount() { AccountCode = "1A6E2BE5-6F50-413E-B379-7869BBA162F7".AsGuid(), StoreCode = "D8671847-06AD-4FAC-BE37-321616969F4C".AsGuid(), SaveDate = DateTime.Now, UpdateDate = DateTime.Now });
            context.StoreAccount.AddOrUpdate(x => x.Code, new StoreAccount() { AccountCode = "A3A593CC-8CD5-4A0D-86FB-391C35FAF952".AsGuid(), StoreCode = "D8671847-06AD-4FAC-BE37-321616969F4C".AsGuid(), SaveDate = DateTime.Now, UpdateDate = DateTime.Now });

            context.StoreAccount.AddOrUpdate(x => x.Code, new StoreAccount() { AccountCode = "1A6E2BE5-6F50-413E-B379-7869BBA162F7".AsGuid(), StoreCode = "C844BC13-001B-44CB-87C0-D43CAB14035A".AsGuid(), SaveDate = DateTime.Now, UpdateDate = DateTime.Now });
            context.StoreAccount.AddOrUpdate(x => x.Code, new StoreAccount() { AccountCode = "A3A593CC-8CD5-4A0D-86FB-391C35FAF952".AsGuid(), StoreCode = "C844BC13-001B-44CB-87C0-D43CAB14035A".AsGuid(), SaveDate = DateTime.Now, UpdateDate = DateTime.Now });
        }

        public Account AddUserZumbi(List<ApplicationStore> applicationStoreCollection)
        {
            var account = new Account
            {
                Code = "A3A593CC-8CD5-4A0D-86FB-391C35FAF952".AsGuid(),
                Login = "zumbi@hagrid.com.br",
                Password = "thMpFmDgrGAIZG8wGYaM9A==",
                Email = "zumbi@hagrid.com.br",
                Document = "33573300910",
                QtyWrongsPassword = 0,
                LockedUp = null,
                Status = true,
                Removed = false,
                SaveDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsResetPasswordNeeded = false,
                OtherPasswordType = PasswordEncryptionType.Default,
                FacebookId = null,
                AccountApplicationStoreCollection = GetAccountApplicationStores(applicationStoreCollection),
                Customer = new Person()
                {
                    Type = CustomerType.Person,
                    Status = true,
                    FirstName = "Zumbi",
                    LastName = "Teste",
                    Email = "zumbi@hagrid.com.br",
                    Cpf = "33573300910",
                    Rg = "123456789",
                    Gender = "M",
                    BirthDate = new DateTime(1990, 01, 01),
                    Addresses = new List<AddressCustomer>(){
                            new AddressCustomer(){
                                Type = AddressType.HomeAddress,
                                Purpose = AddressPurposeType.Contact,
                                Street = "Rua Brasil",
                                Number = "123",
                                District = "Paulista",
                                City = "São Paulo",
                                State = "SP",
                                Country = "Brasil",
                                ZipCode = "01010-100",
                                Status = true,
                                Phones = new List<Phone>(){
                                    new Phone(){
                                        CodeCountry = "55",
                                        PhoneType = PhoneType.Residencial,
                                        DDD = "11",
                                        Number = "4000-0000"
                                    },
                                    new Phone(){
                                        CodeCountry = "55",
                                        PhoneType = PhoneType.Celular,
                                        DDD = "11",
                                        Number = "99999-9999"
                                    }
                                }
                            }
                        },
                }
            };

            account.Customer.AddressData = account.Customer.Addresses.Serialize().RemoveHeaderXML();

            return account;
        }

        public Account AddUserRobson(List<ApplicationStore> applicationStoreCollection)
        {
            var account = new Account
            {
                Code = "1A6E2BE5-6F50-413E-B379-7869BBA162F7".AsGuid(),
                Login = "robson.pedroso@hagrid.com.br",
                Password = "thMpFmDgrGC3UKNpPZF9jA==",
                Email = "robson.pedroso@hagrid.com.br",
                Document = "42982533766",
                QtyWrongsPassword = 0,
                LockedUp = null,
                Status = true,
                Removed = false,
                SaveDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsResetPasswordNeeded = false,
                OtherPasswordType = PasswordEncryptionType.Default,
                FacebookId = null,
                AccountApplicationStoreCollection = GetAccountApplicationStores(applicationStoreCollection),
                AccountRoles = GetAccountRoles(),
                Customer = new Person()
                {
                    Type = CustomerType.Person,
                    Status = true,
                    FirstName = "Robson",
                    LastName = "Pedroso",
                    Email = "robson.pedroso@hagrid.com.br",
                    Cpf = "42982533766",
                    Rg = "123456789",
                    Gender = "M",
                    BirthDate = new DateTime(1987, 12, 17),
                    Addresses = new List<AddressCustomer>(){
                        new AddressCustomer(){
                            Type = AddressType.HomeAddress,
                            Purpose = AddressPurposeType.Contact,
                            Street = "Rua Brasil",
                            Number = "123",
                            District = "Paulista",
                            City = "São Paulo",
                            State = "SP",
                            Country = "Brasil",
                            ZipCode = "01010-100",
                            Status = true,
                            Phones = new List<Phone>(){
                                new Phone(){
                                    CodeCountry = "55",
                                    PhoneType = PhoneType.Residencial,
                                    DDD = "11",
                                    Number = "4000-0000"
                                },
                                new Phone(){
                                    CodeCountry = "55",
                                    PhoneType = PhoneType.Celular,
                                    DDD = "11",
                                    Number = "90000-0000"
                                }
                            }
                        }
                    },
                }
            };

            account.Customer.AddressData = account.Customer.Addresses.Serialize().RemoveHeaderXML();

            return account;
        }

        public Account AdduserCompany(List<ApplicationStore> applicationStoreCollection)
        {
            var account = new Account
            {
                Code = "0051F09D-4419-4323-B39D-50FEEFAEA192".AsGuid(),
                Login = "company@hagrid.com.br",
                Password = "L9r8kI8ngJk=",
                Email = "company@hagrid.com.br",
                Document = "99705049000107",
                QtyWrongsPassword = 0,
                LockedUp = null,
                Status = true,
                Removed = false,
                SaveDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsResetPasswordNeeded = false,
                OtherPasswordType = PasswordEncryptionType.Default,
                FacebookId = null,
                AccountApplicationStoreCollection = GetAccountApplicationStores(applicationStoreCollection),
                Customer = new Company()
                {
                    Type = CustomerType.Company,
                    Status = true,
                    Email = "company@hagrid.com.br",
                    Cnpj = "01303446000158",
                    CompanyName = "Hagrid Company",
                    TradeName = "Hagrid Company Ltda",
                    Ie = "6527353506",
                    Im = "2411298433",
                    Addresses = new List<AddressCustomer>(){
                        new AddressCustomer(){
                            Type = AddressType.Comercial,
                            Purpose = AddressPurposeType.Shipping,
                            Street = "Avenida Brasil",
                            Number = "100",
                            District = "Paulista",
                            City = "São Paulo",
                            State = "SP",
                            Name = "Entrega",
                            ContactName = "",
                            Country = "Brasil",
                            ZipCode = "01010-100",
                            Complement = "",
                            Status = true,
                            Phones = new List<Phone>(){
                                new Phone(){
                                    CodeCountry = "55",
                                    PhoneType = PhoneType.Comercial,
                                    DDD = "11",
                                    Number = "4000-0000"
                                }
                            }
                        }
                    },
                }
            };

            account.Customer.AddressData = account.Customer.Addresses.Serialize().RemoveHeaderXML();

            return account;
        }

        public List<AccountApplicationStore> GetAccountApplicationStores(List<ApplicationStore> applicationStoreCollection)
        {
            return applicationStoreCollection.Where(x =>
                    x.Code == "448BBD99-1E43-46EA-AA37-843C4250757F".AsGuid() ||
                    x.Code == "831151F4-FDCE-457A-9AE5-28E715C455EB".AsGuid())
                    .Select(x => new AccountApplicationStore()
                    {
                        ApplicationStoreCode = x.Code,
                        ApplicationStore = x,
                        Code = Guid.NewGuid(),
                        SaveDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    })
                    .ToList();
        }

        public List<AccountRole> GetAccountRoles()
        {
            return new List<AccountRole>() {
                    new AccountRole()
                    {
                        Code = Guid.NewGuid(),
                        SaveDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Status = true,
                        RoleCode = "954f1276-201e-4d1c-a11e-079c896d7cb9".AsGuid()
                    }
            };
        }

    }
}
