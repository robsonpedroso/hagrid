using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Services
{
    public class ApplicationStoreService : IApplicationStoreService
    {
        private IApplicationStoreRepository applicationStoreRepository;
        private IApplicationRepository applicationRepository;

        public ApplicationStoreService(IApplicationStoreRepository applicationStoreRepository, IApplicationRepository applicationRepository)
        {
            this.applicationStoreRepository = applicationStoreRepository;
            this.applicationRepository = applicationRepository;
        }

        public bool Authenticate(string clientId, string clientSecret, string reqOrigin, out string message, out ApplicationStore applicationStore)
        {
            message = string.Empty;
            applicationStore = null;

            if (string.IsNullOrWhiteSpace(clientId))
            {
                message = "ClientId should be sent.";
            }
            else
            {
                Guid clientCode;
                if (Guid.TryParse(clientId, out clientCode))
                {

                    if (!clientSecret.IsNullOrWhiteSpace())
                        applicationStore = applicationStoreRepository.Get(ClientAuthType.Confidential, clientCode);
                    else
                        applicationStore = applicationStoreRepository.Get(ClientAuthType.JavaScript, clientCode);


                    if (applicationStore == null)
                    {
                        message = string.Format("Client '{0}' not found.", clientId);
                    }
                    else
                    {
                        if (!applicationStore.Status)
                        {
                            message = string.Format("Client '{0}' is inactive.", clientId);
                        }
                        else
                        {
                            return applicationStore.IsValidCall(clientSecret, reqOrigin);
                        }
                    }
                }
                else
                {
                    message = string.Format("Client '{0}' not found.", clientId);
                }
            }

            return false;
        }

        public bool Exists(string clientId)
        {
            Guid clientCode;
            if (Guid.TryParse(clientId, out clientCode))
            {
                var client = applicationStoreRepository.GetByClientId(clientCode);
                return !client.IsNull() && !client.Code.IsEmpty();
            }

            return false;
        }

        public ApplicationStore Save(Application application, Store store, string[] jsAllowedOrigins)
        {
            var applicationStore = new ApplicationStore();

            applicationStore.Code = Guid.NewGuid();
            applicationStore.Application = application;
            applicationStore.Store = store;
            applicationStore.Status = true;
            applicationStore.ConfClient = Guid.NewGuid();
            applicationStore.ConfSecret = Guid.NewGuid().ToString().Substring(24);
            applicationStore.JSClient = Guid.NewGuid();
            applicationStore.JSAllowedOrigins = !jsAllowedOrigins.IsNull() ? string.Join(",", jsAllowedOrigins) : string.Empty;

            return applicationStoreRepository.Save(applicationStore);
        }

        public List<ApplicationStore> Get(string[] applications, Guid storeCode)
        {
            List<ApplicationStore> applicationStores = new List<ApplicationStore>();
            var mpAccountsMain = GetByStoreTypeMain(new string[] { "MP-Accounts" });

            if (!applications.IsNull() && !applications.Length.IsZero())
            {
                var apps = applicationRepository.Get(applications, true);

                applications.ForEach(applicationName =>
                {
                    var app = apps.SingleOrDefault(x => x.Name == applicationName);

                    if (app.IsNull())
                        throw new ArgumentException("Ops! A aplicação {0} não existe. :(".ToFormat(applicationName));

                    var others = applicationStoreRepository.Get(app.Code.Value, storeCode);

                    if (others.IsNull())
                        throw new ArgumentException("Ops! A loja não tem permissão a aplicação informada {0}. :(".ToFormat(applicationName));

                    applicationStores.Add(others);
                });

                if (!applicationStores.Any(appSto => mpAccountsMain.Any(acc => acc.Code == appSto.Code)))
                {
                    applicationStores.AddRange(mpAccountsMain);
                }
            }
            else
            {
                applicationStores.AddRange(mpAccountsMain);
            }

            return applicationStores;
        }

        public List<ApplicationStore> GetByStoreTypeMain(string[] applications)
        {
            if (!applications.IsNull() && !applications.Length.IsZero())
            {
                List<ApplicationStore> applicationStores = new List<ApplicationStore>();

                var apps = applicationRepository.Get(applications, true);

                applications.ForEach(applicationName =>
                {
                    var app = apps.SingleOrDefault(x => x.Name == applicationName);

                    if (app.IsNull())
                        throw new ArgumentException("Ops! A aplicação {0} não existe. :(".ToFormat(applicationName));

                    var others = applicationStoreRepository.GetByStoreTypeMain(app.Code.Value);

                    if (others.IsNull())
                        throw new ArgumentException("Ops! A loja não tem permissão a aplicação informada {0}. :(".ToFormat(applicationName));

                    applicationStores.AddRange(others);
                });

                return applicationStores;
            }
            else
            {
                return null;
            }
        }

        public ApplicationStore Get(ApplicationStore applicationStore, Guid storeCode)
        {
            ApplicationStore _applicationStore;

            //get application on token
            if (applicationStore.Store.Code == storeCode)
                _applicationStore = applicationStore;
            else
                _applicationStore = applicationStoreRepository.Get(applicationStore.Application.Code.Value, storeCode);

            return _applicationStore;
        }

        public List<ApplicationStore> CreateAppStore(Store store)
        {
            var applicationStoreCollection = new List<ApplicationStore>();

            if (!store.ApplicationsStore.IsNull() && store.ApplicationsStore.Count() > 0)
            {
                List<ApplicationStore> storeApp = RemoveMainApplications(store.ApplicationsStore.ToList(), null);

                storeApp.ForEach(application =>
                {
                    var _application = applicationRepository.Get(application.Application.Name);

                    if (_application.IsNull())
                        throw new ArgumentException("A aplicação '{0}' não existe.".ToFormat(application.Application.Name));


                    applicationStoreCollection.Add(Save(_application, store, application.JSAllowedOrigins.Split(',')));
                });
            }
            else
            {
                List<Application> applications = RemoveMainApplications(null, applicationRepository.List());

                applications.ForEach(a => applicationStoreCollection.Add(Save(a, store, new string[0])));
            }

            return applicationStoreCollection;
        }

        public dynamic RemoveMainApplications(List<ApplicationStore> storeApplications = null, List<Application> applications = null)
        {
            List<string> removedStoreApplications = new List<string>()
            {
                "MP-Accounts-Admin"
            };

            if (!storeApplications.IsNull() && storeApplications.Count > 0)
            {
                foreach (var removedApplication in removedStoreApplications)
                {
                    var app = storeApplications.FirstOrDefault(sa => sa.Application.Name.Equals(removedApplication));

                    storeApplications.Remove(app);
                }

                return storeApplications.ToList();
            }
            else
            {
                foreach (var removedApplication in removedStoreApplications)
                {
                    var app = applications.FirstOrDefault(a => a.Name.Equals(removedApplication));

                    applications.Remove(app);
                }

                return applications.ToList();
            }
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() { applicationStoreRepository, applicationRepository };
        }

        #endregion
    }
}
