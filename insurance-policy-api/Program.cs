using AutoMapper;
using insurance_policy_api.Interfaces;
using insurance_policy_api.Mapper;
using insurance_policy_api.Middlewares;
using insurance_policy_api.Services;
using insurance_policy_api_domain.Contracts;
using insurance_policy_api_domain.Services;
using insurance_policy_api_infrastructure.Contexts;
using insurance_policy_api_infrastructure.Interfaces;
using insurance_policy_api_infrastructure.Repositories;
using insurance_policy_api_infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PolicyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")
                        , options => { options.CommandTimeout(5); }));
            //.EnableSensitiveDataLogging()
            //.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())));

builder.Services.AddScoped<IPolicyAppService, PolicyAppService>();
builder.Services.AddScoped<IPolicyDomainService, PolicyDomainService>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "insurance-policy-api", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);
});

//Configuração do AutoMapper
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});
builder.Services.AddSingleton(sp => config.CreateMapper());

var app = builder.Build();

app.UseHealthChecks("/health");
app.UseHealthChecks("/actuator/health");
app.UseHealthChecks("/status",
   new HealthCheckOptions()
   {
       ResponseWriter = async (context, report) =>
       {
           var result = JsonSerializer.Serialize(
               new
               {
                   statusApplication = report.Status.ToString(),
                   healthChecks = report.Entries.Select(e => new
                   {
                       check = e.Key,
                       ErrorMessage = e.Value.Exception?.Message,
                       status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                   })
               });
           context.Response.ContentType = MediaTypeNames.Application.Json;
           await context.Response.WriteAsync(result);
       }
   });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();