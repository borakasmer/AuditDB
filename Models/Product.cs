using System.ComponentModel.DataAnnotations;

public class Product{
    [Key]
    public int ID { get; set; } 
    public string Name { get; set; }
    public string SerieNo { get; set; }
    public int TotalCount { get; set; }
    public decimal Price { get; set; }
    public string WarehouseAddress { get; set; }
}