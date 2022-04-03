using ComputationalMathematicsLabs.Lab_3;
using System;

namespace ComputationalMathematicsLabs.Lab_5_2
{
    public class Laboratory5_2
    {
        private readonly Func<decimal, decimal> _func = (x) => x * x + 3 * Convert.ToDecimal(Math.Exp(Convert.ToDouble(-0.45m * x)));
        private readonly Func<decimal, decimal> _der1Func = (x) => 2 * x - 1.35m * Convert.ToDecimal(Math.Exp(Convert.ToDouble(-0.45m * x)));
        private readonly Func<decimal, decimal> _der2Func = (x) => 2 + 0.6075m * Convert.ToDecimal(Math.Exp(Convert.ToDouble(-0.45m * x)));
        private readonly string epsilonFormat = "{0:#,#.#############################}";
        private Interval _startInterval;

        public void Start()
        {
            ConstantStepNewtonMethod();
            Console.WriteLine();

            SvennMethod();
            Console.WriteLine();

            UniformSearch(_startInterval);
            Console.WriteLine();

            NewtonMethod();
            Console.WriteLine();

            HalfDivisionMethod(_startInterval);
            Console.WriteLine();

            Console.ReadKey();
        }

        private void ConstantStepNewtonMethod()
        {
            Console.WriteLine("Нахождение нуля производной функции с помощью разностного метода Ньютона с постоянным шагом");
            double epsilon = 0.000001;
            double startX = 1d;
            double step = 0.001;
            string epsilonString = string.Format(epsilonFormat, epsilon);
            int _length = epsilonString.Split(',')[1].Length;
            string stringInfo = "Точность - {0:F" + _length + "} | " + "х0 - {1} | шаг - {2}";
            Console.WriteLine(string.Format(stringInfo, epsilon, startX, step));
            СonstantStepNewtonMethod newtonMethod = new СonstantStepNewtonMethod(epsilon, startX, step, (x) => 2 * x - 1.35 * Math.Exp(-0.45 * x));
            newtonMethod.StartComputational();
        }

        private void SvennMethod()
        {
            Console.WriteLine("Нахождение интервала решения с помощью метода Свенна");
            Console.WriteLine("Введите x0 (например: 1,0)");
            decimal.TryParse(Console.ReadLine(), out decimal startX0);
            Console.WriteLine("Введите h (например: 1,0)");
            decimal.TryParse(Console.ReadLine(), out decimal h);
            if (h <= 0)
            {
                h = 1m;
            }
            Console.WriteLine("Используемые значения:");
            Console.WriteLine("х0 - {0} | h - {1}", startX0, h);

            SvennMethod svennMethod = new SvennMethod(startX0, h, _func);
            bool result = svennMethod.FindInterval();
            if (!result)
            {
                Console.WriteLine("Ошибка вычислений. Возможно функция не является унимодальной, требуемый интервал не может быть найден");
                Console.WriteLine("Попробуйте другие значения");
                SvennMethod();
            }
            else
            {
                _startInterval = svennMethod.SearchInterval;
                Console.WriteLine("Полученный интервал:");
                Console.WriteLine("a0 = {0} | b0 = {1}", _startInterval.A, _startInterval.B);
                Console.WriteLine();
            }
        }

        private void UniformSearch(Interval startInterval)
        {
            Console.WriteLine("Нахождение решения с помощью метода равномерного поиска");
            Console.WriteLine("Введите N (например: 9)");
            int.TryParse(Console.ReadLine(), out int n);
            if (n <= 0)
            {
                n = 9;
            }
            Console.WriteLine("Используемые значения:");
            Console.WriteLine("a0 - {0} | b0 - {1} | n - {2}", startInterval.A, startInterval.B, n);

            UniformSearch uniformSearch = new UniformSearch(startInterval, n, _func);
            uniformSearch.FindInterval();
        }

        private void NewtonMethod()
        {
            Console.WriteLine("Нахождение решения с помощью метода Ньютона");
            Console.WriteLine("Введите x0 (например: 1,0)");
            decimal.TryParse(Console.ReadLine(), out decimal startX0);
            Console.WriteLine("Введите l (например: 0,000001)");
            decimal.TryParse(Console.ReadLine(), out decimal l);
            if (l <= 0)
            {
                l = 0.000001m;
            }

            string epsilonString = string.Format("{0:#,#.#############################}", l);
            int length = epsilonString.Split(',')[1].Length;
            string stringInfo = "x0 - {0} | l - {1:F" + length + "}";
            Console.WriteLine("Используемые значения:");

            Console.WriteLine(stringInfo, startX0, l);

            NewtonMethod newtonMethod = new NewtonMethod(startX0, l, _der1Func, _der2Func);
            decimal result = newtonMethod.FindSolution();

            epsilonString = string.Format("{0:#,#.#############################}", l);
            length = epsilonString.Split(',')[1].Length;

            stringInfo = "Решение задачи находится в точке - {0:F" + length + "} " + "| значение - {1:F" + length + "}";
            Console.WriteLine(string.Format(stringInfo, result, _func(result)));
        }

        private void HalfDivisionMethod(Interval startInterval)
        {
            Console.WriteLine("Нахождение решения с помощью метода половинного деления поиска");
            Console.WriteLine("Введите l (например: 0,000001)");
            decimal.TryParse(Console.ReadLine(), out decimal l);
            if (l <= 0)
            {
                l = 0.000001m;
            }

            string epsilonString = string.Format("{0:#,#.#############################}", l);
            int _length = epsilonString.Split(',')[1].Length;

            string stringInfo = "a0 - {0} | b0 - {1} | l - {2:F" + _length + "}";
            Console.WriteLine("Используемые значения:");
            Console.WriteLine(stringInfo, startInterval.A, startInterval.B, l);

            HalfDivisionMethod halfDivisionMethod = new HalfDivisionMethod(startInterval, l, _func);
            decimal result = halfDivisionMethod.FindSolution();

            stringInfo = "Решение задачи находится в точке - {0:F" + _length + "} " + "| значение - {1:F" + _length + "}";
            Console.WriteLine(string.Format(stringInfo, result, _func(result)));
        }
    }
}