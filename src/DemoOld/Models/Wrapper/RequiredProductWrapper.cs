using System.ComponentModel.DataAnnotations;
using DemoOld.Models.Products;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace DemoOld.Models.Wrapper {
	public class RequiredProductWrapper {
		[Required]
		[FromJson] // <-- This attribute is required for binding.
		public Product Product { get; set; } = null!;
		
		[Required]
		public IFormFileCollection  Files { get; set; } = new FormFileCollection();
	}
}