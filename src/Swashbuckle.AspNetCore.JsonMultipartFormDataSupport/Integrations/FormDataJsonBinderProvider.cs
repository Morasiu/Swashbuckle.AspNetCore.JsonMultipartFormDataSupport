using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations {
	/// <summary>
	/// Looks for field with <see cref="FromJsonAttribute"/> and use <see cref="JsonModelBinder"/> for binder.
	/// </summary>
	public class FormDataJsonBinderProvider : IModelBinderProvider {
		readonly JsonModelBinder _jsonModelBinder;

		public FormDataJsonBinderProvider(JsonModelBinder jsonModelBinder)
		{
			_jsonModelBinder = jsonModelBinder;
		}
		
		/// <inheritdoc />
		public IModelBinder GetBinder(ModelBinderProviderContext context) {
			if (context == null) throw new ArgumentNullException(nameof(context));

			// Do not use this provider for binding simple values
			if (!context.Metadata.IsComplexType) return null;

			// Do not use this provider if the binding target is not a property
			var propName = context.Metadata.PropertyName;
			if (propName is null) return null;
			var propInfo = context.Metadata.ContainerType?.GetProperty(propName);
			if (propInfo == null) return null;

			// Do not use this provider if the target property type implements IFormFile
			if (propInfo.PropertyType.IsAssignableFrom(typeof(IFormFile))) return null;

			// Do not use this provider if this property does not have the From attribute
			if (propInfo.GetCustomAttribute<FromJsonAttribute>() == null) return null;

			return _jsonModelBinder;
		}
	}
}