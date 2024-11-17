﻿using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using System.ComponentModel.DataAnnotations;
using DemoOld.Models.Products;

namespace DemoOld.Models.Wrapper
{
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
}
