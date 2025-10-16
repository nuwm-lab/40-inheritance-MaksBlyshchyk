using System;

namespace MathFunctions
{
    /// <summary>
    /// Базовий клас для дробово-лінійної функції:
    /// f(x) = (A1*x + A0) / (B1*x + B0)
    /// </summary>
    public class FractionalLinearFunction
    {
        public double A1 { get; protected set; }
        public double A0 { get; protected set; }
        public double B1 { get; protected set; }
        public double B0 { get; protected set; }

        /// <summary>Мала величина для порівняння з нулем.</summary>
        protected const double Epsilon = 1e-9;

        public FractionalLinearFunction(double a1, double a0, double b1, double b0)
        {
            SetCoefficients(a1, a0, b1, b0);
        }

        /// <summary>Встановлює коефіцієнти функції (віртуальний метод).</summary>
        public virtual void SetCoefficients(double a1, double a0, double b1, double b0)
        {
            A1 = a1;
            A0 = a0;
            B1 = b1;
            B0 = b0;
        }

        /// <summary>Обчислює значення функції у точці x.</summary>
        public virtual double CalculateValue(double x)
        {
            double denominator = B1 * x + B0;
            if (Math.Abs(denominator) < Epsilon)
                return double.NaN;

            return (A1 * x + A0) / denominator;
        }

        /// <summary>Форматує знак для гарного виводу.</summary>
        protected string FormatSign(double value) => value >= 0 ? "+" : "-";

        /// <summary>Охайне текстове представлення функції.</summary>
        public override string ToString()
        {
            return $"f(x) = ({A1}*x {FormatSign(A0)} {Math.Abs(A0)}) / ({B1}*x {FormatSign(B0)} {Math.Abs(B0)})";
        }
    }

    /// <summary>
    /// Квадратична дробова функція:
    /// f(x) = (A2*x² + A1*x + A0) / (B2*x² + B1*x + B0)
    /// </summary>
    public class FractionalFunction : FractionalLinearFunction
    {
        public double A2 { get; protected set; }
        public double B2 { get; protected set; }

        public FractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0)
            : base(a1, a0, b1, b0)
        {
            A2 = a2;
            B2 = b2;
        }

        public override double CalculateValue(double x)
        {
            double numerator = A2 * x * x + A1 * x + A0;
            double denominator = B2 * x * x + B1 * x + B0;

            if (Math.Abs(denominator) < Epsilon)
                return double.NaN;

            return numerator / denominator;
        }

        public override string ToString()
        {
            return $"f(x) = ({A2}*x² {FormatSign(A1)} {Math.Abs(A1)}*x {FormatSign(A0)} {Math.Abs(A0)}) / " +
                   $"({B2}*x² {FormatSign(B1)} {Math.Abs(B1)}*x {FormatSign(B0)} {Math.Abs(B0)})";
        }
    }

    /// <summary>
    /// Масштабована дробово-лінійна функція:
    /// f(x) = ScaleFactor * BaseFunction(x)
    /// </summary>
    public class ScaledFractionalFunction : FractionalLinearFunction
    {
        public double ScaleFactor { get; protected set; }

        public ScaledFractionalFunction(double a1, double a0, double b1, double b0, double scaleFactor)
            : base(a1, a0, b1, b0)
        {
            ScaleFactor = scaleFactor;
        }

        public override double CalculateValue(double x)
        {
            double baseValue = base.CalculateValue(x);
            if (double.IsNaN(baseValue))
                return double.NaN;

            return baseValue * ScaleFactor;
        }

        public override string ToString()
        {
            string baseStr = base.ToString().Replace("f(x) = ", "");
            return $"f(x) = {ScaleFactor} * [{baseStr}]";
        }
    }

    /// <summary>
    /// Головний клас програми.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- Тестування ієрархії класів функцій ---");

            // Створення об’єктів
            var linearFunc = new FractionalLinearFunction(2.0, -4.0, 1.0, 2.0);
            var fracFunc = new FractionalFunction(3.0, -1.0, 5.0, 1.0, 0.0, -9.0);
            var scaledFunc = new ScaledFractionalFunction(1.0, 1.0, 1.0, -5.0, 10.0);

            Console.WriteLine("\nЛінійна функція:");
            Console.WriteLine(linearFunc);

            Console.WriteLine("\nКвадратична функція:");
            Console.WriteLine(fracFunc);

            Console.WriteLine("\nМасштабована функція:");
            Console.WriteLine(scaledFunc);

            Console.Write("\nВведіть значення x: ");
            if (double.TryParse(Console.ReadLine(), out double x))
            {
                Console.WriteLine("\n--- Результати обчислень ---");

                double result1 = linearFunc.CalculateValue(x);
                Console.WriteLine(double.IsNaN(result1)
                    ? $"Помилка: знаменник лінійної функції = 0 при x = {x}."
                    : $"f₁({x}) = {result1:F4}");

                double result2 = fracFunc.CalculateValue(x);
                Console.WriteLine(double.IsNaN(result2)
                    ? $"Помилка: знаменник квадратичної функції = 0 при x = {x}."
                    : $"f₂({x}) = {result2:F4}");

                double result3 = scaledFunc.CalculateValue(x);
                Console.WriteLine(double.IsNaN(result3)
                    ? $"Помилка: знаменник масштабованої функції = 0 при x = {x}."
                    : $"f₃({x}) = {result3:F4}");
            }
            else
            {
                Console.WriteLine("Некоректне введення. Потрібно ввести число.");
            }
        }
    }
}
