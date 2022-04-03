using System;

namespace ComputationalMathematicsLabs.Lab_5_2
{
    public class UniformSearch
    {
        private readonly int _n;
        private readonly Interval _interval;
        private readonly Func<decimal, decimal> _func;
        public UniformSearch(Interval interval, int n, Func<decimal, decimal> func)
        {
            if (n <= 0 || func == null)
            {
                throw new ArgumentException("Неверные значения входных данных");
            }
            _interval = interval;
            _n = n;
            _func = func;
        }

        public Interval FindInterval()
        {
            Console.WriteLine("Вычисляем точки, равностоящие друг от друга");
            decimal[] xPoints = new decimal[_n];
            decimal[] xValues = new decimal[_n];
            Console.WriteLine("xi = a0 + i (b0-a0)/(N+1)\n");
            decimal a0 = _interval.A;
            decimal b0 = _interval.B;
            decimal distance = b0 - a0;
            decimal intervalPoints = distance / (_n + 1);
            for (int i = 1; i <= xPoints.Length; i++)
            {
                decimal point = _interval.A + i * intervalPoints;
                Console.WriteLine("x{0} = {1} * {2} * {3}/{4} = {5}", i, a0, i, distance, _n + 1, point);
                xPoints[i - 1] = point;
                decimal pointVal = _func(point);
                Console.WriteLine("Значение в точке x{0} = {1:F6}\n", i, pointVal);
                xValues[i - 1] = pointVal;
            }
            int indexMin = 0;
            decimal valueMin = xValues[indexMin];
            for (int i = 1; i < xValues.Length; i++)
            {
                if (xValues[i] < valueMin)
                {
                    indexMin = i;
                    valueMin = xValues[i];
                }
            }
            Console.WriteLine("Находим точку с минимальным значением функции:");
            Console.WriteLine("f(x{0}) = {1:F6}", indexMin + 1, valueMin);

            return DetermineInterval(xPoints, indexMin);
        }

        private Interval DetermineInterval(decimal[] xPoints, int indexMin)
        {
            string stringInfo = "Точка минимума принадлежит [{0};{1}]";
            string solutionInfo = "Приближенное решение в точке: {0}";
            Interval result;
            decimal solution;
            if (indexMin == 0)
            {
                result = new Interval(_interval.A, xPoints[1]);
                Console.WriteLine(stringInfo, _interval.A, xPoints[1]);
                solution = (_interval.A + xPoints[1]) / 2;
                Console.WriteLine(solutionInfo, solution);
                return result;
            }
            if (indexMin == xPoints.Length - 1)
            {
                result = new Interval(xPoints[xPoints.Length - 2], _interval.B);
                Console.WriteLine(stringInfo, xPoints[xPoints.Length - 2], _interval.B);
                solution = (xPoints[xPoints.Length - 2] + _interval.B) / 2;
                Console.WriteLine(solutionInfo, solution);
                return result;
            }
            result = new Interval(xPoints[indexMin - 1], xPoints[indexMin + 1]);
            Console.WriteLine(stringInfo, xPoints[indexMin - 1], xPoints[indexMin + 1]);
            solution = (xPoints[indexMin - 1] + xPoints[indexMin + 1]) / 2;
            Console.WriteLine(solutionInfo, solution);
            return result;
        }
    }
}
