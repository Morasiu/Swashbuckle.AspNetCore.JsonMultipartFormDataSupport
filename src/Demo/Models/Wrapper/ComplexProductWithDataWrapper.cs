using Demo.Models.Products;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models.Wrapper
{
    public class ComplexProductWithDataWrapper
    {
        [FromJson]
        [Required]
        public Product Product { get; set; }

        [FromJson]
        public ProductData? ProductData { get; set; }

        // [FromJson] <-- not required
        [Required]
        public int? ProductId { get; set; }

        public string ProductName { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
