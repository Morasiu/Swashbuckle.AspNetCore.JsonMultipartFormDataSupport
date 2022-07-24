using FluentValidation;

namespace Demo.Models.Products; 

public class ProductValidator : AbstractValidator<Product> {
	public ProductValidator() {
		RuleFor(a => a.Name).NotEmpty();
	}
}