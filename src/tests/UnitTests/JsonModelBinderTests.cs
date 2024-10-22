using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;
using UnitTests.TestData.Types;

namespace UnitTests;

public class JsonModelBinderTests {
	[Test]
	public void BindModelAsync_NullContext_ShouldReturnNull() {
		// Arrange
		var sut = new JsonModelBinder();
		// Act
		var action = async () => await sut.BindModelAsync(null!);
		// Assert
		action.Should().ThrowExactlyAsync<ArgumentNullException>();
	}

	[Test]
	public async Task BindModelAsync_ShouldBindData() {
		// Arrange
		var sut = new JsonModelBinder();
		var testType = new TestType {
			Id = 1,
			Text = Guid.NewGuid().ToString()
		};
		var context = Substitute.For<ModelBindingContext>();
		context.ValueProvider.GetValue(nameof(TestTypeContainer.Test))
		       .Returns(_ => new ValueProviderResult(new StringValues(new[] { JsonSerializer.Serialize(testType) })));
		context.ModelName.Returns(_ => nameof(TestTypeContainer.Test));
		context.ModelType.Returns(_ => typeof(TestType));
		context.ModelState.Returns(_ => new ModelStateDictionary());
		// Act
		await sut.BindModelAsync(context);
		// Assert
		context.Result.IsModelSet.Should().BeTrue();
		context.Result.Model.Should().NotBeNull().And.BeAssignableTo<TestType>();
		var result = (TestType)context.Result.Model!;
		result.Id.Should().Be(testType.Id);
		result.Text.Should().Be(testType.Text);
	}
}