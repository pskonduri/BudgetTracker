using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class BudgetManager
{
    private readonly string filePath = "expenses.json";
    public List<Expense> Expenses { get; private set; }

    public BudgetManager()
    {
        Expenses = LoadExpenses();
    }

    // -------------------------
    // Loading & Saving
    // -------------------------
    private List<Expense> LoadExpenses()
    {
        if (!File.Exists(filePath))
            return new List<Expense>();

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Expense>>(json)
               ?? new List<Expense>();
    }

    private void SaveExpenses()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(Expenses, options);
        File.WriteAllText(filePath, json);
    }

    // -------------------------
    // CRUD Operations
    // -------------------------
    public void AddExpense(double amount, string category, DateTime date)
    {
        Expenses.Add(new Expense(amount, category, date));
        SaveExpenses();
    }

    public void EditExpense(int id, double? amount, string category, DateTime? date)
    {
        if (id < 0 || id >= Expenses.Count)
            return;

        var exp = Expenses[id];

        if (amount.HasValue)
            exp.Amount = amount.Value;

        if (!string.IsNullOrWhiteSpace(category))
            exp.Category = category;

        if (date.HasValue)
            exp.Date = date.Value;

        SaveExpenses();
    }

    public void DeleteExpense(int id)
    {
        if (id < 0 || id >= Expenses.Count)
            return;

        Expenses.RemoveAt(id);
        SaveExpenses();
    }

    // -------------------------
    // Reports
    // -------------------------
    public double GetTotal()
    {
        return Expenses.Sum(e => e.Amount);
    }

    public List<Expense> GetMonthlyExpenses(int year, int month)
    {
        return Expenses
            .Where(e => e.Date.Year == year && e.Date.Month == month)
            .ToList();
    }

    public IEnumerable<(string Category, double Total, int Count)> GetCategoryReport()
    {
        return Expenses
            .GroupBy(e => e.Category)
            .Select(g => (g.Key, g.Sum(e => e.Amount), g.Count()))
            .OrderByDescending(x => x.Item2);
    }
}

