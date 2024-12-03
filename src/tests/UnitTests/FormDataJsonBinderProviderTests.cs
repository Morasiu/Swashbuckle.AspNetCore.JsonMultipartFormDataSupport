using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;
using UnitTests.TestData;
using UnitTests.TestData.Types;

namespace UnitTests;

public class FormDataJsonBinderProviderTests {
	[Test]
	public void GetBinder_ContextIsNull_ShouldThrowException() {
		// Arrange
		var options = Substitute.For<IOptions<JsonOptions>>();
		var sut = new FormDataJsonBinderProvider(new(new JsonSerializationProvider(options)));
		// Act
		var action = () => sut.GetBinder(null!);
		// Assert
		action.Should().Throw<ArgumentNullException>();
	}

	[TestCase(typeof(int))]
	[TestCase(typeof(string))]
	[TestCase(typeof(long))]
	[TestCase(typeof(double))]
	public void GetBinder_SimpleType_ShouldReturnNull(Type type) {
		// Arrange
		var options = Substitute.For<IOptions<JsonOptions>>();
		var sut = new FormDataJsonBinderProvider(new(new JsonSerializationProvider(options)));
		var context = new TestModelBinderProviderContext(new TestModelMetadata(ModelMetadataIdentity.ForType(type)));
		// Act
		var result = sut.GetBinder(context);
		// Assert
		result.Should().BeNull();
	}

	[Test]
	public void GetBinder_NotProperty_ShouldReturnNull() {
		// Arrange
		var options = Substitute.For<IOptions<JsonOptions>>();
		var sut = new FormDataJsonBinderProvider(new(new JsonSerializationProvider(options)));
		var context =
			new TestModelBinderProviderContext(
				new TestModelMetadata(ModelMetadataIdentity.ForType(typeof(TestTypeNoProperty))));
		// Act
		var result = sut.GetBinder(context);
		// Assert
		result.Should().BeNull();
	}

	[Test]
	public void GetBinder_IFormFileProperty_ShouldReturnNull() {
		// Arrange
		var options = Substitute.For<IOptions<JsonOptions>>();
		var sut = new FormDataJsonBinderProvider(new(new JsonSerializationProvider(options)));
		var context =
			TestModelBinderProviderContext.ForProperty(typeof(TestTypePropertyIFromFile), nameof(TestTypePropertyIFromFile.Test));
		// Act
		var result = sut.GetBinder(context);
		// Assert
		result.Should().BeNull();
	}

	[Test]
	public void GetBinder_PropertyWithoutFromJsonAttribute_ShouldReturnNull() {
		// Arrange
		var options = Substitute.For<IOptions<JsonOptions>>();
		var sut = new FormDataJsonBinderProvider(new(new JsonSerializationProvider(options)));
		var context =
			TestModelBinderProviderContext.ForProperty(typeof(TestTypeNoAttribute), nameof(TestTypeNoAttribute.Test));
		// Act
		var result = sut.GetBinder(context);
		// Assert
		result.Should().BeNull();
	}

	[Test]
	public void GetBinder_ShouldReturnJsonBinder() {
		// Arrange
		var options = Substitute.For<IOptions<JsonOptions>>();
		var sut = new FormDataJsonBinderProvider(new(new JsonSerializationProvider(options)));
		var context = TestModelBinderProviderContext.ForProperty(typeof(TestTypeContainer), nameof(TestTypeContainer.Test));
		// Act
		var result = sut.GetBinder(context);
		// Assert
		result.Should().NotBeNull().And.BeAssignableTo<JsonModelBinder>();
	}
}