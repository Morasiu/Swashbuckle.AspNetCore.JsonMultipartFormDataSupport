using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport {
	/// <summary>
	/// Suggest that this form file should be deserialized from JSON.
	/// </summary>
	public class FromJsonAttribute : FromFormAttribute { }
}