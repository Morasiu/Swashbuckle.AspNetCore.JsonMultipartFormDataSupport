using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport {
	/// <summary>
	/// Binds field from JSON string.
	/// </summary>
	public class JsonModelBinder : IModelBinder {
		private readonly IOptions<JsonOptions> _jsonOptions;

		public JsonModelBinder(IOptions<JsonOptions> jsonOptions) {
			_jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
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
				var result = JsonSerializer.Deserialize(valueAsString, bindingContext.ModelType, _jsonOptions.Value.JsonSerializerOptions);
				if (result != null) {
					bindingContext.Result = ModelBindingResult.Success(result);
					return Task.CompletedTask;
				}
			}

			return Task.CompletedTask;
		}
	}
}