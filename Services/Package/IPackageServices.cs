using Span.Culturio.Api.Models.Package;

namespace Span.Culturio.Api.Services.Package
{
    public interface IPackageServices
    {
        Task<PackageDto> CreatePackageAsync(CreatePackageDto createPackageDto);
        Task<List<PackageDto>> GetPackagesAsync();
    }
}
