public abstract class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class Member : Person
{
    public DateTime MembershipDate { get; set; }
}

public class Librarian : Person
{
    public decimal Salary { get; set; }
}