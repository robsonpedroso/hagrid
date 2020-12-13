using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using VO = Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Domain.Enums;

namespace Hagrid.Core.Infrastructure.Services.Customer
{
    public class CustomerImportFileInfraService : ICustomerImportFileInfraService
    {
        public CustomerImportFileInfraService() { }

        public Tuple<bool, List<string>, CustomerImport> ValidCustomer(string[] properties, FileRequisition fileRequisition)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            bool valid = true;
            List<string> messages = new List<string>();
            CustomerImport customer = null;

            if (properties.Length < 19)
            {
                valid = false;
                messages.Add("Linha com quantidade de colunas inválido");
                return new Tuple<bool, List<string>, CustomerImport>(valid, messages, customer);
            }

            if (properties[ConstantsFileImport.TypeCustomer].Trim().ToLower() == "pf")
            {
                if (properties[ConstantsFileImport.FirstName].Trim().IsNullorEmpty()) //FirstName
                {
                    valid = false;
                    messages.Add("Nome inválido");
                }

                if (properties[ConstantsFileImport.LastName].Trim().IsNullorEmpty()) //LastName
                {
                    valid = false;
                    messages.Add("Sobrenome inválido");
                }

                if (properties[ConstantsFileImport.Document].Trim().IsNullorEmpty() || !properties[ConstantsFileImport.Document].Trim().IsValidCPF()) //CPF
                {
                    valid = false;
                    messages.Add("CPF inválido");
                }

                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(properties[ConstantsFileImport.BirthDate].Trim(), out dt);
                if (properties[ConstantsFileImport.BirthDate].Trim().IsNullorEmpty() || dt == DateTime.MinValue || dt >= DateTime.Now) //BirthDate
                {
                    valid = false;
                    messages.Add("Data de nascimento inválida");
                }

                if (!properties[ConstantsFileImport.Sexo].Trim().IsNullorEmpty() && (properties[ConstantsFileImport.Sexo].Trim().ToLower() != "m" && properties[ConstantsFileImport.Sexo].Trim().ToLower() != "f")) //Sexo
                {
                    valid = false;
                    messages.Add("Gênero inválido");
                }
            }
            else if (properties[ConstantsFileImport.TypeCustomer].Trim().ToLower() == "pj")
            {
                if (properties[ConstantsFileImport.CompanyName].Trim().IsNullorEmpty()) //CompanyName
                {
                    valid = false;
                    messages.Add("Razão Social inválido");
                }

                if (properties[ConstantsFileImport.TradeName].Trim().IsNullorEmpty()) //TradeName
                {
                    valid = false;
                    messages.Add("Nome Fantasia inválido");
                }

                if (properties[ConstantsFileImport.Document].Trim().IsNullorEmpty() || !properties[ConstantsFileImport.Document].Trim().IsValidCNPJ()) //CNPJ
                {
                    valid = false;
                    messages.Add("CNPJ inválido");
                }

                if (properties[ConstantsFileImport.IM].Trim().IsNullorEmpty()) //IM
                {
                    valid = false;
                    messages.Add("IM inválido");
                }

                if (properties[ConstantsFileImport.IE].Trim().IsNullorEmpty()) //IE
                {
                    valid = false;
                    messages.Add("IE inválido");
                }
            }
            else
            {
                valid = false;
                messages.Add("Tipo de usuário inválido");
            }

            if (properties[ConstantsFileImport.Email].Trim().IsNullorEmpty() || !properties[ConstantsFileImport.Email].Trim().IsValidEmail())
            {
                valid = false;
                messages.Add("E-mail inválido");
            }

            Tuple<bool, List<string>> resultAddress = ValidAddress(properties);

            if (!resultAddress.Item1)
            {
                valid = false;
                messages.AddRange(resultAddress.Item2);
            }

            if (valid)
                customer = MakeCustomer(properties, fileRequisition);

            return new Tuple<bool, List<string>, CustomerImport>(valid, messages, customer);
        }

        private Tuple<bool, List<string>> ValidAddress(string[] properties)
        {
            bool valid = true;
            List<string> messages = new List<string>();

            // Six address positions, but only one mandatory and the remaining optional
            int[] indexAddress = new int[] { 0, 1, 2, 3, 4, 5 };
            List<VO.AddressCustomer> result = new List<VO.AddressCustomer>();

            // Fields quantity for o endereço
            var positionCount = 12;

            indexAddress.ForEach(i =>
            {
                // Get the next position of index 
                var index = positionCount * i;

                // If last field exists in properties, address exists, get address
                if (properties.Length >= 19 + (index))
                {
                    if (index == 0 || !properties[ConstantsFileImport.ZipCode + index].Trim().IsNullorEmpty())
                    {
                        if (properties[ConstantsFileImport.ZipCode + index].Trim().IsNullorEmpty() || 
                            properties[ConstantsFileImport.ZipCode + index].Trim().Length < 8 || 
                            !properties[ConstantsFileImport.ZipCode + index].Trim().Replace("-", string.Empty).IsNumeric()) //ZipCode
                        {
                            valid = false;
                            messages.Add("CEP inválido");
                        }

                        if (properties[ConstantsFileImport.Street + index].Trim().IsNullorEmpty()) //Street
                        {
                            valid = false;
                            messages.Add("Logradouro inválido");
                        }

                        if (properties[ConstantsFileImport.Number + index].Trim().IsNullorEmpty()) //Number
                        {
                            valid = false;
                            messages.Add("Número inválido");
                        }

                        if (!properties[ConstantsFileImport.TypeAddress + index].Trim().IsNullorEmpty() && 
                            properties[ConstantsFileImport.TypeAddress + index].Trim() != "1" && 
                            properties[ConstantsFileImport.TypeAddress + index].Trim() != "2" && 
                            properties[ConstantsFileImport.TypeAddress + index].Trim() != "3") //Type MakeAddressType
                        {
                            valid = false;
                            messages.Add("Tipoo de endereço inválido");
                        }

                        if (properties[ConstantsFileImport.State + index].Trim().IsNullorEmpty() || 
                            properties[ConstantsFileImport.State + index].Trim().Length != 2) //State
                        {
                            valid = false;
                            messages.Add("Estado inválido");
                        }

                        if (properties[ConstantsFileImport.District + index].Trim().IsNullorEmpty()) //District
                        {
                            valid = false;
                            messages.Add("Bairro inválido");
                        }

                        if (properties[ConstantsFileImport.City + index].Trim().IsNullorEmpty()) //City
                        {
                            valid = false;
                            messages.Add("Cidade inválido");
                        }

                        if (properties[ConstantsFileImport.PhoneDDD + index].Trim().IsNullorEmpty() ||
                            properties[ConstantsFileImport.PhoneDDD + index].Trim().AsInt(0) == 0) //DDD
                        {
                            valid = false;
                            messages.Add("DDD do telefone inválido");
                        }

                        if (properties[ConstantsFileImport.PhoneNumber + index].Trim().IsNullorEmpty() || 
                            !properties[ConstantsFileImport.PhoneNumber + index].Trim().Replace("-", string.Empty).IsNumeric()) //Number
                        {
                            valid = false;
                            messages.Add("Número do telefone inválido");
                        }

                        // Phone optional
                        if (!properties[ConstantsFileImport.CelDDD + index].Trim().IsNullorEmpty() && 
                            properties[ConstantsFileImport.CelDDD + index].Trim().AsInt(0) == 0) //Celular DDD
                        {
                            valid = false;
                            messages.Add("DDD do celular inválido");
                        }
                        // Phone optional
                        if (!properties[ConstantsFileImport.CelNumber + index].Trim().IsNullorEmpty() && 
                            !properties[ConstantsFileImport.CelNumber + index].Trim().Replace("-", string.Empty).IsNumeric()) //Celular Number
                        {
                            valid = false;
                            messages.Add("Número do celular inválido");
                        }
                    }
                }
            });

            return new Tuple<bool, List<string>>(valid, messages);
        }

        private CustomerImport MakeCustomer(string[] properties, FileRequisition fileRequisition)
        {
            CustomerImport customer;

            if (properties[ConstantsFileImport.TypeCustomer].Trim().ToLower() == "pf")
            {
                customer = new PersonImport
                {
                    FirstName = properties[ConstantsFileImport.FirstName],
                    LastName = properties[ConstantsFileImport.LastName],
                    CPF = properties[ConstantsFileImport.Document],
                    RG = string.Empty,
                    BirthDate = DateTime.Parse(properties[ConstantsFileImport.BirthDate]),
                    Gender = properties[ConstantsFileImport.Sexo].IsNullorEmpty() ? null : properties[ConstantsFileImport.Sexo].AsString().ToUpper(),
                    Type = CustomerType.Person
                };
            }
            else
            {
                customer = new CompanyImport
                {
                    CompanyName = properties[ConstantsFileImport.CompanyName],
                    TradeName = properties[ConstantsFileImport.TradeName],
                    CNPJ = properties[ConstantsFileImport.Document],
                    IM = properties[ConstantsFileImport.IM],
                    IE = properties[ConstantsFileImport.IE],
                    Type = CustomerType.Company
                };
            }

            customer.Email = properties[ConstantsFileImport.Email];
            customer.NewsLetter = false;
            customer.Status = true;
            customer.Removed = false;
            customer.SaveDate = DateTime.Now;
            customer.UpdateDate = DateTime.Now;
            customer.StoreCode = fileRequisition.Store.Code;
            customer.Address = MakeAddress(properties);

            return customer;
        }

        private List<VO.AddressCustomer> MakeAddress(string[] properties)
        {
            // Six address positions, but only one mandatory and the remaining optional
            int[] indexAddress = new int[] { 0, 1, 2, 3, 4, 5 };
            List<VO.AddressCustomer> result = new List<VO.AddressCustomer>();

            // Fields quantity for o endereço
            var positionCount = 12;

            indexAddress.ForEach(i =>
            {
                // Get the next position of index 
                var index = positionCount * i;

                // If last field exists in properties, address exists, get address
                if (properties.Length >= 18 + (index))
                {
                    if (index == 0 || !properties[ConstantsFileImport.ZipCode + index].Trim().IsNullorEmpty())
                    {
                        var address = new VO.AddressCustomer
                        {
                            AddressCustomerCode = Guid.NewGuid(),
                            ZipCode = properties[ConstantsFileImport.ZipCode + index].Trim().Replace("-", string.Empty).PadLeft(8, '0'),
                            Street = properties[ConstantsFileImport.Street + index].Trim(),
                            Number = properties[ConstantsFileImport.Number + index].Trim(),
                            Complement = properties[ConstantsFileImport.Complement + index].Trim(),
                            Type = MakeAddressType(properties[ConstantsFileImport.TypeAddress + index].Trim()),
                            State = properties[ConstantsFileImport.State + index].Trim().ToUpper(),
                            District = properties[ConstantsFileImport.District + index].Trim(),
                            City = properties[ConstantsFileImport.City + index].Trim(),
                            Name = properties[ConstantsFileImport.FirstName].Trim(),
                            ContactName = properties[ConstantsFileImport.FirstName].Trim(),
                            Purpose = i == 0 ? AddressPurposeType.Contact : AddressPurposeType.Shipping,
                            Status = true,
                            Removed = false
                        };

                        address.Phones = new List<Hagrid.Core.Domain.ValueObjects.Phone>();

                        address.Phones.Add(new VO.Phone()
                        {
                            CodeCountry = "55",
                            DDD = properties[ConstantsFileImport.PhoneDDD + index].Trim(),
                            Number = properties[ConstantsFileImport.PhoneNumber + index].Trim(),
                            Extension = string.Empty,
                            PhoneType = PhoneType.Residencial
                        });

                        if (!properties[ConstantsFileImport.CelDDD + index].Trim().IsNullOrWhiteSpace() &&
                            !properties[ConstantsFileImport.CelDDD + index].Trim().IsNullOrWhiteSpace())
                        {
                            address.Phones.Add(new Hagrid.Core.Domain.ValueObjects.Phone()
                            {
                                CodeCountry = "55",
                                DDD = properties[ConstantsFileImport.CelDDD + index].Trim(),
                                Number = properties[ConstantsFileImport.CelNumber + index].Trim(),
                                Extension = string.Empty,
                                PhoneType = PhoneType.Celular
                            });
                        }

                        result.Add(address);
                    }
                }
            });

            return result;
        }

        private AddressType MakeAddressType(string value)
        {
            switch (value.AsInt())
            {
                case 1:
                    return AddressType.HomeAddress;
                case 2:
                    return AddressType.Comercial;
                case 3:
                    return AddressType.Other;
                default:
                    return AddressType.HomeAddress;
            }
        }
    }
}
