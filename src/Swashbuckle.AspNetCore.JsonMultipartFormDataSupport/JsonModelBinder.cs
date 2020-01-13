using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport {
	/// <summary>
	/// Binds field from JSON string.
	/// </summary>
	public class JsonModelBinder : IModelBinder {
		private readonly IOptions<JsonOptions> _jsonOptions;
		private readonly IOptions<MvcNewtonsoftJsonOptions> _newtonsoftJsonOptions;

		public JsonModelBinder(){}

		public JsonModelBinder(IOptions<JsonOptions> jsonOptions) {
			_jsonOptions = jsonOptions;
		}

		public JsonModelBinder(IOptions<MvcNewtonsoftJsonOptions> newtonsoftJsonOptions) {
			_newtonsoftJsonOptions = newtonsoftJsonOptions;
		}
		/// <inheritdoc />
		public Task BindModelAsync(ModelBindingContext bindingContext) {
			if (bindingContext == null) {
				throw new ArgumentNullException(nameof(bindingContext));
			}

			// Check the value sent in
			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (valueProviderResult != ValueProviderResult.None) {
				bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

				// Attempt to convert the input value
				var valueAsString = valueProviderResult.FirstValue;

				object result;
				if (_jsonOptions != null)
					result = JsonSerializer.Deserialize(valueAsString, bindingContext.ModelType, _jsonOptions.Value.JsonSerializerOptions);
				else if (_newtonsoftJsonOptions != null)
					result = JsonConvert.DeserializeObject(valueAsString, bindingContext.ModelType,
						_newtonsoftJsonOptions.Value.SerializerSettings);
				else
					result = JsonSerializer.Deserialize(valueAsString, bindingContext.ModelType);
				if (result != null) {
					bindingContext.Result = ModelBindingResult.Success(result);
					return Task.CompletedTask;
				}
			}

			return Task.CompletedTask;
		}
	}
}