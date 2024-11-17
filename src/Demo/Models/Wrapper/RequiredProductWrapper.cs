using System.ComponentModel.DataAnnotations;
using Demo.Models.Products;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace Demo.Models.Wrapper;

public class RequiredProductWrapper {
	[Required]
	[FromJson] // <-- This attribute is required for binding.
	public Product Product { get; set; } = null!;
		
	[Required]
	public IFormFileCollection  Files { get; set; } = new FormFileCollection();
}