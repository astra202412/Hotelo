namespace Hotelo.Core.DTOs.Package;

public class PackageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal AdditionalPrice { get; set; }
    public bool IncludesBreakfast { get; set; }
    public bool IncludesParking { get; set; }
    public bool IncludesSpa { get; set; }
    public bool IncludesAirportTransfer { get; set; }
    public int? MinNights { get; set; }
    public bool IsActive { get; set; }
    public string IncludesLabel { get; set; } = string.Empty;
}

public class CreatePackageDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal AdditionalPrice { get; set; }
    public bool IncludesBreakfast { get; set; }
    public bool IncludesParking { get; set; }
    public bool IncludesSpa { get; set; }
    public bool IncludesAirportTransfer { get; set; }
    public int? MinNights { get; set; }
}

public class UpdatePackageDto : CreatePackageDto
{
    public bool IsActive { get; set; } = true;
}
