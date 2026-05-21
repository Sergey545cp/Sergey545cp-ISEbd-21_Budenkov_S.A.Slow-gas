using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SlowGas.Contracts.StoragesContracts;
using SlowGas.Database;
using SlowGas.Database.Implementations;
using SlowGas.Models.Infrastructure;
using SlowGas.Models.StoragesContracts;
using SlowGas.WebApi.Adapters;
using SlowGas.WebApi.Infrastructure;
using SlowGas.BusinessLogic.Implementations;
using SlowGas.Database.Implementations;
using SlowGas.Models.BusinessLogicsContracts;
using SlowGas.Models.StoragesContracts;
using SlowGas.Models.Infrastructure;
using SlowGas.WebApi.Infrastructure;
using SlowGas.BusinessLogic.Implementations;
using SlowGas.Models.BusinessLogicsContracts;
using SlowGas.Models.Infrastructure;
using SlowGas.WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IConfigurationDatabase, SlowGas.WebApi.Infrastructure.ConfigurationDatabase>();
builder.Services.AddScoped<SlowGasDbContext>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<ICustomerStorageContract, CustomerStorage>();
builder.Services.AddScoped<IMotorStorageContract, MotorStorage>();
builder.Services.AddScoped<IRequestStorageContract, RequestStorage>();
builder.Services.AddScoped<IShipmentStorageContract, ShipmentStorage>();
builder.Services.AddScoped<IInvoiceStorageContract, InvoiceStorage>();

builder.Services.AddScoped<ICustomerAdapter, CustomerAdapter>();
builder.Services.AddScoped<IMotorAdapter, MotorAdapter>();
builder.Services.AddScoped<IRequestAdapter, RequestAdapter>();
builder.Services.AddScoped<IShipmentAdapter, ShipmentAdapter>();
builder.Services.AddScoped<IInvoiceAdapter, InvoiceAdapter>();

builder.Services.AddScoped<IPostStorageContract, PostStorage>();
builder.Services.AddScoped<ISalaryStorageContract, SalaryStorage>();
builder.Services.AddScoped<ISaleStorageContract, SaleStorage>();
builder.Services.AddScoped<ISalaryBusinessLogic, SalaryBusinessLogic>();
builder.Services.AddScoped<IWorkerStorageContract, WorkerStorage>();
builder.Services.AddSingleton<IConfigurationSalary, ConfigurationSalary>();
builder.Services.AddScoped<ISalaryBusinessLogic, SalaryBusinessLogic>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/login/{username}", (string username) =>
{
    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
    {
        Issuer = AuthOptions.ISSUER,
        Audience = AuthOptions.AUDIENCE,
        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username)
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
    });
    return tokenHandler.WriteToken(token);
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SlowGasDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();

public partial class Program { }