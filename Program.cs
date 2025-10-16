using System;

// Базовий клас для дробово-лінійної функції
// f(x) = (a1*x + a0) / (b1*x + b0)
public class FractionalLinearFunction
{
    // Константа для перевірки ділення на нуль
    protected const double Epsilon = 1e-9;

    // Приватні поля для кращої інкапсуляції
    private double _a1, _a0, _b1, _b0;

    // Публічні властивості для доступу до коефіцієнтів
    public double A1 { get; protected set; }
    public double A0 { get; protected set; }
    public double B1 { get; protected set; }
    public double B0 { get; protected set; }

    // Конструктор за замовчуванням
    public FractionalLinearFunction() { }

    // Конструктор для ініціалізації
    public FractionalLinearFunction(double a1, double a0, double b1, double b0)
    {
        SetCoefficients(a1, a0, b1, b0);
    }

    // Метод для задання коефіцієнтів
    public virtual void SetCoefficients(double a1_val, double a0_val, double b1_val, double b0_val)
    {
        A1 = a1_val;
        A0 = a0_val;
        B1 = b1_val;
        B0 = b0_val;
    }

    // Метод для знаходження значення в заданій точці
    public virtual double CalculateValue(double x)
    {
        double denominator = B1 * x + B0;
        if (Math.Abs(denominator) < Epsilon) // Використання константи
        {
            Console.Error.WriteLine($"Помилка: Ділення на нуль! Знаменник дорівнює нулю в точці x = {x}");
            return double.NaN;
        }
        return (A1 * x + A0) / denominator;
    }

    // Перевизначений метод для представлення об'єкта у вигляді рядка
    public override string ToString()
    {
        return $"Функція: f(x) = ({A1}*x + {A0}) / ({B1}*x + {B0})";
    }
}

// Похідний клас для дробової функції
// f(x) = (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0)
public class FractionalFunction : FractionalLinearFunction
{
    // Додаткові властивості
    public double A2 { get; protected set; }
    public double B2 { get; protected set; }

    // Конструктори
    public FractionalFunction() : base() { }

    public FractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0)
        : base(a1, a0, b1, b0) // Виклик конструктора базового класу
    {
        A2 = a2;
        B2 = b2;
    }

    // Новий метод для задання всіх коефіцієнтів
    public void SetCoefficients(double a2_val, double a1_val, double a0_val, double b2_val, double b1_val, double b0_val)
    {
        base.SetCoefficients(a1_val, a0_val, b1_val, b0_val); // Встановлення спільних коефіцієнтів
        A2 = a2_val;
        B2 = b2_val;
    }

    // Перевизначений метод для знаходження значення в точці
    public override double CalculateValue(double x)
    {
        double numerator = A2 * x * x + A1 * x + A0;
        double denominator = B2 * x * x + B1 * x + B0;

        if (Math.Abs(denominator) < Epsilon) // Використання константи
        {
            Console.Error.WriteLine($"Помилка: Ділення на нуль! Знаменник дорівнює нулю в точці x = {x}");
            return double.NaN;
        }
        return numerator / denominator;
    }

    // Перевизначений метод ToString
    public override string ToString()
    {
        return $"Функція: f(x) = ({A2}*x^2 + {A1}*x + {A0}) / ({B2}*x^2 + {B1}*x + {B0})";
    }
}

// Головний клас програми
class Program
{
    static void Main(string[] args)
    {
        // --- Створення та робота з об'єктом "дробово-лінійна функція" ---
        Console.WriteLine("--- Дробово-лінійна функція ---");
        var linearFunc = new FractionalLinearFunction(2.0, -4.0, 1.0, 2.0); // f(x) = (2x - 4) / (x + 2)
        Console.WriteLine(linearFunc); // Виклик ToString()
        Console.WriteLine();

        // --- Створення та робота з об'єктом "дробова функція" ---
        Console.WriteLine("--- Дробова функція ---");
        var fracFunc = new FractionalFunction(3.0, -1.0, 5.0, 1.0, 0.0, -9.0); // f(x) = (3x^2 - x + 5) / (x^2 - 9)
        Console.WriteLine(fracFunc); // Виклик ToString()
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
                Console.WriteLine($"f({x0}) = {result1:F4}"); // Форматування результату
            }

            // Обчислення для другої функції
            Console.WriteLine($"\nЗначення дробової функції в точці {x0}:");
            double result2 = fracFunc.CalculateValue(x0);
            if (!double.IsNaN(result2))
            {
                Console.WriteLine($"f({x0}) = {result2:F4}"); // Форматування результату
            }
        }
        else
        {
            Console.WriteLine("Некоректне введення. Будь ласка, введіть число.");
        }
    }
}
