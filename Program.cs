using System;

namespace MathFunctions
{
    // 1. Опис класу "Дробово-лінійна функція"
    // Вигляд: (a1*x + a0) / (b1*x + b0)
    class LinearFractionalFunction
    {
        // Поля для зберігання коефіцієнтів (protected, щоб їх бачив клас-нащадок)
        protected double a1, a0;
        protected double b1, b0;

        // Метод завдання коефіцієнтів
        public virtual void SetCoefficients(double a1, double a0, double b1, double b0)
        {
            this.a1 = a1;
            this.a0 = a0;
            this.b1 = b1;
            this.b0 = b0;
        }

        // Метод виведення коефіцієнтів на екран
        public virtual void Display()
        {
            Console.WriteLine($"Функція: ({a1}x + {a0}) / ({b1}x + {b0})");
        }

        // Знаходження значення в заданій точці x0
        public virtual double Calculate(double x)
        {
            double numerator = a1 * x + a0;
            double denominator = b1 * x + b0;

            if (denominator == 0)
            {
                Console.WriteLine("Помилка: Ділення на нуль!");
                return 0;
            }

            return numerator / denominator;
        }
    }

    // 2. Опис класу "Дробова функція" (спадкується від попереднього)
    // Вигляд: (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0)
    class RationalFunction : LinearFractionalFunction
    {
        // Додаємо нові коефіцієнти для x^2
        private double a2;
        private double b2;

        // Перевантажуємо метод завдання коефіцієнтів (додаємо a2 та b2)
        public void SetCoefficients(double a2, double a1, double a0, double b2, double b1, double b0)
        {
            // Викликаємо базовий метод для спільних коефіцієнтів
            base.SetCoefficients(a1, a0, b1, b0);
            this.a2 = a2;
            this.b2 = b2;
        }

        // Перевизначаємо метод виведення (override)
        public override void Display()
        {
            Console.WriteLine($"Функція: ({a2}x^2 + {a1}x + {a0}) / ({b2}x^2 + {b1}x + {b0})");
        }

        // Перевизначаємо обчислення значення (override)
        public override double Calculate(double x)
        {
            double numerator = a2 * Math.Pow(x, 2) + a1 * x + a0;
            double denominator = b2 * Math.Pow(x, 2) + b1 * x + b0;

            if (denominator == 0)
            {
                Console.WriteLine("Помилка: Ділення на нуль!");
                return 0;
            }

            return numerator / denominator;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Щоб коректно відображалась кирилиця

            // --- Робота з дробово-лінійною функцією ---
            Console.WriteLine("--- Дробово-лінійна функція ---");
            LinearFractionalFunction linearFunc = new LinearFractionalFunction();

            // Задаємо коефіцієнти: (2x + 5) / (x + 1)
            linearFunc.SetCoefficients(2, 5, 1, 1);
            linearFunc.Display();

            Console.Write("Введіть x0 для першої функції: ");
            double x1 = Convert.ToDouble(Console.ReadLine());
            double res1 = linearFunc.Calculate(x1);
            Console.WriteLine($"Значення y({x1}) = {res1:F3}");
            Console.WriteLine();


            // --- Робота з дробовою (квадратичною) функцією ---
            Console.WriteLine("--- Дробова функція (квадратична) ---");
            RationalFunction rationalFunc = new RationalFunction();

            // Задаємо коефіцієнти: (1x^2 + 2x + 3) / (1x^2 + 4x + 4)
            rationalFunc.SetCoefficients(1, 2, 3, 1, 4, 4);
            rationalFunc.Display();

            Console.Write("Введіть x0 для другої функції: ");
            double x2 = Convert.ToDouble(Console.ReadLine());
            double res2 = rationalFunc.Calculate(x2);
            Console.WriteLine($"Значення y({x2}) = {res2:F3}");

            Console.ReadKey();
        }
    }
}