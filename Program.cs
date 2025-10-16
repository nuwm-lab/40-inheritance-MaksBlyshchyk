using System;

// Базовий клас для дробово-лінійної функції
// f(x) = (A1*x + A0) / (B1*x + B0)
public class FractionalLinearFunction
{
    // Використовуємо властивості з приватними полями для кращої інкапсуляції.
    // Доступ protected дозволяє похідним класам їх змінювати.
    public double A1 { get; protected set; }
    public double A0 { get; protected set; }
    public double B1 { get; protected set; }
    public double B0 { get; protected set; }

    // Створюємо константу для перевірки на нуль, щоб уникнути "магічних чисел".
    protected const double Epsilon = 1e-9;

    // Конструктор для ініціалізації коефіцієнтів при створенні об'єкта.
    // Це кращий підхід, ніж окремий метод SetCoefficients.
    public FractionalLinearFunction(double a1, double a0, double b1, double b0)
    {
        A1 = a1;
        A0 = a0;
        B1 = b1;
        B0 = b0;
    }

    // Метод для знаходження значення в заданій точці.
    public virtual double CalculateValue(double x)
    {
        double denominator = B1 * x + B0;
        // Перевірка на ділення на нуль з використанням константи Epsilon.
        if (Math.Abs(denominator) < Epsilon)
        {
            Console.Error.WriteLine($"Помилка: Ділення на нуль! Знаменник дорівнює нулю в точці x = {x}");
            return double.NaN; // Повертаємо "не число"
        }
        return (A1 * x + A0) / denominator;
    }

    // Перевизначаємо метод ToString() для зручного виведення інформації про об'єкт.
    // Це більш ідіоматичний підхід у C#, ніж кастомний метод DisplayCoefficients().
    public override string ToString()
    {
        return $"Функція: f(x) = ({A1}*x + {A0}) / ({B1}*x + {B0})";
    }
}

// Похідний клас для дробової функції
// f(x) = (A2*x^2 + A1*x + A0) / (B2*x^2 + B1*x + B0)
public class FractionalFunction : FractionalLinearFunction
{
    // Додаємо нову властивість, специфічну для цього класу.
    public double A2 { get; protected set; }
    public double B2 { get; protected set; }

    // Конструктор похідного класу, що викликає конструктор базового класу.
    public FractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0)
        : base(a1, a0, b1, b0) // Передаємо коефіцієнти до базового класу
    {
        // Ініціалізуємо власні коефіцієнти.
        A2 = a2;
        B2 = b2;
    }

    // Перевизначений метод для знаходження значення в точці.
    public override double CalculateValue(double x)
    {
        double numerator = A2 * x * x + A1 * x + A0;
        double denominator = B2 * x * x + B1 * x + B0;

        // Використовуємо Epsilon, успадкований від базового класу.
        if (Math.Abs(denominator) < Epsilon)
        {
            Console.Error.WriteLine($"Помилка: Ділення на нуль! Знаменник дорівнює нулю в точці x = {x}");
            return double.NaN;
        }
        return numerator / denominator;
    }

    // Перевизначаємо ToString() для відображення повної інформації.
    public override string ToString()
    {
        return $"Функція: f(x) = ({A2}*x^2 + {A1}*x + {A0}) / ({B2}*x^2 + {B1}*x + {B0})";
    }
}

// Головний клас програми
public class Program
{
    public static void Main(string[] args)
    {
        // --- Створення та робота з об'єктом "дробово-лінійна функція" ---
        Console.WriteLine("--- Дробово-лінійна функція ---");
        // Ініціалізація через конструктор
        FractionalLinearFunction linearFunc = new FractionalLinearFunction(2.0, -4.0, 1.0, 2.0);
        // Неявно викликається linearFunc.ToString()
        Console.WriteLine(linearFunc);
        Console.WriteLine();

        // --- Створення та робота з об'єктом "дробова функція" ---
        Console.WriteLine("--- Дробова функція ---");
        // Ініціалізація через конструктор, без дублювання змінних
        FractionalFunction fracFunc = new FractionalFunction(3.0, -1.0, 5.0, 1.0, 0.0, -9.0);
        // Неявно викликається fracFunc.ToString()
        Console.WriteLine(fracFunc);
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
                Console.WriteLine($"f({x0}) = {result1:F4}"); // Форматуємо вивід до 4 знаків
            }

            // Обчислення для другої функції
            Console.WriteLine($"\nЗначення дробової функції в точці {x0}:");
            double result2 = fracFunc.CalculateValue(x0);
            if (!double.IsNaN(result2))
            {
                Console.WriteLine($"f({x0}) = {result2:F4}"); // Форматуємо вивід до 4 знаків
            }
        }
        else
        {
            Console.WriteLine("Некоректне введення. Будь ласка, введіть число.");
        }
    }
}
