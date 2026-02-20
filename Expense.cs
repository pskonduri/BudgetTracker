public class Expense
{
    public double Amount { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }

    public Expense(double amount, string category, DateTime date)
    {
        Amount = amount;
        Category = category;
        Date = date;
    }

    public Expense() { } // Needed for JSON deserialization
}

