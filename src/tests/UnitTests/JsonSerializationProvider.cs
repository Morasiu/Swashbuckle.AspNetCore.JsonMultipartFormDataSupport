using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;

namespace UnitTests;

public class JsonSerializationProvider : IMultipartJsonSerializationProvider
{
    private readonly JsonSerializerOptions _serializerOptions;
    
    public JsonSerializationProvider(IOptions<JsonOptions> options)
    {
        _serializerOptions = options.Value?.JsonSerializerOptions ?? new JsonOptions().JsonSerializerOptions;
    }

    public JsonSerializationProvider()
    {
        _serializerOptions = new JsonOptions().JsonSerializerOptions;
    }
    
    public string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, _serializerOptions);
    }

    public object Deserialize(ModelBindingContext bindingContext, string valueAsString)
    {
        return JsonSerializer.Deserialize(valueAsString, bindingContext.ModelType, _serializerOptions)!;
    }
}