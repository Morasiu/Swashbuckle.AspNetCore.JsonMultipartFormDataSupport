[![https://www.nuget.org/packages/Swashbuckle.AspNetCore.JsonMultipartFormDataSupport](https://img.shields.io/nuget/v/Swashbuckle.AspNetCore.JsonMultipartFormDataSupport)](https://www.nuget.org/packages/Swashbuckle.AspNetCore.JsonMultipartFormDataSupport/)
[![https://www.nuget.org/packages/Swashbuckle.AspNetCore.JsonMultipartFormDataSupport](https://img.shields.io/nuget/dt/Swashbuckle.AspNetCore.JsonMultipartFormDataSupport)](https://www.nuget.org/packages/Swashbuckle.AspNetCore.JsonMultipartFormDataSupport/)

# Swashbuckle.AspNetCore.JsonMultipartFormDataSupport
Adds support for json in multipart/form-data requests.

![Exmaple](Example.png)

# Usage

1. Simple add this to your `ConfigureServices`

```csharp
services.AddJsonMultipartFormDataSupport(JsonSerializerChoice.Newtonsoft);
```

Or manually:

* Binder

```csharp
services
    .AddMvc(
        properties => {
            // ...
            properties.ModelBinderProviders.Insert(0, new FormDataJsonBinderProvider()); // Here
        }
    )
```

* Operation filter

```csharp
services.AddSwaggerGen(c => {
        c.OperationFilter<MultiPartJsonOperationFilter>();
    });
```

2. Add to your `Controller` 

```csharp
[HttpPost]
[Consumes("multipart/form-data")] 
public async Task<IActionResult> Post([FromForm] MultipartFormData<Product> multiPartData) {
    var file = multiPartData.File;
    var product = multiPartData.Json;
}
```
or

```csharp
[HttpPost]
[Consumes("multipart/form-data")] 
public async Task<IActionResult> Post([FromForm] MultipartRequiredFormData<Product> multiPartData) {
    var file = multiPartData.File;
    var product = multiPartData.Json;
}
```

Or you can create your on wrapper

```csharp
public class MyWrapper {
    [FromJson] // <-- This attribute is required for binding.
    public MyModel Json { get; set; }

    public IFormFile File { get; set; }
}
```

and then
```csharp
[HttpPost]
[Consumes("multipart/form-data")] 
public async Task<IActionResult> Post([FromForm] MyWrapper myWrapper) {
    // code
}
```

Notes:

It automatically adds examples from class which implements `IExampleProvider<MyModel>`.

<a href="https://www.buymeacoffee.com/morasiu" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: 41px !important;width: 174px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a>
