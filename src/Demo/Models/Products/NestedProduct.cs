namespace Demo.Models.Products;

public sealed class NestedProduct
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public ProductType Type { get; set; }
    public required NestedProductData Data { get; set; }
}

public sealed class NestedProductData
{
    public double Price { get; set; }
    public DateTime StartDate { get; set; }
}