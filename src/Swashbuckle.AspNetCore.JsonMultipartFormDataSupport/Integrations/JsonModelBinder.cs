using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;

/// <summary>
/// Binds field from JSON string.
/// </summary>
public class JsonModelBinder : IModelBinder {
	readonly IMultipartJsonSerializationProvider _serializationProvider;

	public JsonModelBinder(IMultipartJsonSerializationProvider serializationProvider) {
		_serializationProvider = serializationProvider;
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
				var result = _serializationProvider.Deserialize(bindingContext, valueAsString);

				bindingContext.Result = ModelBindingResult.Success(result);
			}
			catch (Exception e) {
				bindingContext.ModelState.AddModelError(modelBindingKey ?? string.Empty, e.Message);
			}
		}
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