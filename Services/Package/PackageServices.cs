using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.Data.Entities;
using Span.Culturio.Api.Models.Package;

namespace Span.Culturio.Api.Services.Package
{
    public class PackageServices : IPackageServices
    {
        private readonly IMapper _iMapper;
        private readonly DataContext _dataContext;

        public PackageServices(IMapper mapper, DataContext dataContext)
        {
            _iMapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<PackageDto> CreatePackageAsync(CreatePackageDto createPackageDto)
        {
            var createPackageItemsDto = createPackageDto.CultureObjects;

            var package = _iMapper.Map<Data.Entities.Package>(createPackageDto);
            var packageItems = _iMapper.Map<List<PackageItem>>(createPackageItemsDto);

            await _dataContext.AddAsync(package);
            await _dataContext.SaveChangesAsync();

            packageItems.ForEach(x => x.PackageId = package.Id);

            await _dataContext.AddRangeAsync(packageItems);
            await _dataContext.SaveChangesAsync();

            var packageItemsDto = _iMapper.Map<IEnumerable<PackageItemDto>>(packageItems);

            var packageDto = _iMapper.Map<PackageDto>(package);
            packageDto.CultureObjects = packageItemsDto;

            return packageDto;
        }

        public async Task<List<PackageDto>> GetPackagesAsync()
        {
            var packages = await _dataContext.Packages.ToListAsync();
            var packageDtos = _iMapper.Map<List<PackageDto>>(packages);

            packageDtos.ForEach(x =>
            {
                var packageItems = _dataContext.PackageItems.Where(y => y.PackageId == x.Id).ToList();
                var packageItemDtos = _iMapper.Map<List<PackageItemDto>>(packageItems);
                x.CultureObjects = packageItemDtos;
            });

            return packageDtos;
        }
    }
}
