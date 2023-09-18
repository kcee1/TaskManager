using TaskManagerApi.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceLibrary.AzureBlobService.AzureBlob;
using ServiceLibrary.AzureBlobService.Blob.Dtos;
using ServiceLibrary.AzureBlobService.IAzureBlobInterface;
using ServiceLibrary.EmailService.Implementations;
using ServiceLibrary.EmailService.Interfaces;
using ServiceLibrary.EmailService.Model;
using System.Text;
using TaskManager.DAL.Data;
using TaskManager.DomainLayer.Models;
using TaskManger.BusinessLogic.UnitOfWorks;
using TaskManager.BusinessLogic.IValidationFactoryInterface;
using TaskManager.BusinessLogic.ValidationFactory;
using Hangfire;
using TaskManagerApi.BackgroundServices;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Add Hangfire services
builder.Services.AddHangfire(configuration => configuration
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


var emailCon = builder.Configuration.GetSection("EmailDetails").Get<EmailDetails>();
var AzureBlobDetails = builder.Configuration.GetSection("AzureBlobDetails").Get<BlobDetails>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<ITokenAuth, TokenAuth>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IAzureStorage, AzureStorage>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IValidationFactory, ValidationFactory>();
builder.Services.AddScoped<NotificationService>();

builder.Services.AddControllers();

builder.Services.AddSingleton(emailCon);
builder.Services.AddSingleton(AzureBlobDetails);
builder.Services.AddHangfireServer();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});



builder.Services.AddAuthorization();
// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TASK MANAGER V1", Version = "v1" });

    //Configure swagger for authorization
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT containing userid claim",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });
    var security =
        new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                UnresolvedReference = true
            },
            new List<string>()
        }
        };
    c.AddSecurityRequirement(security);

});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseAuthentication();

app.Map("/hangfire", hangfireApp =>
{
	hangfireApp.UseRouting();
	app.UseAuthorization();
	hangfireApp.UseEndpoints(endpoints =>
	{
		endpoints.MapHangfireDashboard();
	});
});



// Schedule the background job to run every 1 minute (for testing)
RecurringJob.AddOrUpdate<NotificationService>("notification-job", x => x.NotificationForDueDate(), Cron.Daily);
RecurringJob.AddOrUpdate<TaskCompleteService>("notification-job", x => x.TaskCompleteNotification(), "*/15 * * * *");
RecurringJob.AddOrUpdate<TaskAssignedService>("notification-job", x => x.TaskAssignedNotifications(), "*/15 * * * *");


// Enable Hangfire dashboard (optional)
app.UseHangfireDashboard();


app.MapControllers();


app.Run();
