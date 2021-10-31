using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations {
	/// <summary>
	/// Aggregates form fields in Swagger to one JSON field and add example.
	/// </summary>
	public class MultiPartJsonOperationFilter : IOperationFilter {
		private readonly IServiceProvider _serviceProvider;
		private readonly IOptions<JsonOptions> _jsonOptions;
		private readonly IOptions<MvcNewtonsoftJsonOptions> _newtonsoftJsonOption;

		/// <summary>
		/// Creates <see cref="MultiPartJsonOperationFilter"/>
		/// </summary>
		public MultiPartJsonOperationFilter(IServiceProvider serviceProvider, IOptions<JsonOptions> jsonOptions, IOptions<MvcNewtonsoftJsonOptions> newtonsoftJsonOption) {
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_jsonOptions = jsonOptions;
			_newtonsoftJsonOption = newtonsoftJsonOption;
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

							var openApiSchema = GetSchema(context, propertyInfo);
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

		/// <summary>
		/// Generate schema for propertyInfo
		/// </summary>
		/// <returns></returns>
		private OpenApiSchema GetSchema(OperationFilterContext context, PropertyInfo propertyInfo)
		{
			bool present = context.SchemaRepository.TryLookupByType(propertyInfo.PropertyType, out OpenApiSchema openApiSchema);
			if (!present)
			{
				_ = context.SchemaGenerator.GenerateSchema(propertyInfo.PropertyType, context.SchemaRepository);
				_ = context.SchemaRepository.TryLookupByType(propertyInfo.PropertyType, out openApiSchema);
				var schema = context.SchemaRepository.Schemas[openApiSchema.Reference.Id];
				AddDescription(schema, openApiSchema.Title);
				AddExample(propertyInfo, schema);
			}
			return context.SchemaRepository.Schemas[openApiSchema.Reference.Id];
		}

		private static void AddDescription(OpenApiSchema openApiSchema, string SchemaDisplayName) {
			openApiSchema.Description += $"\n See {SchemaDisplayName} model.";
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
			string json;

			if (JsonMultipartFormDataOptions.JsonSerializerChoice == JsonSerializerChoice.SystemText)
				json = JsonSerializer.Serialize(example, _jsonOptions.Value.JsonSerializerOptions);
			else if (JsonMultipartFormDataOptions.JsonSerializerChoice == JsonSerializerChoice.Newtonsoft)
				json = JsonConvert.SerializeObject(example, _newtonsoftJsonOption.Value.SerializerSettings);
			else
				json = JsonSerializer.Serialize(example);
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