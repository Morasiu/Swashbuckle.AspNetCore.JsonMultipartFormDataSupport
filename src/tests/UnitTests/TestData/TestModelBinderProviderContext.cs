using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace UnitTests.TestData; 

public class TestModelBinderProviderContext : ModelBinderProviderContext {
	
	public static TestModelBinderProviderContext ForProperty(Type type, string propertyName) =>
		new TestModelBinderProviderContext(new TestModelMetadata(
			ModelMetadataIdentity.ForProperty(type.GetProperty(propertyName) ?? throw new InvalidOperationException(), type,
				type)));
	public TestModelBinderProviderContext(ModelMetadata metadata) {
		Metadata = metadata;
	}
	public override IModelBinder CreateBinder(ModelMetadata metadata) {
		throw new NotImplementedException();
	}

	public override BindingInfo BindingInfo { get; } = null!;
	public override ModelMetadata Metadata { get; }

	public override IModelMetadataProvider MetadataProvider { get; } = null!;
}