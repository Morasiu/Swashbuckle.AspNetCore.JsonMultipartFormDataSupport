using System.ComponentModel.DataAnnotations;
using Demo.Models.Products;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace Demo.Models.Wrapper;

public class ComplexProductWithDataWrapper
{
    [FromJson]
    [Required]
    public Product Product { get; set; } = null!;

    [FromJson]
    public ProductData? ProductData { get; set; }

    // [FromJson] <-- not required
    [Required]
    public int? ProductId { get; set; }

    public string? ProductName { get; set; }
    public IFormFileCollection Files { get; set; } = new FormFileCollection();
}