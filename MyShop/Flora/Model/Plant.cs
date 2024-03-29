namespace Flora;

public partial class Plant
{
    public int PlantId { get; set; }

    public string Name { get; set; }

    public decimal? Price { get; set; }

    public string Description { get; set; }

    public int? StockQuantity { get; set; }

    public int? CategoryId { get; set; }

    public string Image { get; set; }

    public virtual PlantCategory Category { get; set; }
}