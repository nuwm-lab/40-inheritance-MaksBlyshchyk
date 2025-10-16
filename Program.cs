using System;
using System.Globalization;

namespace FractionalFunctionsApp
{
    // Базовий клас — дробово-лінійна функція
    public class FractionalLinearFunction
    {
        protected double A1 { get; set; }
        protected double A0 { get; set; }
        protected double B1 { get; set; }
        protected double B0 { get; set; }

        protected const double Epsilon = 1e-12;

        public FractionalLinearFunction(double a1, double a0, double b1, double b0)
        {
            if (Math.Abs(b1) < Epsilon && Math.Abs(b0) < Epsilon)
                throw new ArgumentException("Знаменник не може бути тотожно нульовим.");

            A1 = a1;
            A0 = a0;
            B1 = b1;
            B0 = b0;
        }

        public virtual double CalculateValue(double x)
        {
            double denominator = B1 * x + B0;
            if (Math.Abs(denominator) < Epsilon)
                return double.NaN;

            return (A1 * x + A0) / denominator;
        }

        protected static string FormatSign(double value)
        {
            return value >= 0 ? $"+ {value}" : $"- {Math.Abs(value)}";
        }

        public override string ToString()
        {
            return $"f(x) = ({A1}x {FormatSign(A0)}) / ({B1}x {FormatSign(B0)})";
        }
    }

    // Квадратична дробова функція
    public class FractionalFunction : FractionalLinearFunction
    {
        protected double A2 { get; set; }
        protected double B2 { get; set; }

        public FractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0)
            : base(a1, a0, b1, b0)
        {
            if (Math.Abs(b2) < Epsilon && Math.Abs(b1) < Epsilon && Math.Abs(b0) < Epsilon)
                throw new ArgumentException("Знаменник не може бути тотожно нульовим.");

            A2 = a2;
            B2 = b2;
        }

        public override double CalculateValue(double x)
        {
            double denominator = B2 * x * x + B1 * x + B0;
            if (Math.Abs(denominator) < Epsilon)
                return double.NaN;

            double numerator = A2 * x * x + A1 * x + A0;
            return numerator / denominator;
        }

        public override string ToString()
        {
            return $"f(x) = ({A2}x² {FormatSign(A1)}x {FormatSign(A0)}) / ({B2}x² {FormatSign(B1)}x {FormatSign(B0)})";
        }
    }

    // Масштабована функція
    public class ScaledFractionalFunction : FractionalFunction
    {
        public double Scale { get; private set; }

        public ScaledFractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0, double scale)
            : base(a2, a1, a0, b2, b1, b0)
        {
            Scale = scale;
        }

        public override double CalculateValue(double x)
        {
            double baseValue = base.CalculateValue(x);
            return double.IsNaN(baseValue) ? double.NaN : baseValue * Scale;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, масштаб: {Scale}";
        }
    }

    // Головна програма
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Console.Write("Введіть x: ");
                string input = Console.ReadLine();

                if (!double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double x))
                {
                    Console.WriteLine("Некоректне введення. Введіть число.");
                    return;
                }

                var f1 = new FractionalLinearFunction(2, 1, 1, 3);
                var f2 = new FractionalFunction(1, -2, 3, 2, -1, 1);
                var f3 = new ScaledFractionalFunction(1, 2, 1, 1, -1, 1, 2);

                Console.WriteLine(f1);
                Console.WriteLine($"f1({x}) = {f1.CalculateValue(x)}");
                Console.WriteLine();

                Console.WriteLine(f2);
                Console.WriteLine($"f2({x}) = {f2.CalculateValue(x)}");
                Console.WriteLine();

                Console.WriteLine(f3);
                Console.WriteLine($"f3({x}) = {f3.CalculateValue(x)}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}

