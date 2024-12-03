using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;

/// <summary>
/// Serialization provider for <see cref="JsonModelBinder"/> and <see cref="MultiPartJsonOperationFilter"/>.
/// </summary>
public interface IMultipartJsonSerializationProvider
{
    public object Deserialize(ModelBindingContext bindingContext, string valueAsString);
    public string Serialize(object value);
}