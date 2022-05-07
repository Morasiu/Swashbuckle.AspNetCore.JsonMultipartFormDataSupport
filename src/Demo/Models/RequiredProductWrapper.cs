using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace Demo.Models {
	public class RequiredProductWrapper {
		[Required]
		[FromJson] // <-- This attribute is required for binding.
		public Product Product { get; set; }
		
		[Required]
		public IFormFileCollection  Files { get; set; }
	}
}