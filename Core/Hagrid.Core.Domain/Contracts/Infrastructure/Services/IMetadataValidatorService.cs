using Hagrid.Core.Domain.Entities;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Services
{
    public interface IMetadataValidatorService
    {
        MetadataValidator Get(BaseMetadata metadata);
    }
}