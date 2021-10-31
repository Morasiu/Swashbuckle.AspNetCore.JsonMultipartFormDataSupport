namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions {
	internal static class StringExtension {
		internal static string ToCamelCase(this string str) {
			if (!string.IsNullOrEmpty(str) && str.Length > 1) {
				return char.ToLowerInvariant(str[0]) + str.Substring(1);
			}

			return str;
		}
	}
}