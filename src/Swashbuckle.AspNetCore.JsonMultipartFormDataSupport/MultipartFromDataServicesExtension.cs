using Microsoft.Extensions.DependencyInjection;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport {
	/// <summary>
	/// Extensions for ASP.Net Core IServiceCollection
	/// </summary>
	public static class MultipartFromDataServicesExtension {
		/// <summary>
		/// Adds support for json in multipart/form-data requests
		/// </summary>
		public static IServiceCollection AddJsonMultipartFormDataSupport(this IServiceCollection services) {
			services.AddMvc(options => {
				options.ModelBinderProviders.Insert(0, new FormDataJsonBinderProvider());
			});

			services.AddSwaggerGen(options => {
				options.OperationFilter<MultiPartJsonOperationFilter>();
			});
			return services;
		}
	}
}