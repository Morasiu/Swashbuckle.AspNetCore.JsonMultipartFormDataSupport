using Demo.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Demo {
	public class ProductExamples : IExamplesProvider<Product> {
		public Product GetExamples() {
			var product = new Product {
				Name = "Example",
				Type = ProductType.Phone
			};
			return product;
		}
	}
}