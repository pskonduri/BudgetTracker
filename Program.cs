using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<Expense> LoadExpenses()
    {
        string filePath = "expenses.txt";
        List<Expense> expenses = new List<Expense>();

        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                expenses.Add(Expense.FromString(line));
            }
        }

        return expenses;
    }

    static void SaveExpenses(List<Expense> expenses)
    {
        string filePath = "expenses.txt";
        File.WriteAllLines(filePath, expenses.Select(e => e.ToString()));
    }

    static void ShowMonthlySummary(List<Expense> expenses)
    {
            Console.Write("Enter year (e.g., 2025): ");
            int year = int.Parse(Console.ReadLine());

            Console.Write("Enter month (1-12): ");
            int month = int.Parse(Console.ReadLine());

            var monthlyExpenses = expenses
                    .Where(e => e.Date.Year == year && e.Date.Month == month)
                    .ToList();

            if (monthlyExpenses.Count == 0)
            {
                    Console.WriteLine("No expenses found for that month.");
                    return;
            }

            double total = monthlyExpenses.Sum(e => e.Amount);

            Console.WriteLine($"\nSummary for {month}/{year}:");
            Console.WriteLine($"Total spent: ${total}");

            Console.WriteLine("\nBreakdown:");
            foreach (var exp in monthlyExpenses)
            {
                    Console.WriteLine($"{exp.Date.ToShortDateString()} - {exp.Category} - ${exp.Amount}");
            }
    }

    static void ShowCategoryReport(List<Expense> expenses)
    {
            if (expenses.Count == 0)
            {
                    Console.WriteLine("No expenses recorded.");
                    return;
            }

            var grouped = expenses
                    .GroupBy(e => e.Category)
                    .Select(g => new
                                    {
                                    Category = g.Key,
                                    Total = g.Sum(e => e.Amount),
                                    Count = g.Count()
                                    })
            .OrderByDescending(g => g.Total)
                    .ToList();

            Console.WriteLine("\n--- Category Report ---");

            foreach (var item in grouped)
            {
                    Console.WriteLine($"{item.Category}: ${item.Total} ({item.Count} expenses)");
            }

            var top = grouped.First();
            Console.WriteLine($"\nTop category: {top.Category} (${top.Total})");
    }

    static void ListExpenses(List<Expense> expenses)
    {
            if (expenses.Count == 0)
            {
                    Console.WriteLine("No expenses recorded.");
                    return;
            }

            Console.WriteLine("\n--- All Expenses ---");

            for (int i = 0; i < expenses.Count; i++)
            {
                    var e = expenses[i];
                    Console.WriteLine($"{i}: ${e.Amount} - {e.Category} - {e.Date.ToShortDateString()}");
            }
    }

    static void EditExpense(List<Expense> expenses)
    {
        ListExpenses(expenses);

        Console.Write("\nEnter the ID of the expense to edit: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id < 0 || id >= expenses.Count)
        {
                Console.WriteLine("Invalid ID.");
                return;
        }

        var exp = expenses[id];

        Console.Write($"New amount (current: {exp.Amount}): ");
        if (double.TryParse(Console.ReadLine(), out double newAmount))
                exp.Amount = newAmount;

        Console.Write($"New category (current: {exp.Category}): ");
        string newCategory = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newCategory))
                exp.Category = newCategory;

        Console.Write($"New date (current: {exp.Date.ToShortDateString()}), format YYYY-MM-DD: ");
        string newDate = Console.ReadLine();
        if (DateTime.TryParse(newDate, out DateTime parsedDate))
                exp.Date = parsedDate;

        SaveExpenses(expenses);
        Console.WriteLine("Expense updated!");
    }

    static void DeleteExpense(List<Expense> expenses)
    {
        ListExpenses(expenses);

        Console.Write("\nEnter the ID of the expense to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id < 0 || id >= expenses.Count)
        {
                Console.WriteLine("Invalid ID.");
                return;
        }

        expenses.RemoveAt(id);
        SaveExpenses(expenses);

        Console.WriteLine("Expense deleted!");
    }

    static void Main()
    {
        List<Expense> expenses = LoadExpenses();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- Budget Tracker ---");
            Console.WriteLine("1. Add Expense");
            Console.WriteLine("2. View Total");
            Console.WriteLine("3. Exit");
            Console.WriteLine("4. Monthly Summary");
            Console.WriteLine("5. Category Report");
            Console.WriteLine("6. List All Expenses");
            Console.WriteLine("7. Edit an Expense");
            Console.WriteLine("8. Delete an Expense");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter amount: ");
                    if (!double.TryParse(Console.ReadLine(), out double amount))
                    {
                        Console.WriteLine("Invalid amount.");
                        break;
                    }

                    Console.Write("Enter category: ");
                    string category = Console.ReadLine();

                    DateTime date = DateTime.Now;

                    expenses.Add(new Expense(amount, category, date));
                    SaveExpenses(expenses);

                    Console.WriteLine("Expense added!");
                    break;
 
                case "2":
                    double total = expenses.Sum(e => e.Amount);
                    Console.WriteLine($"Total expenses: ${total}");
                    break;

                case "3":
                    running = false;
                    break;

                case "4":
                    ShowMonthlySummary(expenses);
                    break;

                case "5":
                    ShowCategoryReport(expenses);
                    break;

                case "6":
                    ListExpenses(expenses);
                    break;

                case "7":
                    EditExpense(expenses);
                    break;

                case "8":
                    DeleteExpense(expenses);
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}

