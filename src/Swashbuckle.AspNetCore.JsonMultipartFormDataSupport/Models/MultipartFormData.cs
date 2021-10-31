using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Models {
	/// <summary>
	/// Wrapper for sending json with image in multipart/form-data
	/// </summary>
	/// <typeparam name="TJsonPart"></typeparam>
	public class MultipartFormData<TJsonPart> {
		/// <summary>
		/// Object of type <typeparamref name="TJsonPart"/> mapped from JSON.
		/// </summary>
		[FromJson]
		public TJsonPart Json { get; set; }

		/// <summary>
		/// IFormFile form multipart/form-data
		/// </summary>
		public IFormFile File { get; set; }
	}
}