namespace Hotelo.Core.DTOs.Guest;

public class GuestDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Passport { get; set; }
    public string? Nationality { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int VIPLevel { get; set; }
    public string VIPLabel { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public int TotalStays { get; set; }
}

public class CreateGuestDto
{
    public string FullName { get; set; } = string.Empty;
    public string? Passport { get; set; }
    public string? Nationality { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int VIPLevel { get; set; } = 0;
    public string? Address { get; set; }
    public string? Notes { get; set; }
}

public class UpdateGuestDto : CreateGuestDto { }
