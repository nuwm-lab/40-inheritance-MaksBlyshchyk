using System;

// Базовий клас для дробово-лінійної функції
// f(x) = (a1*x + a0) / (b1*x + b0)
public class FractionalLinearFunction
{
    // Захищена константа, доступна для нащадків
    protected const double Epsilon = 1e-9;

    // Властивості для коефіцієнтів. `protected set` дозволяє змінювати їх лише зсередини класу та нащадків.
    public double A1 { get; protected set; }
    public double A0 { get; protected set; }
    public double B1 { get; protected set; }
    public double B0 { get; protected set; }

    // Конструктор
    public FractionalLinearFunction(double a1, double a0, double b1, double b0)
    {
        A1 = a1;
        A0 = a0;
        B1 = b1;
        B0 = b0;
    }

    // Метод для знаходження значення в заданій точці
    public virtual double CalculateValue(double x)
    {
        double denominator = B1 * x + B0;
        if (Math.Abs(denominator) < Epsilon) // Використовуємо спільну константу
        {
            Console.Error.WriteLine($"Помилка: Ділення на нуль в точці x = {x}");
            return double.NaN;
        }
        return (A1 * x + A0) / denominator;
    }

    // Перевизначений метод для представлення об'єкта у вигляді рядка. Більш ідіоматичний підхід, ніж DisplayCoefficients().
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

    // Конструктор, що викликає конструктор базового класу
    public FractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0)
        : base(a1, a0, b1, b0)
    {
        A2 = a2;
        B2 = b2;
    }

    // Перевизначений метод для обчислення значення
    public override double CalculateValue(double x)
    {
        double numerator = A2 * x * x + A1 * x + A0;
        double denominator = B2 * x * x + B1 * x + B0;

        if (Math.Abs(denominator) < Epsilon) // Використовуємо ту саму константу з базового класу
        {
            Console.Error.WriteLine($"Помилка: Ділення на нуль в точці x = {x}");
            return double.NaN;
        }
        return numerator / denominator;
    }

    // Перевизначений метод ToString для повного представлення функції
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
        // --- Дробово-лінійна функція ---
        Console.WriteLine("--- Дробово-лінійна функція ---");
        var linearFunc = new FractionalLinearFunction(2.0, -4.0, 1.0, 2.0);
        Console.WriteLine(linearFunc); // Неявно викликається linearFunc.ToString()
        Console.WriteLine();

        // --- Дробова функція ---
        Console.WriteLine("--- Дробова функція ---");
        var fracFunc = new FractionalFunction(3.0, -1.0, 5.0, 1.0, 0.0, -9.0);
        Console.WriteLine(fracFunc); // Неявно викликається fracFunc.ToString()
        Console.WriteLine();

        // --- Обчислення значень в точці ---
        Console.Write("Введіть точку x0 для обчислення значень функцій: ");
        if (double.TryParse(Console.ReadLine(), out double x0))
        {
            // Обчислення для першої функції
            Console.WriteLine($"\nЗначення дробово-лінійної функції в точці {x0}:");
            double result1 = linearFunc.CalculateValue(x0);
            if (!double.IsNaN(result1))
            {
                Console.WriteLine($"f({x0}) = {result1:F4}"); // Форматуємо вивід до 4 знаків після коми
            }

            // Обчислення для другої функції
            Console.WriteLine($"\nЗначення дробової функції в точці {x0}:");
            double result2 = fracFunc.CalculateValue(x0);
            if (!double.IsNaN(result2))
            {
                Console.WriteLine($"f({x0}) = {result2:F4}");
            }
        }
        else
        {
            Console.WriteLine("Некоректне введення. Будь ласка, введіть число.");
        }
    }
}