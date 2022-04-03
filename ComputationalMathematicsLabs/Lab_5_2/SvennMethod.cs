using System;

namespace ComputationalMathematicsLabs.Lab_5_2
{
    public struct Interval
    {
        public decimal A { get; set; }
        public decimal B { get; set; }
        public Interval(decimal A, decimal B)
        {
            this.A = A;
            this.B = B;
        }
    }
    public class SvennMethod
    {
        private enum CalculatelResult
        {
            IntervalFound,
            IsNotUnimodal
        }

        private enum TerminationCondition
        {
            Finished,
            Impossible,
            RecalculationRequired
        }

        private readonly decimal _startX;
        private readonly decimal _step;
        private readonly Func<decimal, decimal> _func;
        private Interval _searchInterval;
        public Interval SearchInterval
        {
            get => _searchInterval;
            private set => _searchInterval = value;
        }
        public SvennMethod(decimal startX, decimal step, Func<decimal, decimal> func)
        {
            if (step <= 0 || func == null)
            {
                throw new ArgumentException("Неверные значения входных данных");
            }

            _startX = startX;
            _step = step;
            _func = func;
        }

        public bool FindInterval()
        {
            CalculatelResult result = CalculateInterval(_startX, _step);
            if (result == CalculatelResult.IntervalFound)
            {
                return true;
            }

            return false;
        }

        private CalculatelResult CalculateInterval(decimal x0, decimal step)
        {
            decimal lX = x0 - _step;
            decimal rX = x0 + _step;
            decimal lVal = _func(lX);
            decimal x0Val = _func(x0);
            decimal rVal = _func(rX);
            TerminationCondition conditionResult = CheckCondition(lVal, x0Val, rVal);
            switch (conditionResult)
            {
                case TerminationCondition.Finished:
                    _searchInterval.A = lX;
                    _searchInterval.B = rX;
                    return CalculatelResult.IntervalFound;
                case TerminationCondition.Impossible:
                    return CalculatelResult.IsNotUnimodal;
                case TerminationCondition.RecalculationRequired:
                    Console.WriteLine("Определяем d");
                    decimal delta = default;
                    decimal x1 = default;
                    string stringFormat = string.Empty;
                    string stringBorder = string.Empty;
                    if (lVal >= x0Val && x0Val >= rVal)
                    {
                        delta = step;
                        _searchInterval.A = x0;
                        x1 = rX;
                        stringBorder = "a0";
                    }
                    else if (lVal <= x0Val && x0Val <= rVal)
                    {
                        delta = -step;
                        _searchInterval.B = x0;
                        x1 = lX;
                        stringBorder = "b0";
                    }
                    Console.WriteLine("d = {0} | " + stringBorder + " = {1} | x1 = {2} | k = 1", delta, x0, x1);
                    if (FindNextPoint(x1, 1, delta))
                    {
                        return CalculatelResult.IntervalFound;
                    }
                    break;
            }
            return CalculatelResult.IsNotUnimodal;
        }

        private bool FindNextPoint(decimal curX, int k, decimal delta)
        {
            if (k > 100)
            {
                return false;
            }

            Console.WriteLine("Находим следующую точку: xk+1 = xk + 2^k * d");

            decimal newX = curX + Convert.ToDecimal(Math.Pow(2, k)) * delta;

            Console.WriteLine("x{0} = {1} + 2^{2} * {3} = {4}", k + 1, curX, k, delta, newX);
            decimal newXVal = _func(newX);
            decimal curXVal = _func(curX);
            Console.WriteLine("Проверяем условие убывания функции: f(xk+1) < (fk)");
            Console.WriteLine("{0:F6} < {1:F6}", newXVal, curXVal);
            if (newXVal < curXVal)
            {
                Console.WriteLine("Условие убывания выполняется");
                if (delta == _step)
                {
                    _searchInterval.A = curX;
                    Console.WriteLine("d = {0} | a0 = {1}", delta, curX);
                }

                if (delta == -_step)
                {
                    _searchInterval.B = curX;
                    Console.WriteLine("d = {0} | b0 = {1}", delta, curX);
                }
                FindNextPoint(newX, ++k, delta);
            }
            else
            {
                Console.WriteLine("Условие не выполнено. Процедура завершается");
                if (delta == _step)
                {
                    _searchInterval.B = newX;
                    Console.WriteLine("d = {0} | b0 = {1}", delta, newX);
                }

                if (delta == -_step)
                {
                    _searchInterval.A = newX;
                    Console.WriteLine("d = {0} | a0 = {1}", delta, newX);
                }
            }
            return true;
        }

        private TerminationCondition CheckCondition(decimal lVal, decimal x0Val, decimal rVal)
        {
            Console.WriteLine("Проверка условий : f(x0-h) >= f(x0) <= f(x0+h)");
            Console.WriteLine("f(x0-h) - {0:F6} | f(x0) - {1:F6} | f(x0+h) - {2:F6}", lVal, x0Val, rVal);
            if (lVal >= x0Val && x0Val <= rVal)
            {
                Console.WriteLine("Условия выполнены, интервал найден.");
                return TerminationCondition.Finished;
            }

            if (lVal <= x0Val && x0Val >= rVal)
            {
                Console.WriteLine("Функция не является унимодальной.");
                return TerminationCondition.Impossible;
            }

            Console.WriteLine("Условие не выполняется, продолжаем вычисление.");
            return TerminationCondition.RecalculationRequired;
        }
    }
}
