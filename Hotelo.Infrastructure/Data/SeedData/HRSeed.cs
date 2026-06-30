using Hotelo.Core.Entities.HR;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Data.SeedData;

public static class HRSeed
{
    public static async Task SeedAsync(HoteloDbContext context)
    {
        if (await context.Departments.AnyAsync()) return;

        var departments = new List<Department>
        {
            new Department { Name="Reception",    Description="Accueil et gestion clients",     IsActive=true },
            new Department { Name="Housekeeping", Description="Nettoyage et entretien",          IsActive=true },
            new Department { Name="Restauration", Description="Restaurant et room service",      IsActive=true },
            new Department { Name="Maintenance",  Description="Technique et maintenance",         IsActive=true },
            new Department { Name="Direction",    Description="Direction generale et gestion",   IsActive=true },
            new Department { Name="Comptabilite", Description="Finances et comptabilite",         IsActive=true },
        };
        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        var employees = new List<Employee>
        {
            new Employee { FullName="Kamel Bensalem",  DepartmentId=departments[0].Id, HireDate=new DateTime(2020,3,1),  ContractType="CDI", BaseSalary=65000,  IsActive=true },
            new Employee { FullName="Nadia Cherif",    DepartmentId=departments[0].Id, HireDate=new DateTime(2021,6,15), ContractType="CDI", BaseSalary=55000,  IsActive=true },
            new Employee { FullName="Fatima Zaoui",    DepartmentId=departments[1].Id, HireDate=new DateTime(2019,1,10), ContractType="CDI", BaseSalary=45000,  IsActive=true },
            new Employee { FullName="Mohamed Laid",    DepartmentId=departments[1].Id, HireDate=new DateTime(2022,9,1),  ContractType="CDD", BaseSalary=40000,  IsActive=true },
            new Employee { FullName="Yacine Hamidi",   DepartmentId=departments[2].Id, HireDate=new DateTime(2020,7,20), ContractType="CDI", BaseSalary=50000,  IsActive=true },
            new Employee { FullName="Amina Khelil",    DepartmentId=departments[3].Id, HireDate=new DateTime(2018,4,5),  ContractType="CDI", BaseSalary=70000,  IsActive=true },
            new Employee { FullName="Omar Benchikh",   DepartmentId=departments[4].Id, HireDate=new DateTime(2015,1,1),  ContractType="CDI", BaseSalary=120000, IsActive=true },
            new Employee { FullName="Leila Mansouri",  DepartmentId=departments[5].Id, HireDate=new DateTime(2019,8,12), ContractType="CDI", BaseSalary=80000,  IsActive=true },
        };
        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();
    }
}
