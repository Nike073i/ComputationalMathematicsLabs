using ComputationalMathematicsLabs.Lab_2;
using ComputationalMathematicsLabs.Lab_4;
using System;

namespace ComputationalMathematicsLabs.Lab_6_2
{
    public class Laboratory6_2
    {
        private readonly Func<double, double, double> _func = (x, y) =>
        Math.Pow(x, 4) + Math.Pow(y, 4) + Math.Sqrt(2 + x * x + y * y) - 2d * x + 3d * y;
        private readonly Func<double, double, double> _der1xFunc = (x, y) => 4 * Math.Pow(x, 3) + x / (Math.Sqrt(2d + x * x + y * y)) - 2d;
        private readonly Func<double, double, double> _der1yFunc = (x, y) => 4 * Math.Pow(y, 3) + y / Math.Sqrt(2d + x * x + y * y) + 3d;
        private readonly Func<double, double, double> _der2xxFunc = (x, y) => 12 * Math.Pow(x, 2) - Math.Pow(x, 2) / Math.Pow(2d + x * x + y * y, 3d / 2d) + 1 / Math.Sqrt(2d + x * x + y * y);
        private readonly Func<double, double, double> _der2xyFunc = (x, y) => -x * Math.Pow(y, 2) / Math.Pow(2d + x * x + y * y, 3d / 2d);
        private readonly Func<double, double, double> _der2yyFunc = (x, y) => 12 * Math.Pow(y, 2) - Math.Pow(y, 2) / Math.Pow(2d + x * x + y * y, 3d / 2d) + 1 / Math.Sqrt(2d + x * x + y * y);

        private readonly string epsilonFormat = "{0:#,#.#############################}";
        public void Start()
        {
            SteepestDescentMethod();
            Console.WriteLine();

            NewtonMethod();
            Console.WriteLine();

            FletcherRivsMethod();
            Console.WriteLine();

            Console.ReadKey();
        }

        private void SteepestDescentMethod()
        {
            Console.WriteLine("Нахождение решения системы с помощью метода наискорейшего спуска");
            double epsilon = 0.000001;
            double alpha = 0.13;
            string epsilonString = string.Format(epsilonFormat, epsilon);
            int _length = epsilonString.Split(',')[1].Length;
            double[] startX = new[] { 0d, -1d };
            string stringInfo = "Точность - {0:F" + _length + "} | " + "x1 - {1} | x2 - {2} | альфа - {3}";
            Console.WriteLine(string.Format(stringInfo, epsilon, startX[0], startX[1], alpha));
            SteepestDescentMethod descentMethod = new SteepestDescentMethod(epsilon, startX, alpha,
                (x, y) => 4 * x * x * x + x / Math.Sqrt(2d + x * x + y * y) - 2d, (x, y) => 4 * y * y * y + y / Math.Sqrt(2d + x * x + y * y) + 3d);
            Matrix result = descentMethod.StartComputational();

            double resX1 = result[0, 0];
            double resX2 = result[1, 0];
            double func = _func(resX1, resX2);

            stringInfo = "Решение задачи - ({0:F" + _length + "}" + " ; " + "{1:F" + _length + "})" + " | значение - {2:F" + _length + "}";
            Console.WriteLine(string.Format(stringInfo, resX1, resX2, func));
        }

        private void NewtonMethod()
        {

            Console.WriteLine("Нахождение решения с помощью метода Ньютона");
            Console.WriteLine("Введите x1 (например: 1,0)");
            double.TryParse(Console.ReadLine(), out double startX1);
            Console.WriteLine("Введите x2 (например: 1,0)");
            double.TryParse(Console.ReadLine(), out double startX2);
            Console.WriteLine("Введите e1 (например: 0,000001)");
            double.TryParse(Console.ReadLine(), out double e1);
            if (e1 <= 0)
            {
                e1 = 0.000001;
            }
            Console.WriteLine("Введите e2 (например: 0,000001)");
            double.TryParse(Console.ReadLine(), out double e2);
            if (e2 <= 0)
            {
                e2 = 0.000001;
            }
            Console.WriteLine("Введите m (например: 10)");
            int.TryParse(Console.ReadLine(), out int m);
            if (m <= 0 || m > 200)
            {
                m = 10;
            }

            string epsilonString = string.Format(epsilonFormat, e1);
            int length = epsilonString.Split(',')[1].Length;
            string stringInfo = "x1 - {0:F" + length + "}" + " | " +
                "x2 - {1:F" + length + "}" + " | " +
                "e1 - {2:F" + length + "}" + " | " +
                "e2 - {3:F" + length + "}" + " | " +
                "m - {4}";
            Console.WriteLine("Используемые значения:");

            Console.WriteLine(stringInfo, startX1, startX2, e1, e2, m);

            double[] startX = new[] { startX1, startX2 };
            NewtonMethod newton = new NewtonMethod(startX, e1, e2, m, _func, _der1xFunc, _der1yFunc, _der2xxFunc, _der2xyFunc, _der2yyFunc);
            Matrix result = newton.FindSolution();

            double resX1 = result[0, 0];
            double resX2 = result[1, 0];
            double func = _func(resX1, resX2);

            stringInfo = "Решение задачи - ({0:F" + length + "}" + " ; " + "{1:F" + length + "})" + " | значение - {2:F" + length + "}";
            Console.WriteLine(string.Format(stringInfo, resX1, resX2, func));
        }

        private void FletcherRivsMethod()
        {
            Console.WriteLine("Нахождение решения с помощью метода Флетчера-Ривса");
            Console.WriteLine("Введите x1 (например: 1,0)");
            double.TryParse(Console.ReadLine(), out double startX1);
            Console.WriteLine("Введите x2 (например: 1,0)");
            double.TryParse(Console.ReadLine(), out double startX2);
            Console.WriteLine("Введите e1 (например: 0,000001)");
            double.TryParse(Console.ReadLine(), out double e1);
            if (e1 <= 0)
            {
                e1 = 0.000001;
            }
            Console.WriteLine("Введите e2 (например: 0,000001)");
            double.TryParse(Console.ReadLine(), out double e2);
            if (e2 <= 0)
            {
                e2 = 0.000001;
            }
            Console.WriteLine("Введите m (например: 10)");
            int.TryParse(Console.ReadLine(), out int m);
            if (m <= 0 || m > 200)
            {
                m = 10;
            }

            double[] startX = new[] { startX1, startX2 };

            string epsilonString = string.Format(epsilonFormat, e1);
            int length = epsilonString.Split(',')[1].Length;
            string stringInfo = "x1 - {0:F" + length + "}" + " | " +
                "x2 - {1:F" + length + "}" + " | " +
                "e1 - {2:F" + length + "}" + " | " +
                "e2 - {3:F" + length + "}" + " | " +
                "m - {4}";
            Console.WriteLine("Используемые значения:");

            Console.WriteLine(stringInfo, startX1, startX2, e1, e2, m);
            Console.WriteLine();

            FletcherRivsMethod fletcherRivs = new FletcherRivsMethod(startX, e1, e2, m, _func, _der1xFunc, _der1yFunc);
            Matrix result = fletcherRivs.FindSolution();

            double resX1 = result[0, 0];
            double resX2 = result[1, 0];
            double func = _func(resX1, resX2);

            stringInfo = "Решение задачи - ({0:F" + length + "}" + " ; " + "{1:F" + length + "})" + " | значение - {2:F" + length + "}";

            Console.WriteLine(string.Format(stringInfo, resX1, resX2, func));
        }
    }
}

//double[] startX = new[] { 0d, -1d };
//double e1 = 0.000001;
//double e2 = 0.000001;
//int m = 200;
//Console.WriteLine(stringInfo, startX[0], startX[1], e1, e2, m);

