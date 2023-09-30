using System.Reflection;
using WiremockMicroservices.Extensions;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.AddWiremockEndpoints(configuration, Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseWiremockEndpoints();

app.Run();
