// Директиви using розташовані нагорі файлу для правильної структури.
using System;

// Весь код огорнуто в простір імен для кращої організації та уникнення конфліктів імен.
namespace MathFunctions
{
    /// <summary>
    /// Представляє дробово-лінійну функцію вигляду f(x) = (a1*x + a0) / (b1*x + b0).
    /// </summary>
    public class FractionalLinearFunction
    {
        // Використовуються автоматичні властивості з 'protected set',
        // що дозволяє змінювати їх у похідних класах, але не ззовні.
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
        public FractionalLinearFunction(double a1, double a0, double b1, double b0)
        {
            SetCoefficients(a1, a0, b1, b0);
        }

        /// <summary>
        /// Встановлює коефіцієнти функції. Метод є віртуальним, щоб його можна було розширити.
        /// </summary>
        public virtual void SetCoefficients(double a1, double a0, double b1, double b0)
        {
            A1 = a1;
            A0 = a0;
            B1 = b1;
            B0 = b0;
        }

        /// <summary>
        /// Обчислює значення функції в заданій точці.
        /// </summary>
        /// <returns>Результат обчислення або double.NaN, якщо знаменник дорівнює нулю.</returns>
        public virtual double CalculateValue(double x)
        {
            double denominator = B1 * x + B0;
            if (Math.Abs(denominator) < Epsilon)
            {
                // Повертаємо NaN (Not a Number) як ознаку помилки обчислення.
                return double.NaN;
            }
            return (A1 * x + A0) / denominator;
        }

        /// <summary>
        /// Повертає рядкове представлення функції.
        /// Це стандартний підхід для виведення інформації про об'єкт.
        /// </summary>
        public override string ToString()
        {
            return $"Дробово-лінійна функція: f(x) = ({A1}x + {A0}) / ({B1}x + {B0})";
        }
    }

    /// <summary>
    /// Похідний клас для дробової функції з квадратичними поліномами.
    /// f(x) = (A2*x^2 + A1*x + A0) / (B2*x^2 + B1*x + B0).
    /// </summary>
    public class FractionalFunction : FractionalLinearFunction
    {
        public double A2 { get; protected set; }
        public double B2 { get; protected set; }

        /// <summary>
        /// Ініціалізує новий екземпляр, викликаючи конструктор базового класу для спільних коефіцієнтів.
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

            // Використовується константа Epsilon з базового класу для консистентності.
            if (Math.Abs(denominator) < Epsilon)
            {
                return double.NaN;
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

    /// <summary>
    /// Похідний клас, що масштабує результат базової функції: f(x) = ScaleFactor * BaseFunction(x).
    /// </summary>
    public class ScaledFractionalFunction : FractionalLinearFunction
    {
        public double ScaleFactor { get; protected set; }

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
            // Викликаємо реалізацію базового класу, щоб уникнути дублювання коду.
            double baseValue = base.CalculateValue(x);

            // Якщо базовий метод повернув помилку, передаємо її далі.
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
            string baseString = base.ToString().Replace("f(x) = ", "");
            return $"f(x) = {ScaleFactor} * [{baseString}]";
        }
    }

    /// <summary>
    /// Головний клас програми, що містить точку входу (метод Main).
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Сценарій для демонстрації та тестування роботи класів.
            Console.WriteLine("--- Тестування ієрархії класів функцій ---");

            // Створення об'єктів різних типів функцій.
            var linearFunc = new FractionalLinearFunction(2.0, -4.0, 1.0, 2.0);
            var fracFunc = new FractionalFunction(3.0, -1.0, 5.0, 1.0, 0.0, -9.0);
            var scaledFunc = new ScaledFractionalFunction(1.0, 1.0, 1.0, -5.0, 10.0);

            // Виведення інформації про створені функції (неявно викликається метод ToString).
            Console.WriteLine("\nСтворено дробово-лінійну функцію:");
            Console.WriteLine(linearFunc);

            Console.WriteLine("\nСтворено дробову квадратичну функцію:");
            Console.WriteLine(fracFunc);

            Console.WriteLine("\nСтворено масштабовану функцію:");
            Console.WriteLine(scaledFunc);

            // Отримання вводу від користувача для обчислення.
            Console.Write("\nВведіть точку x для обчислення значень: ");
            if (double.TryParse(Console.ReadLine(), out double x))
            {
                Console.WriteLine("---------------------------------------------");

                // Обчислення та виведення результату для лінійної функції.
                double result1 = linearFunc.CalculateValue(x);
                if (double.IsNaN(result1))
                {
                    Console.WriteLine($"Помилка: для лінійної функції в точці x={x} знаменник дорівнює нулю.");
                }
                else
                {
                    Console.WriteLine($"Результат лінійної функції в точці {x}: {result1:F4}");
                }

                // Обчислення та виведення результату для квадратичної функції.
                double result2 = fracFunc.CalculateValue(x);
                if (double.IsNaN(result2))
                {
                    Console.WriteLine($"Помилка: для квадратичної функції в точці x={x} знаменник дорівнює нулю.");
                }
                else
                {
                    Console.WriteLine($"Результат квадратичної функції в точці {x}: {result2:F4}");
                }
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Некоректне введення. Потрібно було ввести число.");
            }
        }
    }
}