using System;
using System.Collections.Generic;
using System.IO;

// Base Employee Class (Abstract)
abstract class BaseEmployee
{
    public string Name { get; set; }
    public int ID { get; set; }
    public string Role { get; set; }
    public double BasicPay { get; set; }
    public double Allowances { get; set; }
    public double Deductions { get; set; }

    // Constructor to initialize employee details
    public BaseEmployee(string name, int id, string role, double basicPay, double allowances, double deductions)
    {
        Name = name;
        ID = id;
        Role = role;
        BasicPay = basicPay;
        Allowances = allowances;
        Deductions = deductions;
    }

    // Method to calculate salary
    public  double CalculateSalary()
    {
        return BasicPay + Allowances - Deductions;
    }

    // Method to return formatted employee details
    public override string ToString()
    {
        return $"ID: {ID}, Name: {Name}, Role: {Role}, Salary: {CalculateSalary():F2}";
    }
}

// Derived classes representing specific employee roles
class Manager : BaseEmployee
{
    public Manager(string name, int id, double basicPay, double allowances, double deductions)
        : base(name, id, "Manager", basicPay, allowances, deductions) { }
}

class Developer : BaseEmployee
{
    public Developer(string name, int id, double basicPay, double allowances, double deductions)
        : base(name, id, "Developer", basicPay, allowances, deductions) { }
}

class Intern : BaseEmployee
{
    public Intern(string name, int id, double basicPay, double allowances, double deductions)
        : base(name, id, "Intern", basicPay, allowances, deductions) { }
}

// Payroll System Class to manage employees
class PayrollSystem
{
    private static List<BaseEmployee> employees = new List<BaseEmployee>();
    private static string filePath = "employees.txt";

    static void Main()
    {
        // Load employees from file
        LoadEmployees(); 
        while (true)
        {
            Console.WriteLine("\nEmployee Payroll System");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Display Employees");
            Console.WriteLine("3. Calculate Total Payroll");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddEmployee();
                    break;
                case "2":
                    DisplayEmployees();
                    break;
                case "3":
                    CalculateTotalPayroll();
                    break;
                case "4":
                    SaveEmployees(); 
                    return;
                default:
                    Console.WriteLine("Invalid choice! Try again.");
                    break;
            }
        }
    }

    // Method to add a new employee
    public static void AddEmployee()
    {
        string name;
        while (true)
        {
            Console.Write("Enter Name: ");
            name = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(name) && name.All(c => char.IsLetter(c) || c == ' '))
                break;
            Console.WriteLine("Invalid input! Name should only contain alphabets and spaces.");
        }

        int id;
        while (true)
        {
            Console.Write("Enter ID: ");
            if (int.TryParse(Console.ReadLine(), out id)) break;
            Console.WriteLine("Invalid input! ID must be a number.");
        }

        string role;
        while (true)
        {
            Console.Write("Enter Role (Manager/Developer/Intern): ");
            role = Console.ReadLine().ToLower();
            if (role == "manager" || role == "developer" || role == "intern") break;
            Console.WriteLine("Invalid role! Please enter Manager, Developer, or Intern.");
        }

        double basicPay;
        while (true)
        {
            Console.Write("Enter Basic Pay: ");
            if (double.TryParse(Console.ReadLine(), out basicPay) && basicPay >= 0) break;
            Console.WriteLine("Invalid input! Basic Pay must be a non-negative number.");
        }

        double allowances;
        while (true)
        {
            Console.Write("Enter Allowances: ");
            if (double.TryParse(Console.ReadLine(), out allowances) && allowances >= 0) break;
            Console.WriteLine("Invalid input! Allowances must be a non-negative number.");
        }

        double deductions;
        while (true)
        {
            Console.Write("Enter Deductions: ");
            if (double.TryParse(Console.ReadLine(), out deductions) && deductions >= 0) break;
            Console.WriteLine("Invalid input! Deductions must be a non-negative number.");
        }

        BaseEmployee employee;
        if (role == "manager")
        {
            employee = new Manager(name, id, basicPay, allowances, deductions);
        }
        else if (role == "developer")
        {
            employee = new Developer(name, id, basicPay, allowances, deductions);
        }
        else if (role == "intern")
        {
            employee = new Intern(name, id, basicPay, allowances, deductions);
        }
        else
        {
            employee = null;
        }


        if (employee != null)
        {
            employees.Add(employee);
            Console.WriteLine("Employee added successfully!");
        }
    }

    // Method to display all employees
    static void DisplayEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        Console.WriteLine("\nEmployee List:");
        foreach (var employee in employees)
        {
            Console.WriteLine(employee);
        }
    }

    // Method to calculate total payroll
    static void CalculateTotalPayroll()
    {
        double totalPayroll = 0;
        foreach (var employee in employees)
        {
            totalPayroll += employee.CalculateSalary();
        }
        Console.WriteLine($"Total Payroll: {totalPayroll:F2}");
    }

    // Method to save employees to a file
    static void SaveEmployees()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var emp in employees)
            {
                writer.WriteLine($"{emp.ID},{emp.Name},{emp.Role},{emp.BasicPay},{emp.Allowances},{emp.Deductions}");
            }
        }
    }

    // Method to load employees from a file
    static void LoadEmployees()
    {
        if (File.Exists(filePath))
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var data = line.Split(',');
                int id = int.Parse(data[0]);
                string name = data[1];
                string role = data[2];
                double basicPay = double.Parse(data[3]);
                double allowances = double.Parse(data[4]);
                double deductions = double.Parse(data[5]);

                BaseEmployee employee;

                if (role.ToLower() == "manager")
                {
                    employee = new Manager(name, id, basicPay, allowances, deductions);
                }
                else if (role.ToLower() == "developer")
                {
                    employee = new Developer(name, id, basicPay, allowances, deductions);
                }
                else if (role.ToLower() == "intern")
                {
                    employee = new Intern(name, id, basicPay, allowances, deductions);
                }
                else
                {
                    employee = null;
                }
                if (employee != null)
                {
                    employees.Add(employee); 
                }

            }
        }
    }
}
