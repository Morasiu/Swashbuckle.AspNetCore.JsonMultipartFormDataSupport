using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;

namespace UnitTests;

public class NewtonsoftSerializationProvider : IMultipartJsonSerializationProvider
{
    private readonly JsonSerializerSettings _serializerSettings;
    
    public NewtonsoftSerializationProvider(IOptions<MvcNewtonsoftJsonOptions> options)
    {
        _serializerSettings = options.Value.SerializerSettings;
    }

    public NewtonsoftSerializationProvider()
    {
        _serializerSettings = new MvcNewtonsoftJsonOptions().SerializerSettings;
    }
    
    public string Serialize(object value)
    {
        return JsonConvert.SerializeObject(value, _serializerSettings);
    }

    public object Deserialize(ModelBindingContext bindingContext, string valueAsString)
    {
        return JsonConvert.DeserializeObject(valueAsString, bindingContext.ModelType, _serializerSettings)!;
    }
}