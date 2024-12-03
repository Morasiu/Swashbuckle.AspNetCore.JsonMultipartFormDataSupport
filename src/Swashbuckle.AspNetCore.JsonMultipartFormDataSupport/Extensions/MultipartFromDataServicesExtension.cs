using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions {
	/// <summary>
	/// Extensions for ASP.Net Core IServiceCollection
	/// </summary>
	public static class MultipartFromDataServicesExtension {
		/// <summary>
		/// Adds support for json in multipart/form-data requests
		/// </summary>
		public static IServiceCollection AddJsonMultipartFormDataSupport<TMultipartJsonSerializationProvider>(this IServiceCollection services) 
			where TMultipartJsonSerializationProvider : class, IMultipartJsonSerializationProvider
		{
			services.AddSingleton<IMultipartJsonSerializationProvider, TMultipartJsonSerializationProvider>();

			return AddJsonMultipartFormDataSupport(services);
		}
		
		/// <summary>
		/// Adds support for json in multipart/form-data requests
		/// </summary>
		public static IServiceCollection AddJsonMultipartFormDataSupport(this IServiceCollection services)
		{
			var serviceProvider = services.BuildServiceProvider();
			
			var serializationProvider = serviceProvider.GetRequiredService<IMultipartJsonSerializationProvider>();
			
			services.AddMvc(options =>
			{
				options.ModelBinderProviders.Insert(0, new FormDataJsonBinderProvider(new(serializationProvider)));
			});

			services.AddSwaggerGen(options => {
				options.OperationFilter<MultiPartJsonOperationFilter>();
			});
			return services;
		}
	}
}