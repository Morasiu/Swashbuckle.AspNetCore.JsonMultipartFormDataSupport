using Demo;
using Demo.Models.Products;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ===== System.Text.Json =====
// services.AddControllers()
// 	.AddJsonOptions(options => {
// 		options.JsonSerializerOptions.WriteIndented = true;
// 		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
// 	});

// ===== JSON.Net- =====
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.Formatting = Formatting.Indented;
    });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();
ValidatorOptions.Global.LanguageManager.Enabled = false;
builder.Services.AddJsonMultipartFormDataSupport<NewtonsoftSerializationProvider>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ProductExamples>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new() { Title = "Demo", Version = "v1" });
    o.ExampleFilters();
});
builder.Services.AddFluentValidationRulesToSwagger();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();