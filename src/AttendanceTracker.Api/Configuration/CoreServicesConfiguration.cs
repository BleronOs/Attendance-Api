using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Services;
using AttendanceTracker.Infrastructure.Configuration.Email;
using AttendanceTracker.Infrastructure.Data;
using AttendanceTracker.Infrastructure.Logging;

namespace AttendanceTracker.Api.Configuration;

public static class CoreServicesConfiguration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IAsyncRepository<>), typeof(AttendanceTrackerRepository<>));
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        var emailConfiguration =
        configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton<IEmailSender>(emailSender => new EmailSender(emailConfiguration));
        services.AddScoped<IJobPositionService, JobPositionService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IManagerService, ManagerService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployeeManagmentService, EmployeeManagmentService>();
        services.AddScoped<ICheckService, CheckService>();
        services.AddScoped<IModulesService, ModulesService>();
        services.AddScoped<IModulesAccessService, ModulesAccessService>();
        services.AddScoped<IRemarksService, RemarksService>();
        services.AddScoped<IDashBoardService, DashBoardService>();
        return services;
    }
}