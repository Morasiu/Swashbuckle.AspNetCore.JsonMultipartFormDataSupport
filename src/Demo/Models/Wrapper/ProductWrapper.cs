using Demo.Models.Products;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace Demo.Models.Wrapper {
	public class ProductWrapper {
		[FromJson] // <-- This attribute is required for binding.
		public Product Product { get; set; }

		public IFormFileCollection  Files { get; set; }
	}
}