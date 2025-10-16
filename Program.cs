using System;

namespace MathFunctions
{
    /// <summary>
    /// Базовий клас для дробово-лінійної функції.
    /// Представляє функцію вигляду: f(x) = (A1*x + A0) / (B1*x + B0).
    /// </summary>
    public class FractionalLinearFunction
    {
        // Властивості з автоматичною реалізацією. 'protected set' дозволяє
        // змінювати їх у похідних класах, але не ззовні.
        public double A1 { get; protected set; }
        public double A0 { get; protected set; }
        public double B1 { get; protected set; }
        public double B0 { get; protected set; }

        /// <summary>
        /// Мала величина для порівняння з нулем, щоб уникнути помилок з плаваючою комою.
        /// </summary>
        protected const double Epsilon = 1e-9;

        /// <summary>
        /// Ініціалізує новий екземпляр класу з заданими коефіцієнтами.
        /// </summary>
        /// <param name="a1">Коефіцієнт 'a1' у чисельнику.</param>
        /// <param name="a0">Коефіцієнт 'a0' у чисельнику.</param>
        /// <param name="b1">Коефіцієнт 'b1' у знаменнику.</param>
        /// <param name="b0">Коефіцієнт 'b0' у знаменнику.</param>
        public FractionalLinearFunction(double a1, double a0, double b1, double b0)
        {
            A1 = a1;
            A0 = a0;
            B1 = b1;
            B0 = b0;
        }

        /// <summary>
        /// Обчислює значення функції в заданій точці x.
        /// </summary>
        /// <param name="x">Точка, в якій обчислюється значення.</param>
        /// <returns>Результат обчислення або double.NaN, якщо знаменник дорівнює нулю.</returns>
        public virtual double CalculateValue(double x)
        {
            double denominator = B1 * x + B0;
            if (Math.Abs(denominator) < Epsilon)
            {
                Console.Error.WriteLine($"Помилка (лінійна функція): Ділення на нуль в точці x = {x}");
                return double.NaN;
            }
            return (A1 * x + A0) / denominator;
        }

        /// <summary>
        /// Повертає рядкове представлення функції.
        /// </summary>
        /// <returns>Рядок, що описує функцію та її коефіцієнти.</returns>
        public override string ToString()
        {
            return $"f(x) = ({A1}*x + {A0}) / ({B1}*x + {B0})";
        }
    }

    /// <summary>
    /// Похідний клас для дробової функції з квадратичними поліномами.
    /// Представляє функцію вигляду: f(x) = (A2*x^2 + A1*x + A0) / (B2*x^2 + B1*x + B0).
    /// </summary>
    public class FractionalFunction : FractionalLinearFunction
    {
        public double A2 { get; protected set; }
        public double B2 { get; protected set; }

        /// <summary>
        /// Ініціалізує новий екземпляр класу з заданими коефіцієнтами.
        /// </summary>
        public FractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0)
            : base(a1, a0, b1, b0) // Виклик конструктора базового класу
        {
            A2 = a2;
            B2 = b2;
        }

        /// <summary>
        /// Перевизначений метод для обчислення значення квадратичної дробової функції.
        /// </summary>
        public override double CalculateValue(double x)
        {
            double numerator = A2 * x * x + A1 * x + A0;
            double denominator = B2 * x * x + B1 * x + B0;

            // Використовуємо константу Epsilon з базового класу
            if (Math.Abs(denominator) < Epsilon)
            {
                Console.Error.WriteLine($"Помилка (квадратична функція): Ділення на нуль в точці x = {x}");
                return double.NaN;
            }
            return numerator / denominator;
        }

        /// <summary>
        /// Повертає рядкове представлення квадратичної дробової функції.
        /// </summary>
        public override string ToString()
        {
            return $"f(x) = ({A2}*x^2 + {A1}*x + {A0}) / ({B2}*x^2 + {B1}*x + {B0})";
        }
    }

    /// <summary>
    /// Похідний клас, що масштабує результат базової дробово-лінійної функції.
    /// Представляє функцію вигляду: f(x) = ScaleFactor * (A1*x + A0) / (B1*x + B0).
    /// </summary>
    public class ScaledFractionalFunction : FractionalLinearFunction
    {
        public double ScaleFactor { get; protected set; }

        /// <summary>
        /// Ініціалізує новий екземпляр масштабованої функції.
        /// </summary>
        public ScaledFractionalFunction(double a1, double a0, double b1, double b0, double scaleFactor)
            : base(a1, a0, b1, b0)
        {
            ScaleFactor = scaleFactor;
        }

        /// <summary>
        /// Перевизначений метод, що обчислює значення базової функції і множить його на коефіцієнт.
        /// </summary>
        public override double CalculateValue(double x)
        {
            // Викликаємо реалізацію базового класу, щоб уникнути дублювання коду
            double baseValue = base.CalculateValue(x);

            // Перевіряємо, чи не повернув базовий метод помилку
            if (double.IsNaN(baseValue))
            {
                return double.NaN;
            }

            return baseValue * ScaleFactor;
        }

        /// <summary>
        /// Повертає рядкове представлення масштабованої функції.
        /// </summary>
        public override string ToString()
        {
            return $"f(x) = {ScaleFactor} * [({A1}*x + {A0}) / ({B1}*x + {B0})]";
        }
    }

    /// <summary>
    /// Головний клас програми для демонстрації роботи з функціями.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- Демонстрація роботи з ієрархією класів функцій ---");

            // Створення об'єктів через конструктори
            var linearFunc = new FractionalLinearFunction(2.0, -4.0, 1.0, 2.0);
            var fracFunc = new FractionalFunction(3.0, -1.0, 5.0, 1.0, 0.0, -9.0);
            var scaledFunc = new ScaledFractionalFunction(1.0, 1.0, 1.0, -5.0, 10.0);

            // Виведення інформації про функції (неявно викликається метод ToString)
            Console.WriteLine("\n1. Дробово-лінійна функція:");
            Console.WriteLine(linearFunc);

            Console.WriteLine("\n2. Дробова квадратична функція:");
            Console.WriteLine(fracFunc);

            Console.WriteLine("\n3. Масштабована дробово-лінійна функція:");
            Console.WriteLine(scaledFunc);

            // --- Обчислення значень в точці ---
            Console.Write("\nВведіть точку x для обчислення значень функцій: ");
            string input = Console.ReadLine();
            if (double.TryParse(input, out double x0))
            {
                Console.WriteLine("---------------------------------------------");

                // Обчислення для першої функції
                double result1 = linearFunc.CalculateValue(x0);
                if (!double.IsNaN(result1))
                {
                    Console.WriteLine($"Значення лінійної функції в точці {x0}: {result1:F4}");
                }

                // Обчислення для другої функції
                double result2 = fracFunc.CalculateValue(x0);
                if (!double.IsNaN(result2))
                {
                    Console.WriteLine($"Значення квадратичної функції в точці {x0}: {result2:F4}");
                }

                // Обчислення для третьої функції
                double result3 = scaledFunc.CalculateValue(x0);
                if (!double.IsNaN(result3))
                {
                    Console.WriteLine($"Значення масштабованої функції в точці {x0}: {result3:F4}");
                }
            }
            else
            {
                Console.WriteLine("Некоректне введення. Було введено не число.");
            }
        }
    }
}]