using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace UnitTests.TestData.Types; 

public class TestTypeContainer {
	[FromJson]
	public string? Test { get; set; }
}