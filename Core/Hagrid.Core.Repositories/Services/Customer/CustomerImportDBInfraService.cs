using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using DO = Hagrid.Core.Domain.Entities;
using DTO = Hagrid.Core.Domain.DTO;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Infrastructure.Services.Customer
{
    public class CustomerImportDBInfraService : ICustomerImportDBInfraService
    {
        private readonly string strConnection = ConfigurationManager.ConnectionStrings[Config.ConnectionStringName].ConnectionString;

        public CustomerImportDBInfraService() { }

        public bool ImportCustomer(DO.Requisition requisition)
        {
            string DBAccountsCustomerImport = GetDbAccountsCustomerImport();

            var dbrequisition = requisition as DO.DBRequisition;

            using (var connection = new SqlConnection(strConnection))
            {
                using (var command = new SqlCommand(string.Format("{0}.[SPRK_CustomerECImport]", DBAccountsCustomerImport), connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@linkedservername", dbrequisition.LinkedServerName);
                    command.Parameters.AddWithValue("@databasename", dbrequisition.DataBaseName);
                    command.Parameters.AddWithValue("@Requisition", requisition.Code);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return true;
        }

        public List<DTO.Account> GetCustomers(DO.Requisition requisition, int skip, int take)
        {
            string DBAccountsCustomerImport = GetDbAccountsCustomerImport();

            var dicAddresses = GetAddresses(requisition);
            var accounts = new List<DTO.Account>();

            using (var connection = new SqlConnection(strConnection))
            {
                using (var command = new SqlCommand(string.Format("{0}.[SPRK_UserList]", DBAccountsCustomerImport), connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Requisition", requisition.Code);
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@Take", take);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customer = new DTO.Customer();
                            var document = string.Empty;

                            if (reader["ConTipo"].AsInt() == 1)
                            {
                                document = reader["ConCPF"].ToString().Trim();

                                string gender = null;
                                if (!reader["UsuSexo"].AsString().IsNullorEmpty())
                                    gender = reader["UsuSexo"].AsInt() == 1 ? "M" : "F";

                                customer = new DTO.Person
                                {
                                    FirstName = reader["ConNome"].ToString().Trim(),
                                    LastName = reader["ConSobreNome"].ToString().Trim(),
                                    Cpf = reader["ConCPF"].ToString().Trim(),
                                    Rg = reader["ConRG"].ToString().Trim(),
                                    BirthDate = reader["ConDtNascimento"].AsDateTime(),
                                    Gender = gender,
                                    Type = CustomerType.Person
                                };
                            }
                            else
                            {
                                document = reader["ConCNPJ"].ToString().Trim();

                                customer = new DTO.Company
                                {
                                    CompanyName = reader["ConRazaoSocial"].ToString().Trim(),
                                    TradeName = reader["ConNomeFantasia"].ToString().Trim(),
                                    Cnpj = reader["ConCNPJ"].ToString().Trim(),
                                    Ie = reader["ConIE"].ToString().Trim(),
                                    Im = reader["ConIM"].ToString().Trim(),
                                    Type = CustomerType.Company
                                };
                            }

                            customer.Status = reader["ConStatus"].AsInt() != 0 || reader["ConStatus"].AsInt() != 2 ? true : false;
                            customer.SaveDate = reader["ConDtInclusao"].AsDateTime();
                            customer.UpdateDate = reader["ConDtAtualizacao"].AsDateTime();

                            customer.Addresses = new List<DTO.Address>();

                            if (dicAddresses.ContainsKey(reader["ConCodigo"].AsInt()))
                                customer.Addresses.AddRange(dicAddresses[reader["ConCodigo"].AsInt()]);

                            var account = new DTO.Account()
                            {
                                Email = reader["UsuEmail"].ToString().Trim(),
                                Login = reader["UsuLogin"].ToString().Trim(),
                                Document = document,
                                Password = reader["UsuSenhaBkp"].ToString().Trim(),
                                Customer = customer,
                                Status = customer.Status,
                                Removed = false
                            };

                            accounts.Add(account);
                        }
                    }
                }
            }

            return accounts;
        }

        private Dictionary<int, List<DTO.Address>> GetAddresses(DO.Requisition requisition)
        {
            string DBAccountsCustomerImport = GetDbAccountsCustomerImport();

            var result = new Dictionary<int, List<DTO.Address>>();

            using (var connection = new SqlConnection(strConnection))
            {
                using (var command = new SqlCommand(string.Format("{0}.[SPRK_AddressList]", DBAccountsCustomerImport), connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Requisition", requisition.Code);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var address = new DTO.Address()
                            {
                                ContactName = reader["EndNome"].ToString().Trim(),
                                Type = GetAddressType(reader["EndTipo"].AsInt()),
                                Purpose = GetAddressPurposeType(reader["EndFinalidade"].AsInt()),
                                Street = string.Format("{0} {1}", (StreetTypeEC)reader["EndTipoLogradouro"].AsInt(), reader["EndLogradouro"].ToString().Trim()),
                                Number = reader["EndNumero"].ToString().Trim(),
                                District = reader["EndBairro"].ToString().Trim(),
                                City = reader["EndCidade"].ToString().Trim(),
                                State = reader["EndEstado"].ToString().Trim(),
                                ZipCode = reader["EndCEP"].ToString().Trim(),
                                Complement = reader["EndComplemento"].ToString().Trim(),
                                Country = reader["EndPais"].ToString().Trim(),
                                Status = true,
                                Removed = false
                            };

                            address.Phones = new List<DTO.Phone>();

                            if (!reader["EndDDD1"].ToString().Trim().ClearStrings().IsNullOrWhiteSpace())
                            {
                                address.Phones.Add(new DTO.Phone()
                                {
                                    CodeCountry = address.Country,
                                    DDD = reader["EndDDD1"].ToString().Trim(),
                                    Number = reader["EndTelefone1"].ToString().Trim().ClearStrings(),
                                    Extension = reader["EndRamal1"].ToString().Trim().ClearStrings(),
                                    PhoneType = PhoneType.Residencial
                                });
                            }

                            if (!reader["EndDDD2"].ToString().Trim().ClearStrings().IsNullOrWhiteSpace())
                            {
                                address.Phones.Add(new DTO.Phone()
                                {
                                    CodeCountry = address.Country,
                                    DDD = reader["EndDDD2"].ToString().Trim(),
                                    Number = reader["EndTelefone2"].ToString().Trim().ClearStrings(),
                                    Extension = reader["EndRamal2"].ToString().Trim().ClearStrings(),
                                    PhoneType = PhoneType.Comercial
                                });
                            }

                            if (!reader["EndDDD3"].ToString().Trim().ClearStrings().IsNullOrWhiteSpace())
                            {
                                address.Phones.Add(new DTO.Phone()
                                {
                                    CodeCountry = address.Country,
                                    DDD = reader["EndDDD3"].ToString().Trim(),
                                    Number = reader["EndTelefone3"].ToString().Trim().ClearStrings(),
                                    Extension = reader["EndRamal3"].ToString().Trim().ClearStrings(),
                                    PhoneType = PhoneType.Outros
                                });
                            }

                            if (!reader["EndDDDCelular"].ToString().Trim().ClearStrings().IsNullOrWhiteSpace())
                            {
                                address.Phones.Add(new DTO.Phone()
                                {
                                    CodeCountry = address.Country,
                                    DDD = reader["EndDDDCelular"].ToString().Trim(),
                                    Number = reader["EndCelular"].ToString().Trim().ClearStrings().PadLeft(9, '9'),
                                    PhoneType = PhoneType.Celular
                                });
                            }

                            if (!reader["EndDDDFax"].ToString().Trim().ClearStrings().IsNullOrWhiteSpace())
                            {
                                address.Phones.Add(new DTO.Phone()
                                {
                                    CodeCountry = address.Country,
                                    DDD = reader["EndDDDFax"].ToString().Trim(),
                                    Number = reader["EndFax"].ToString().Trim().ClearStrings(),
                                    PhoneType = PhoneType.Fax
                                });
                            }

                            if (result.ContainsKey(reader["ConCodigo"].AsInt()))
                                result[reader["ConCodigo"].AsInt()].Add(address);
                            else
                                result.Add(reader["ConCodigo"].AsInt(), new List<DTO.Address>() { address });
                        }
                    }
                }
            }

            return result;
        }

        private AddressType GetAddressType(int addressType)
        {
            switch (addressType)
            {
                case 1:
                    return AddressType.HomeAddress;
                case 2:
                    return AddressType.Comercial;
                case 3:
                    return AddressType.Other;
                default:
                    return AddressType.None;
            }
        }

        private AddressPurposeType GetAddressPurposeType(int addressPurposeType)
        {
            switch (addressPurposeType)
            {
                case 1:
                case 3:
                    return AddressPurposeType.Contact;
                case 2:
                case 5:
                case 6:
                case 4:
                    return AddressPurposeType.Shipping;
                default:
                    return AddressPurposeType.None;
            }
        }

        public void Clear(DO.Requisition requisition)
        {
            string DBAccountsCustomerImport = GetDbAccountsCustomerImport();

            using (var connection = new SqlConnection(strConnection))
            {
                using (var command = new SqlCommand(string.Format("{0}.[SPRK_Clear]", DBAccountsCustomerImport), connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Requisition", requisition.Code);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        private static string GetDbAccountsCustomerImport()
        {
            string DBAccountsCustomerImport;
            if (Config.Environment == EnvironmentType.Sandbox || Config.Environment == EnvironmentType.Staging)
                DBAccountsCustomerImport = string.Format("[DBRKAccountCustomerImport{0}].[dbo]", Config.Environment);
            else
                DBAccountsCustomerImport = "[DBRKAccountCustomerImport].[dbo]";
            return DBAccountsCustomerImport;
        }
    }
}
