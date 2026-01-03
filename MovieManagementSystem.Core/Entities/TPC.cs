public abstract class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class Book : Product
{
    public string Author { get; set; } = string.Empty;
    public int Pages { get; set; }
}

public class DigitalBook : Product
{
    public string Format { get; set; } = "PDF";
    public long FileSizeKB { get; set; }
}