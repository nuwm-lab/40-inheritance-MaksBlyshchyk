using MathFunctions;
using System;

// 1. Усі класи обгорнуто в простір імен.
namespace MathFunctions
{
    /// <summary>
    /// Представляє дробово-лінійну функцію вигляду f(x) = (a1*x + a0) / (b1*x + b0).
    /// </summary>
    public class FractionalLinearFunction
    {
        // 6. "Магічне число" винесено в константу.
        private const double Epsilon = 1e-9;

        // 3. Поля зроблено приватними для кращої інкапсуляції.
        private double _a1, _a0, _b1, _b0;

        // 3. Доступ до коефіцієнтів реалізовано через властивості.
        // `get` є публічним, `protected set` дозволяє змінювати значення в дочірніх класах.
        public double A1 { get; protected set; }
        public double A0 { get; protected set; }
        public double B1 { get; protected set; }
        public double B0 { get; protected set; }

        /// <summary>
        /// Ініціалізує новий екземпляр класу з нульовими коефіцієнтами.
        /// </summary>
        public FractionalLinearFunction()
        {
            A1 = 0; A0 = 0;
            B1 = 0; B0 = 0;
        }

        /// <summary>
        /// Встановлює коефіцієнти для функції.
        /// </summary>
        /// <param name="a1Value">Коефіцієнт 'a1' у чисельнику.</param>
        /// <param name="a0Value">Коефіцієнт 'a0' у чисельнику.</param>
        /// <param name="b1Value">Коефіцієнт 'b1' у знаменнику.</param>
        /// <param name="b0Value">Коефіцієнт 'b0' у знаменнику.</param>
        public virtual void SetCoefficients(double a1Value, double a0Value, double b1Value, double b0Value)
        {
            A1 = a1Value;
            A0 = a0Value;
            B1 = b1Value;
            B0 = b0Value;
        }

        /// <summary>
        /// Обчислює значення функції в заданій точці.
        /// </summary>
        /// <param name="x">Точка, в якій обчислюється значення.</param>
        /// <returns>Результат обчислення функції.</returns>
        /// <exception cref="DivideByZeroException">Виникає, якщо знаменник дорівнює нулю.</exception>
        public virtual double CalculateValue(double x)
        {
            double denominator = B1 * x + B0;
            if (Math.Abs(denominator) < Epsilon)
            {
                // 5. Замість виводу в консоль генерується виняток.
                throw new DivideByZeroException($"Ділення на нуль: знаменник дорівнює нулю в точці x = {x}");
            }
            return (A1 * x + A0) / denominator;
        }

        /// <summary>
        /// Повертає рядкове представлення функції.
        /// </summary>
        /// <returns>Рядок, що описує функцію та її коефіцієнти.</returns>
        public override string ToString()
        {
            return $"Дробово-лінійна функція: f(x) = ({A1}x + {A0}) / ({B1}x + {B0})";
        }
    }

    /// <summary>
    /// Представляє дробову функцію вигляду f(x) = (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0).
    /// </summary>
    public class FractionalFunction : FractionalLinearFunction
    {
        public double A2 { get; protected set; }
        public double B2 { get; protected set; }

        /// <summary>
        /// Встановлює коефіцієнти для дробової функції.
        /// </summary>
        public void SetCoefficients(double a2Value, double a1Value, double a0Value, double b2Value, double b1Value, double b0Value)
        {
            base.SetCoefficients(a1Value, a0Value, b1Value, b0Value);
            A2 = a2Value;
            B2 = b2Value;
        }

        /// <inheritdoc/>
        public override double CalculateValue(double x)
        {
            double numerator = A2 * x * x + A1 * x + A0;
            double denominator = B2 * x * x + B1 * x + B0;

            if (Math.Abs(denominator) < 1e-9)
            {
                throw new DivideByZeroException($"Ділення на нуль: знаменник дорівнює нулю в точці x = {x}");
            }
            return numerator / denominator;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Дробова функція: f(x) = ({A2}x^2 + {A1}x + {A0}) / ({B2}x^2 + {B1}x + {B0})";
        }
    }

    /// <summary>
    /// (Додаткове завдання) Представляє масштабовану дробово-лінійну функцію.
    /// f(x) = scale * (a1*x + a0) / (b1*x + b0)
    /// </summary>
    public class ScaledFractionalFunction : FractionalLinearFunction
    {
        /// <summary>
        /// Коефіцієнт масштабування.
        /// </summary>
        public double Scale { get; set; }

        /// <summary>
        /// Ініціалізує новий екземпляр з коефіцієнтом масштабування 1.0.
        /// </summary>
        public ScaledFractionalFunction(double scale = 1.0)
        {
            Scale = scale;
        }

        /// <inheritdoc/>
        public override double CalculateValue(double x)
        {
            // Викликаємо реалізацію базового класу та множимо результат на коефіцієнт.
            return base.CalculateValue(x) * Scale;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Масштабована функція: f(x) = {Scale} * [({A1}x + {A0}) / ({B1}x + {B0})]";
        }
    }
}

// Головний клас програми
class Program
{
    static void Main(string[] args)
    {
        // Використовуємо класи з нашого простору імен.
        using MathFunctions;

        // --- Дробово-лінійна функція ---
        Console.WriteLine("--- Дробово-лінійна функція ---");
        var linearFunc = new FractionalLinearFunction();
        linearFunc.SetCoefficients(2.0, -4.0, 1.0, 2.0);
        Console.WriteLine(linearFunc); // Використовуємо ToString()
        Console.WriteLine();

        // --- Дробова функція ---
        Console.WriteLine("--- Дробова функція ---");
        FractionalFunction fracFunc = new FractionalFunction();

        fracFunc.SetCoefficients(3.0, -1.0, 5.0, 1.0, 0.0, -9.0); // f(x) = (3x^2 - x + 5) / (x^2 - 9)

        fracFunc.SetCoefficients(3.0, -1.0, 5.0, 1.0, 0.0, -9.0); //  f(x) = (3x^2 - x + 5) / (x^2 - 9)

        fracFunc.DisplayCoefficients();
        var fracFunc = new FractionalFunction();
        fracFunc.SetCoefficients(3.0, -1.0, 5.0, 1.0, 0.0, -9.0);
        Console.WriteLine(fracFunc);
        Console.WriteLine();

        // --- (Додаткове завдання) Масштабована функція ---
        Console.WriteLine("--- Масштабована дробово-лінійна функція ---");
        var scaledFunc = new ScaledFractionalFunction(10.0); // Масштаб 10
        scaledFunc.SetCoefficients(1.0, 1.0, 1.0, 0); // f(x) = 10 * (x+1)/x
        Console.WriteLine(scaledFunc);
        Console.WriteLine();

        // --- Обчислення значень в точці ---
        Console.Write("Введіть точку x0 для обчислення значень функцій: ");
        string input = Console.ReadLine();
        if (double.TryParse(input, out double x0))
        {
            // Обробляємо обчислення в блоці try-catch
            try
            {
                Console.WriteLine($"\nЗначення дробово-лінійної функції в точці {x0}:");
                double result1 = linearFunc.CalculateValue(x0);
                Console.WriteLine($"f({x0}) = {result1}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                Console.WriteLine($"\nЗначення дробової функції в точці {x0}:");
                double result2 = fracFunc.CalculateValue(x0);
                Console.WriteLine($"f({x0}) = {result2}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                Console.WriteLine($"\nЗначення масштабованої функції в точці {x0}:");
                double result3 = scaledFunc.CalculateValue(x0);
                Console.WriteLine($"f({x0}) = {result3}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Некоректне введення. Будь ласка, введіть число.");
        }
    }
}