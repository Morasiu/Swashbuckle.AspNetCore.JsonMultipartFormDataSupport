using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport {
	/// <summary>
	/// Binds field from JSON string.
	/// </summary>
	public class JsonModelBinder : IModelBinder {
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
				var result = JsonSerializer.Deserialize(valueAsString, bindingContext.ModelType);
				if (result != null) {
					bindingContext.Result = ModelBindingResult.Success(result);
					return Task.CompletedTask;
				}
			}

			return Task.CompletedTask;
		}
	}
}