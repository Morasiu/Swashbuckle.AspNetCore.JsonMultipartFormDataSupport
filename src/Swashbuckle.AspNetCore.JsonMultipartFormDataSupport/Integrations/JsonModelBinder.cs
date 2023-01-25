using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations {
	/// <summary>
	/// Binds field from JSON string.
	/// </summary>
	public class JsonModelBinder : IModelBinder {
		private readonly IOptions<JsonOptions> _jsonOptions;
		private readonly IOptions<MvcNewtonsoftJsonOptions> _newtonsoftJsonOptions;

		public JsonModelBinder() { }

		public JsonModelBinder(IOptions<JsonOptions> jsonOptions) {
			_jsonOptions = jsonOptions;
		}

		public JsonModelBinder(IOptions<MvcNewtonsoftJsonOptions> newtonsoftJsonOptions) {
			_newtonsoftJsonOptions = newtonsoftJsonOptions;
		}

		/// <inheritdoc />
		public async Task BindModelAsync(ModelBindingContext bindingContext) {
			if (bindingContext == null) {
				throw new ArgumentNullException(nameof(bindingContext));
			}

			string modelBindingKey;
			if (bindingContext.IsTopLevelObject) {
				modelBindingKey = bindingContext.BinderModelName;
			}
			else {
				modelBindingKey = bindingContext.ModelName;
			}

			// Check the value sent in
			var valueProviderResult = await this.GetValueProvidedResult(bindingContext);
			if (valueProviderResult != ValueProviderResult.None) {
				bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

				// Attempt to convert the input value
				var valueAsString = valueProviderResult.FirstValue;

				try {
					object result;
					if (_jsonOptions != null) {
						result = DeserializeUsingSystemSerializer(bindingContext, valueAsString);
					}
					else if (_newtonsoftJsonOptions != null) {
						result = DeserializeUsingJsonNet(bindingContext, valueAsString);
					}
					else {
						result = DeserializeUsingSystemSerializer(bindingContext, valueAsString);
					}

					bindingContext.Result = ModelBindingResult.Success(result);
				}
				catch (Exception e) {
					bindingContext.ModelState.AddModelError(modelBindingKey ?? string.Empty, e.Message);
				}
			}
        }

		private object DeserializeUsingSystemSerializer(ModelBindingContext bindingContext, string valueAsString) {
			return JsonSerializer.Deserialize(valueAsString, bindingContext.ModelType,
				_jsonOptions?.Value?.JsonSerializerOptions ?? new JsonSerializerOptions());
		}

		private object DeserializeUsingJsonNet(ModelBindingContext bindingContext, string valueAsString) {
			return JsonConvert.DeserializeObject(valueAsString, bindingContext.ModelType,
				_newtonsoftJsonOptions.Value.SerializerSettings);
		}

        private async Task<ValueProviderResult> GetValueProvidedResult(ModelBindingContext bindingContext) {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult != ValueProviderResult.None) {
                return valueProviderResult;
            }

            var file = bindingContext.HttpContext.Request.Form.Files.GetFile(bindingContext.ModelName);
            if (file is null) {
                return valueProviderResult;
            }

            await using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            valueProviderResult = new ValueProviderResult(json);

            return valueProviderResult;
        }
    }
}