using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendanceTracker.Infrastructure.Data;

public class AttendanceTrackerContext : IdentityDbContext<User, Role, string>
{
    public AttendanceTrackerContext(DbContextOptions<AttendanceTrackerContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigureModelBuilders(builder);
    }

    private void ConfigureModelBuilders(ModelBuilder builder)
    {
        builder.ConvertIdentityToPostgresSQLNamingConventions();
    }

    public DbSet<Card> Cards { get; set; }
    public DbSet<Check> Checks { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeManagment> EmployeeManagments { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Modules> Module { get; set; }
    public DbSet<ModulesAccess> ModuleAccess { get; set; }
    public DbSet<Remarks> Remark { get; set; }


}