using System;
using System.Linq;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Models;

namespace Demo.Controllers {
	[Produces("application/json")]
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase {
		[HttpPost]
		public IActionResult Post([FromForm] MultipartFormData<Product> data) {
			var json = data.Json ?? throw new NullReferenceException(nameof(data));
			var image = data.File;
			return Ok(new {json, image?.FileName});
		}

		[HttpPost("wrapper")]
		public IActionResult PostWrapper([FromForm] ProductWrapper wrapper) {
			var wrapperProduct = wrapper.Product ?? throw new NullReferenceException(nameof(wrapper.Product));
			var images = wrapper.Files;
			return Ok(new {wrapperProduct, images = images?.Select(a => a.FileName)});
		}
	}
}