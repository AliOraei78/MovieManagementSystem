public abstract class Content
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
}

public class Film : Content
{
    public int Duration { get; set; }
    public string Director { get; set; } = string.Empty;
}

public class Documentary : Content
{
    public string Narrator { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
}