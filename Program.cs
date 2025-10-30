using System;
using System.Collections.Generic;
using System.Linq;

// Структура для точки на площині
public readonly struct Point
{
    public double X { get; }
    public double Y { get; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";
}

// Клас, що представляє опуклий чотирикутник на площині
public class ConvexQuadrilateral
{
    // Закриті поля для збереження вершин
    private readonly Point[] _vertices; // гарантовано довжиною 4

    // Відкритий лише для читання доступ до вершин (інкапсуляція)
    public IReadOnlyList<Point> Vertices => Array.AsReadOnly(_vertices);

    // Конструктор приймає 4 вершини (у порядку обходу)
    public ConvexQuadrilateral(Point v1, Point v2, Point v3, Point v4)
    {
        _vertices = new[] { v1, v2, v3, v4 };

        if (!IsConvex())
        {
            throw new ArgumentException("Передані вершини не утворюють опуклий чотирикутник.");
        }
    }

    // Периметр як властивість лише для читання
    public double Perimeter => CalculatePerimeter();

    // Обчислення периметра
    private double CalculatePerimeter()
    {
        double sum = 0.0;
        for (var i = 0; i < 4; i++)
        {
            var a = _vertices[i];
            var b = _vertices[(i + 1) % 4];
            sum += Distance(a, b);
        }

        return sum;
    }

    // Статичний допоміжний метод для відстані між точками
    private static double Distance(Point a, Point b)
        => Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

    // Перевірка опуклості: знаки векторних добутків послідовних ребер однакові (без нульових)
    private bool IsConvex()
    {
        // Перевіримо, що ніякі три суміжні точки не колінеарні і що знак зберігається
        int sign = 0;
        for (var i = 0; i < 4; i++)
        {
            var a = _vertices[i];
            var b = _vertices[(i + 1) % 4];
            var c = _vertices[(i + 2) % 4];

            double cross = CrossProduct(a, b, c);
            if (Math.Abs(cross) < 1e-12)
            {
                // колінеарні три точки -> не підходить
                return false;
            }

            int currentSign = cross > 0 ? 1 : -1;
            if (sign == 0)
            {
                sign = currentSign;
            }
            else if (sign != currentSign)
            {
                return false; // зміна знаку -> не опуклий
            }
        }

        return true;
    }

    // Знак векторного (2D) добутку векторів AB і BC
    private static double CrossProduct(Point a, Point b, Point c)
    {
        double abx = b.X - a.X;
        double aby = b.Y - a.Y;
        double bcx = c.X - b.X;
        double bcy = c.Y - b.Y;
        return abx * bcy - aby * bcx;
    }

    public override string ToString()
    {
        return $"Вершини: {string.Join(", ", _vertices.Select(p => p.ToString()))}; Периметр = {Perimeter:F4}";
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введіть кількість чотирикутників n: ");
        if (!int.TryParse(Console.ReadLine(), out var n) || n <= 0)
        {
            Console.WriteLine("Некоректне значення n. Потрібно додатне ціле число.");
            return;
        }

        var quadrilaterals = new List<ConvexQuadrilateral>();

        for (var i = 0; i < n; i++)
        {
            Console.WriteLine($"\nЧотирикутник #{i + 1} - введіть координати 4 вершин в порядку обходу (по одній парі в рядку):");
            Console.WriteLine("Формат: x y (натисніть Enter після кожної пари). Для скасування введіть слово 'skip'");

            var points = new List<Point>();
            while (points.Count < 4)
            {
                Console.Write($"Введіть вершину #{points.Count + 1}: ");
                var line = Console.ReadLine();
                if (string.Equals(line, "skip", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                var parts = (line ?? string.Empty).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2 || !double.TryParse(parts[0], out var x) || !double.TryParse(parts[1], out var y))
                {
                    Console.WriteLine("Некоректний ввід. Будь ласка, введіть два числа через пробіл.");
                    continue;
                }

                points.Add(new Point(x, y));
            }

            if (points.Count != 4)
            {
                Console.WriteLine("Чотирикутник пропущено (не введено 4 вершини).\n");
                continue;
            }

            try
            {
                var quad = new ConvexQuadrilateral(points[0], points[1], points[2], points[3]);
                quadrilaterals.Add(quad);
                Console.WriteLine("Чотирикутник успішно додано.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Не вдалося створити опуклий чотирикутник: {ex.Message}");
                Console.WriteLine("Чотирикутник пропущено.\n");
            }
        }

        if (quadrilaterals.Count == 0)
        {
            Console.WriteLine("Не створено жодного опуклого чотирикутника.");
            return;
        }

        var maxPerimeterQuad = quadrilaterals.OrderByDescending(q => q.Perimeter).First();

        Console.WriteLine("\nЧотирикутник з найбільшим периметром:");
        Console.WriteLine(maxPerimeterQuad);
    }
}