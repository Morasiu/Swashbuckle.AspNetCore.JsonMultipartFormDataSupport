using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport {
	/// <summary>
	/// Aggregates form fields in Swagger to one JSON field and add example.
	/// </summary>
	public class MultiPartJsonOperationFilter : IOperationFilter {
		private readonly IServiceProvider _serviceProvider;
		private readonly IOptions<JsonSerializerOptions> _jsonOptions;

		/// <inheritdoc />
		public MultiPartJsonOperationFilter(IServiceProvider serviceProvider, IOptions<JsonSerializerOptions> jsonOptions) {
			_serviceProvider = serviceProvider;
			_jsonOptions = jsonOptions;
		}

		/// <inheritdoc />
		public void Apply(OpenApiOperation operation, OperationFilterContext context) {
			var descriptors = context.ApiDescription.ActionDescriptor.Parameters.ToList();
			foreach (var descriptor in descriptors) {
				// Get property with [FromJson]
				var propertyInfo = GetPropertyInfo(descriptor);

				if (propertyInfo != null) {
					var mediaType = operation.RequestBody.Content.First().Value;

					// Group all exploded properties.
					var groupedProperties = mediaType.Schema.Properties
						.GroupBy(pair => pair.Key.Split('.')[0]);

					var schemaProperties = new Dictionary<string, OpenApiSchema>();

					foreach (var property in groupedProperties) {
						if (property.Key == propertyInfo.Name) {
							AddEncoding(mediaType, propertyInfo);

							var openApiSchema = GenerateSchema(context, propertyInfo);

							AddDescription(openApiSchema, propertyInfo);
							AddExample(propertyInfo, openApiSchema);

							schemaProperties.Add(property.Key, openApiSchema);
						}
						else {
							schemaProperties.Add(property.Key, property.First().Value);
						}
					}
					// Override schema properties
					mediaType.Schema.Properties = schemaProperties;
				}
			}
		}

		private static OpenApiSchema GenerateSchema(OperationFilterContext context, PropertyInfo propertyInfo) {
			context.SchemaGenerator.GenerateSchema(propertyInfo.PropertyType, context.SchemaRepository);
			var openApiSchema = context.SchemaRepository.Schemas[propertyInfo.PropertyType.Name];
			return openApiSchema;
		}

		private static string AddDescription(OpenApiSchema openApiSchema, PropertyInfo propertyInfo) {
			return openApiSchema.Description += $"\n See {propertyInfo.PropertyType.Name} model.";
		}

		private static void AddEncoding(OpenApiMediaType mediaType, PropertyInfo propertyInfo) {
			mediaType.Encoding = mediaType.Encoding
				.Where(pair => !pair.Key.ToLower().Contains(propertyInfo.Name.ToLower()))
				.ToDictionary(pair => pair.Key, pair => pair.Value);
			mediaType.Encoding.Add(propertyInfo.Name, new OpenApiEncoding() {
				ContentType = "application/json",
				Explode = false
			});
		}

		private void AddExample(PropertyInfo propertyInfo, OpenApiSchema openApiSchema) {
			var example = GetExampleFor(propertyInfo.PropertyType);
			// Example do not exist. Use default.
			if (example == null) return;
			var json = JsonSerializer.Serialize(example, _jsonOptions.Value);
			openApiSchema.Example = new OpenApiString(json);
		}

		private object GetExampleFor(Type parameterType) {
			var makeGenericType = typeof(IExamplesProvider<>).MakeGenericType(parameterType);
			var method = makeGenericType.GetMethod("GetExamples");
			var exampleProvider = _serviceProvider.GetService(makeGenericType);
			// Example do not exist. Use default.
			if (exampleProvider == null) 
				return null;
			var example = method?.Invoke(exampleProvider, null);
			return example;
		}

		private static PropertyInfo GetPropertyInfo(ParameterDescriptor descriptor) =>
			descriptor.ParameterType.GetProperties()
				.SingleOrDefault(f => f.GetCustomAttribute<FromJsonAttribute>() != null);
	}
}