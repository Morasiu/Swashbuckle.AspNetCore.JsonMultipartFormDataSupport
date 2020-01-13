using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport;

namespace Demo.Controllers {
	[Produces("application/json")]
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase {
		private readonly ILogger<ProductController> _logger;

		public ProductController(ILogger<ProductController> logger) {
			_logger = logger;
		}

		[HttpPost]
		public IActionResult Post([FromForm] MultipartFormData<Product> data) {
			var json = data.Json ?? throw new NullReferenceException(nameof(data));
			var image = data.File;
			return Ok();
		}
	}
}
