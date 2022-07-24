using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace UnitTests.TestData;

public class TestModelMetadata : ModelMetadata {
	public TestModelMetadata(ModelMetadataIdentity identity) : base(identity) { }
	public override IReadOnlyDictionary<object, object> AdditionalValues { get; } = null!;
	public override ModelPropertyCollection Properties { get; } = null!;
	public override string BinderModelName => string.Empty;
	public override Type BinderType => throw new NotImplementedException();
	public override BindingSource BindingSource => throw new NotImplementedException();
	public override bool ConvertEmptyStringToNull => throw new NotImplementedException();
	public override string DataTypeName => throw new NotImplementedException();
	public override string Description => throw new NotImplementedException();
	public override string DisplayFormatString => throw new NotImplementedException();
	public override string DisplayName => throw new NotImplementedException();
	public override string EditFormatString => throw new NotImplementedException();
	public override ModelMetadata ElementMetadata => throw new NotImplementedException();
	public override IEnumerable<KeyValuePair<EnumGroupAndName, string>> EnumGroupedDisplayNamesAndValues => throw new NotImplementedException();
	public override IReadOnlyDictionary<string, string> EnumNamesAndValues => throw new NotImplementedException();
	public override bool HasNonDefaultEditFormat => throw new NotImplementedException();
	public override bool HtmlEncode => throw new NotImplementedException();
	public override bool HideSurroundingHtml => throw new NotImplementedException();
	public override bool IsBindingAllowed => throw new NotImplementedException();
	public override bool IsBindingRequired => throw new NotImplementedException();
	public override bool IsEnum => throw new NotImplementedException();
	public override bool IsFlagsEnum => throw new NotImplementedException();
	public override bool IsReadOnly => throw new NotImplementedException();
	public override bool IsRequired => throw new NotImplementedException();
	public override ModelBindingMessageProvider ModelBindingMessageProvider { get; } = null!;
	public override int Order => throw new NotImplementedException();
	public override string Placeholder => throw new NotImplementedException();
	public override string NullDisplayText => throw new NotImplementedException();
	public override IPropertyFilterProvider PropertyFilterProvider => throw new NotImplementedException();
	public override bool ShowForDisplay => throw new NotImplementedException();
	public override bool ShowForEdit => throw new NotImplementedException();
	public override string SimpleDisplayProperty => throw new NotImplementedException();
	public override string TemplateHint => throw new NotImplementedException();
	public override bool ValidateChildren => throw new NotImplementedException();
	public override IReadOnlyList<object> ValidatorMetadata { get; } = null!;
	public override Func<object, object?> PropertyGetter => throw new NotImplementedException();
	public override Action<object, object?> PropertySetter => throw new NotImplementedException();
}