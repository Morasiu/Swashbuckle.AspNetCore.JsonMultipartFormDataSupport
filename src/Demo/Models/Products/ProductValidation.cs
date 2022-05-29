using FluentValidation;

namespace Demo.Models.Products; 

public class ProductValidation : AbstractValidator<Product> {
	public ProductValidation() {
		RuleFor(a => a.Name).NotEmpty();
	}
}