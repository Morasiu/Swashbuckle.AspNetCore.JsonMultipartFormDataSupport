using System;
using System.Linq;
using Demo.Models.Products;
using Demo.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
			return Ok(new { json, image?.FileName });
		}

		[HttpPost("required")]
		public IActionResult Post([FromForm] MultipartRequiredFormData<Product> data) {
			var json = data.Json ?? throw new NullReferenceException(nameof(data));
			var image = data.File;
			return Ok(new { json, image?.FileName });
		}

		[HttpPost("wrapper/required")]
		public IActionResult Post([FromForm] RequiredProductWrapper wrapper) {
			var wrapperProduct = wrapper.Product ?? throw new NullReferenceException(nameof(wrapper.Product));
			var images = wrapper.Files;
			return Ok(new { wrapperProduct, images = images.Select(a => a.FileName) });
		}

		[HttpPost("wrapper")]
		public IActionResult PostWrapper([FromForm] ProductWrapper wrapper) {
			var wrapperProduct = wrapper.Product ?? throw new NullReferenceException(nameof(wrapper.Product));
			var images = wrapper.Files;
			return Ok(new { wrapperProduct, images = images.Select(a => a.FileName) });
		}

		[HttpPost("wrapper/simple")]
		public IActionResult PostWrapper([FromForm] SimpleProductWrapper wrapper) {
			var productName = wrapper.ProductName;
			var productId = wrapper.ProductId ?? throw new NullReferenceException(nameof(wrapper.ProductId));
			var images = wrapper.Files;
			return Ok(new { productName, productId, images = images.Select(a => a.FileName) });
		}

		[HttpPost("wrapper/complex")]
		public IActionResult PostWrapper([FromForm] ComplexProductWrapper wrapper) {
			var productName = wrapper.ProductName;
			var productId = wrapper.ProductId ?? throw new NullReferenceException(nameof(wrapper.ProductId));
			var product = wrapper.Product;
			var images = wrapper.Files;
			return Ok(new {
				productName, 
				productId, 
				product = JsonConvert.SerializeObject(product),
				images = images.Select(a => a.FileName)
			});
		}

        [HttpPost("wrapper/complex-data")]
        public IActionResult PostDataWrapper([FromForm] ComplexProductWithDataWrapper wrapper)
        {
            var productName = wrapper.ProductName;
            var productId = wrapper.ProductId ?? throw new NullReferenceException(nameof(wrapper.ProductId));
            var product = wrapper.Product;
            var images = wrapper.Files;
            var data = wrapper.ProductData;
            return Ok(new
            {
                productName,
                productId,
                product = JsonConvert.SerializeObject(product),
                images = images.Select(a => a.FileName),
                data = JsonConvert.SerializeObject(data)
            });
        }
    }
}