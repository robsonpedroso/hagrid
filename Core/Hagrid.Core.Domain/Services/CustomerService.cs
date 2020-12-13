using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Policies;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Policies;
using Hagrid.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Core.Domain.Enums;
using System.Configuration;

namespace Hagrid.Core.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository customerRepository;
        public IPasswordPolicy passwordPolicy { get; set; }

        public CustomerService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public void PrepareToAdd(Customer customer, Guid originStore)
        {
            if (customer.Addresses.IsNull() || customer.Addresses.Count.IsZero())
                throw new ArgumentException("Endereço inválido. Informe ao menos 1");

            customer.IsValid();
            customer.OriginStore = originStore;
            customer.SaveDate = DateTime.Now;
            customer.UpdateDate = DateTime.Now;
            customer.Status = true;

            AddressCustomer contactAddress = customer.Addresses.Where(a => a.Purpose == AddressPurposeType.Contact).FirstOrDefault();
            if (!contactAddress.IsNull() && !contactAddress.Street.IsNullOrWhiteSpace())
            {
                contactAddress.AddressCustomerCode = Guid.NewGuid();

                bool containFaxPhone = false;
                contactAddress.Phones.ForEach(p =>
                {
                    if (p.PhoneType == PhoneType.Fax)
                        containFaxPhone = true;
                });

                if (!containFaxPhone)
                {
                    contactAddress.Phones.Add(new Phone()
                    {
                        CodeCountry = string.Empty,
                        DDD = string.Empty,
                        Number = string.Empty,
                        PhoneType = PhoneType.Fax
                    });
                }
            }

            customer.Addresses = new List<AddressCustomer>();
            ClearCaractersSpecialAddress(contactAddress);
            customer.Addresses.Add(contactAddress);
            customer.Addresses.ForEach(address => address.RemoveSpecialCharacter());
            customer.AddressData = customer.Addresses.Serialize().RemoveHeaderXML();

            if (customer is Person)
                ((Person)customer).BirthDate = ((Person)customer).BirthDate.Value.Date;

            customer.Account = null;
        }

        public void ClearCaractersSpecialAddress(AddressCustomer addressCustomer)
        {
            addressCustomer.City = !addressCustomer.City.IsNullOrWhiteSpace() ? addressCustomer.City.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : "";
            addressCustomer.Name = !addressCustomer.Name.IsNullOrWhiteSpace() ? addressCustomer.Name.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : "";
            addressCustomer.ContactName = !addressCustomer.ContactName.IsNullOrWhiteSpace() ? addressCustomer.ContactName.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : "";
            addressCustomer.Number = !addressCustomer.Number.IsNullOrWhiteSpace() ? addressCustomer.Number.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : "";
            addressCustomer.District = !addressCustomer.District.IsNullOrWhiteSpace() ? addressCustomer.District.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : "";
            addressCustomer.Street = !addressCustomer.Street.IsNullOrWhiteSpace() ? addressCustomer.Street.Replace("&#x0", "").Replace("\0", "").Replace("\\0", "") : "";
            addressCustomer.Country = !addressCustomer.Country.IsNullOrWhiteSpace() ? addressCustomer.Country.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : "";
            addressCustomer.ZipCode = !addressCustomer.ZipCode.IsNullOrWhiteSpace() ? addressCustomer.ZipCode.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : "";
            addressCustomer.Complement = !addressCustomer.Complement.IsNullOrWhiteSpace() ? addressCustomer.Complement.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : "";
        }

        public void PrepareToAddSimplified(Customer customer)
        {
            customer.OriginStore = Guid.Empty;
            customer.Status = true;

            customer.Addresses = new List<AddressCustomer>();
            customer.AddressData = customer.Addresses.Serialize().RemoveHeaderXML();

            if (customer.Type == CustomerType.Person)
                ((Person)customer).Gender = string.Empty;

            customer.Account = null;
        }

        public Customer PrepareToUpdate(Store currentStore, Customer customer, Customer newCustomer)
        {
            if (newCustomer.Addresses.IsNull() || newCustomer.Addresses.Count.IsZero())
                throw new ArgumentException("Endereço inválido. Informe ao menos 1");

            newCustomer.IsValid(currentStore);

            if (newCustomer is Person)
            {
                var newPerson = (Person)newCustomer;
                var person = (Person)customer;

                person.Transfer(newPerson);
            }
            else
            {
                if (newCustomer is Company && newCustomer.GetType() != customer.GetType())//register info incomplete
                {

                    customer = new Company((Person)customer);

                    ((Company)customer).Transfer((Company)newCustomer);
                }
                else
                {
                    ((Company)customer).Transfer((Company)newCustomer);
                }
            }

            customer.UpdateDate = DateTime.Now;
            customer.Status = true;
            customer.Addresses = customer.AddressData.Deserialize<List<AddressCustomer>>();

            #region "  Address  "

            var contactAddressRegistered = customer.Addresses.Where(a => a.Purpose == AddressPurposeType.Contact).FirstOrDefault();

            List<AddressCustomer> lstAddress = new List<AddressCustomer>();

            AddressCustomer newContactAddress = newCustomer.Addresses.Where(a => a.Purpose == AddressPurposeType.Contact).FirstOrDefault();

            if (!newContactAddress.IsNull() && !newContactAddress.Street.IsNullOrWhiteSpace())
            {
                if (!contactAddressRegistered.IsNull() && !contactAddressRegistered.AddressCustomerCode.IsEmpty())
                {
                    newContactAddress.AddressCustomerCode = contactAddressRegistered.AddressCustomerCode;
                    newContactAddress.UpdateDate = DateTime.Now;
                }
                else
                {
                    newContactAddress.AddressCustomerCode = Guid.NewGuid();
                    newContactAddress.SaveDate = DateTime.Now;
                    newContactAddress.UpdateDate = DateTime.Now;
                }

                #region "  Phones  "

                bool containFaxPhone = false;

                newContactAddress.Phones.ForEach(p =>
                {
                    if (p.PhoneType == PhoneType.Fax)
                        containFaxPhone = true;
                });

                if (!containFaxPhone)
                {
                    AddressCustomer contactAddressResgistered = newCustomer.Addresses.Where(a => a.Purpose == AddressPurposeType.Contact).FirstOrDefault();

                    if (!contactAddressResgistered.IsNull())
                    {
                        Phone fax = contactAddressResgistered.Phones.Where(p => p.PhoneType == PhoneType.Fax).FirstOrDefault();
                        if (!fax.IsNull())
                        {
                            newContactAddress.Phones.Add(new Phone()
                            {
                                CodeCountry = fax.CodeCountry,
                                DDD = fax.DDD,
                                Number = fax.Number,
                                PhoneType = PhoneType.Fax
                            });
                        }
                    }
                }

                #endregion

                lstAddress.Add(newContactAddress);
            }

            List<AddressCustomer> lstShippingAddress;

            if (currentStore.IsMain)
            {
                lstShippingAddress = newCustomer.Addresses.Where(a => a.Purpose != AddressPurposeType.Contact).ToList();

                newCustomer.Addresses.ForEach(a =>
                {
                    if (!a.AddressCustomerCode.IsEmpty())
                    {
                        a.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        a.AddressCustomerCode = Guid.NewGuid();
                        a.SaveDate = DateTime.Now;
                        a.UpdateDate = DateTime.Now;
                    }
                });
            }
            else
            {
                lstShippingAddress = customer.Addresses.Where(a => a.Purpose != AddressPurposeType.Contact).ToList();
            }

            if (!lstShippingAddress.IsNull())
                lstAddress.AddRange(lstShippingAddress);

            lstAddress.ForEach(address => address.RemoveSpecialCharacter());

            customer.Addresses = lstAddress;
            customer.AddressData = lstAddress.Serialize().RemoveHeaderXML();

            #endregion

            if (customer is Person)
                ((Person)customer).BirthDate = ((Person)customer).BirthDate.Value.Date;

            return customer;
        }

        public Customer GetMember(Guid code)
        {
            var customer = customerRepository.Get(code);
            customer.HandlerToGet();

            return customer;
        }

        public Customer GetMember(string document)
        {
            Customer customer = null;

            if (document.IsValidCPF())
            {
                customer = customerRepository.GetByCPF(document);
            }
            else if (document.IsValidCNPJ())
            {
                customer = customerRepository.GetByCNPJ(document);
            }

            customer.HandlerToGet();

            return customer;
        }

        public void UpdateMember(Customer member)
        {
            customerRepository.Update(member);
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() { customerRepository };
        }

        #endregion
    }
}
