using System.ComponentModel.DataAnnotations;

namespace Demo.Models.Wrapper;

public class SimpleProductWrapper {
	// [FromJson] <-- not required
	[Required]
	public int? ProductId { get; set; }
		
	public string? ProductName { get; set; }
	public IFormFileCollection  Files { get; set; } = new FormFileCollection();
}