using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

class Program
{
    static void ShowMonthlySummary(BudgetManager manager)
    {
            Console.Write("Enter year (e.g., 2025): ");
            int year = int.Parse(Console.ReadLine());

            Console.Write("Enter month (1-12): ");
            int month = int.Parse(Console.ReadLine());

            var monthlyExpenses = manager.GetMonthlyExpenses(year, month);
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

    static void ShowCategoryReport(BudgetManager manager)
    {
        var report = manager.GetCategoryReport().ToList();

        if (report.Count == 0)
        {
                Console.WriteLine("No expenses recorded.");
                return;
        }

        Console.WriteLine("\n--- Category Report ---");

        foreach (var item in report)
        {
                Console.WriteLine($"{item.Category}: ${item.Total} ({item.Count} expenses)");
        }

        var top = report.First();
        Console.WriteLine($"\nTop category: {top.Category} (${top.Total})");
    }

    static void Main()
    {
        BudgetManager manager = new BudgetManager();
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

                    manager.AddExpense(amount, category, DateTime.Now);
                    Console.WriteLine("Expense added!");
                    break;


                case "2":
                    Console.WriteLine($"Total expenses: ${manager.GetTotal()}");
                    break;

                case "3":
                    running = false;
                    break;

                case "4":
                    ShowMonthlySummary(manager);
                    break;

                case "5":
                    ShowCategoryReport(manager);
                    break;

                case "6":
                    for (int i = 0; i < manager.Expenses.Count; i++)
                    {
                        var e = manager.Expenses[i];
                        Console.WriteLine($"{i}: ${e.Amount} - {e.Category} - {e.Date.ToShortDateString()}");
                    }
                    break;

                case "7":
                    Console.Write("Enter ID to edit: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                            Console.WriteLine("Invalid ID.");
                            break;
                    }

                    Console.Write("New amount (leave blank to skip): ");
                    string amtInput = Console.ReadLine();
                    double? newAmount = double.TryParse(amtInput, out double parsedAmt) ? parsedAmt : null;

                    Console.Write("New category (leave blank to skip): ");
                    string newCategory = Console.ReadLine();

                    Console.Write("New date YYYY-MM-DD (leave blank to skip): ");
                    string dateInput = Console.ReadLine();
                    DateTime? newDate = DateTime.TryParse(dateInput, out DateTime parsedDate) ? parsedDate : null;

                    manager.EditExpense(id, newAmount, newCategory, newDate);
                    Console.WriteLine("Expense updated!");
                    break;

                case "8":
                    Console.Write("Enter ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int delId))
                    {
                            manager.DeleteExpense(delId);
                            Console.WriteLine("Expense deleted!");
                    }
                    else
                    {
                            Console.WriteLine("Invalid ID.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}

