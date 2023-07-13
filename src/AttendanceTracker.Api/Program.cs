using AttendanceTracker.Api.Configuration;
using AttendanceTracker.Api.Middlewares;
using AttendanceTracker.Api.Utils;
using AttendanceTracker.Core.Entities.Account;
using AttendanceTracker.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using AttendanceTracker.Api.Hangfire;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
}).AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AttendanceTracker");

builder.Services.AddDbContext<AttendanceTrackerContext>(options =>
{
    options.UseNpgsql(connectionString);
});
//hangfire method

builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(connectionString);
});


builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AttendanceTrackerContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromDays(60);
});
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddAuthorization();

//builder.Services.AddAuthorizationServices(builder.Configuration);

//builder.Services.AddNewtonsoft(options =>
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Services.AddCors();

app.UseHttpsRedirection();

app.UseHangfireDashboard();
app.UseHangfireServer();

// Schedule the job Hangfire

// RecurringJob.AddOrUpdate<HangfireJob>(x => x.SendEmailToEmployeesWhoNotComeToCompanyAtTime(CancellationToken.None), "15 6 * * 1-5");
using (var scope = ((IApplicationBuilder) app).ApplicationServices.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<AttendanceTrackerContext>();

    dbContext?.Database.Migrate();
}

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

    // Set the authorization token in Swagger UI
    c.OAuthClientId("swagger");
    c.OAuthAppName("Attendance Tracker Api");
    c.OAuthUsePkce();

    // ...
});

app.UseCors(options =>
{
    options
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed(hostName => true);
});

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();