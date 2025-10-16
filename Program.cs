using System;

// Базовий клас для дробово-лінійної функції
// f(x) = (a1*x + a0) / (b1*x + b0)
public class FractionalLinearFunction
{
    // Коефіцієнти доступні для наслідування
    protected double a1, a0;
    protected double b1, b0;

    // Конструктор
    public FractionalLinearFunction()
    {
        a1 = 0; a0 = 0;
        b1 = 0; b0 = 0;
    }

    // Метод для задання коефіцієнтів
    public virtual void SetCoefficients(double a1_val, double a0_val, double b1_val, double b0_val)
    {
        a1 = a1_val;
        a0 = a0_val;
        b1 = b1_val;
        b0 = b0_val;
    }

    // Метод для виведення коефіцієнтів на екран
    public virtual void DisplayCoefficients()
    {
        Console.WriteLine("Функція: (a1*x + a0) / (b1*x + b0)");
        Console.WriteLine($"Коефіцієнти чисельника: a1 = {a1}, a0 = {a0}");
        Console.WriteLine($"Коефіцієнти знаменника: b1 = {b1}, b0 = {b0}");
    }

    // Метод для знаходження значення в заданій точці
    public virtual double CalculateValue(double x)
    {
        double denominator = b1 * x + b0;
        if (Math.Abs(denominator) < 1e-9) // Перевірка на ділення на нуль
        {
            Console.Error.WriteLine($"Помилка: Ділення на нуль! Знаменник дорівнює нулю в точці x = {x}");
            return double.NaN; // Повертаємо "не число"
        }
        return (a1 * x + a0) / denominator;
    }
}

// Похідний клас для дробової функції
// f(x) = (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0)
public class FractionalFunction : FractionalLinearFunction
{
    private double a2;
    private double b2;

    // Конструктор
    public FractionalFunction() : base() // Виклик конструктора базового класу
    {
        a2 = 0;
        b2 = 0;
    }

    // Новий метод для задання всіх коефіцієнтів
    public void SetCoefficients(double a2_val, double a1_val, double a0_val, double b2_val, double b1_val, double b0_val)
    {
        // Встановлюємо спільні коефіцієнти через метод базового класу
        base.SetCoefficients(a1_val, a0_val, b1_val, b0_val);
        // Встановлюємо нові коефіцієнти
        a2 = a2_val;
        b2 = b2_val;
    }

    // Перевизначений метод для виведення коефіцієнтів
    public override void DisplayCoefficients()
    {
        Console.WriteLine("Функція: (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0)");
        Console.WriteLine($"Коефіцієнти чисельника: a2 = {a2}, a1 = {a1}, a0 = {a0}");
        Console.WriteLine($"Коефіцієнти знаменника: b2 = {b2}, b1 = {b1}, b0 = {b0}");
    }

    // Перевизначений метод для знаходження значення в точці
    public override double CalculateValue(double x)
    {
        double numerator = a2 * x * x + a1 * x + a0;
        double denominator = b2 * x * x + b1 * x + b0;

        if (Math.Abs(denominator) < 1e-9) // Перевірка на ділення на нуль
        {
            Console.Error.WriteLine($"Помилка: Ділення на нуль! Знаменник дорівнює нулю в точці x = {x}");
            return double.NaN;
        }
        return numerator / denominator;
    }
}

// Головний клас програми
class Program
{
    static void Main(string[] args)
    {
        // --- Створення та робота з об'єктом "дробово-лінійна функція" ---
        Console.WriteLine("--- Дробово-лінійна функція ---");
        FractionalLinearFunction linearFunc = new FractionalLinearFunction();
        linearFunc.SetCoefficients(2.0, -4.0, 1.0, 2.0); // f(x) = (2x - 4) / (x + 2)
        linearFunc.DisplayCoefficients();
        Console.WriteLine();

        // --- Створення та робота з об'єктом "дробова функція" ---
        Console.WriteLine("--- Дробова функція ---");
        FractionalFunction fracFunc = new FractionalFunction();
        fracFunc.SetCoefficients(3.0, -1.0, 5.0, 1.0, 0.0, -9.0); // f(x) = (3x^2 - x + 5) / (x^2 - 9)
        fracFunc.DisplayCoefficients();
        Console.WriteLine();

        // --- Обчислення значень в точці ---
        Console.Write("Введіть точку x0 для обчислення значень функцій: ");
        string input = Console.ReadLine();
        if (double.TryParse(input, out double x0))
        {
            // Обчислення для першої функції
            Console.WriteLine($"\nЗначення дробово-лінійної функції в точці {x0}:");
            double result1 = linearFunc.CalculateValue(x0);
            if (!double.IsNaN(result1))
            {
                Console.WriteLine($"f({x0}) = {result1}");
            }

            // Обчислення для другої функції
            Console.WriteLine($"\nЗначення дробової функції в точці {x0}:");
            double result2 = fracFunc.CalculateValue(x0);
            if (!double.IsNaN(result2))
            {
                Console.WriteLine($"f({x0}) = {result2}");
            }
        }
        else
        {
            Console.WriteLine("Некоректне введення. Будь ласка, введіть число.");
        }
    }
}