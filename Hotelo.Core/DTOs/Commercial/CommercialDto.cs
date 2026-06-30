namespace Hotelo.Core.DTOs.Commercial;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    public decimal DiscountRate { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public int ContractsCount { get; set; }
}

public class CreateCompanyDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "Entreprise";
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    public decimal DiscountRate { get; set; }
    public string? Notes { get; set; }
}

public class ContractDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal RoomRate { get; set; }
    public decimal DiscountRate { get; set; }
    public int RoomsQuota { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired => DateTime.Today > EndDate;
    public string? Notes { get; set; }
}

public class CreateContractDto
{
    public int CompanyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal RoomRate { get; set; }
    public decimal DiscountRate { get; set; }
    public int RoomsQuota { get; set; }
    public string? Notes { get; set; }
}

public class CommercialDashboardDto
{
    public int TotalCompanies { get; set; }
    public int ActiveContracts { get; set; }
    public int ExpiredContracts { get; set; }
    public decimal AverageDiscount { get; set; }
    public List<CompanyDto> Companies { get; set; } = new();
    public List<ContractDto> Contracts { get; set; } = new();
}
