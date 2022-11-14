namespace Span.Culturio.Api.Models.Package
{
    public class CreatePackageDto
    {
        public string Name { get; set; }
        public IEnumerable<CreatePackageItemDto> CultureObjects { get; set; }
        public int ValidDays { get; set; }
    }
}