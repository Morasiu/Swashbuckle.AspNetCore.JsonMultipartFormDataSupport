using Demo.Models.Products;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;

namespace Demo {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			// ===== System.Text.Json =====
			// services.AddControllers()
			// 	.AddJsonOptions(options => {
			// 		options.JsonSerializerOptions.WriteIndented = true;
			// 		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			// 	});

			// ===== JSON.Net- =====
			services.AddControllers()
			        .AddNewtonsoftJson(options => {
				        options.SerializerSettings.Converters.Add(new StringEnumConverter());
				        options.SerializerSettings.Formatting = Formatting.Indented;
			        })
			        .AddFluentValidation(f => {
				        f.RegisterValidatorsFromAssemblyContaining<ProductValidator>();
				        // Important! Without this it won't work automatically
				        //  vvv
				        f.ImplicitlyValidateChildProperties = true;
				        
				        f.LocalizationEnabled = false;
			        });

			services.AddJsonMultipartFormDataSupport(JsonSerializerChoice.Newtonsoft);
			services.AddSwaggerExamplesFromAssemblyOf<ProductExamples>();
			services.AddSwaggerGen(o => {
				o.SwaggerDoc("v1", new OpenApiInfo {
					Title = "Demo",
					Version = "v1"
				});
			});
			services.AddFluentValidationRulesToSwagger();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(o => {
				o.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
				o.RoutePrefix = string.Empty;
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});

		}
	}
}
