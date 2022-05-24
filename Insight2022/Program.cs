using Carter;
using Insight2022.Contexts;
using Insight2022.Extentions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// extensions
builder.AddDatabases();
builder.AddSwagger();

builder.Services.AddCors();

//modules

builder.Services.AddCarter();
await using var app = builder.Build();
var environment = app.Environment;

app.UseAppCors();
app.UseExceptionHandling(environment);
app.UseSwaggerEndpoints(routePrefix: String.Empty);


app.MapCarter();

app.Run();
