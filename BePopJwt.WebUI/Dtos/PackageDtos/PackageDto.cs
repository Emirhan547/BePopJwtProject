namespace BePopJwt.WebUI.Dtos.PackageDtos
{
    public class PackageDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public decimal Price { get; set; }
    }
}
