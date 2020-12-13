using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Infra.Utils;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class ApplicationApplication : AccountBaseApplication, IApplicationApplication
    {
        IApplicationRepository appRepository;

        public ApplicationApplication(IComponentContext context, IApplicationRepository appRepository)
            : base(context)
        {
            this.appRepository = appRepository;
        }

        public DTO.Application GetByName(string name)
        {
            DTO.Application result = null;

            var _result = appRepository.Get(name);

            if (!_result.IsNull())
                result = new DTO.Application(_result, withInformations: true);

            return result;
        }

        public object GetApplications()
        {
            var applications = appRepository.List().OrderBy(a => a.Name).ToList();

            return applications.Select(app => new
            {
                code = app.Code,
                name = app.Name,
                member = app.MemberType
            });
        }
    }
}
