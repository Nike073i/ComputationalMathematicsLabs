using System;
using System.Text;

namespace ComputationalMathematicsLabs.Lab_5_2
{
    internal class NewtonMethod
    {
        private readonly decimal _startX;
        private readonly decimal _epsilon;
        private readonly Func<decimal, decimal> _der1Func;
        private readonly Func<decimal, decimal> _der2Func;
        private readonly int _length;
        public NewtonMethod(decimal startX, decimal epsilon, Func<decimal, decimal> der1Func, Func<decimal, decimal> der2Func)
        {
            if (epsilon <= 0 || der1Func == null || der2Func == null)
            {
                throw new ArgumentException("Неверные значения входных данных");
            }
            _startX = startX;
            _epsilon = epsilon;
            _der1Func = der1Func;
            _der2Func = der2Func;
            string epsilonString = string.Format("{0:#,#.#############################}", epsilon);
            _length = epsilonString.Split(',')[1].Length;
        }

        public decimal FindSolution()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"{0," + _length + "}", "n"));
            sb.Append(string.Format(@"{0," + (_length + 1) + "}", "xn"));
            sb.Append(string.Format(@"{0," + (_length + 10) + "}", "f'(xn)"));
            sb.Append(string.Format(@"{0," + (_length + 10) + "}", "f''(xn)"));
            Console.WriteLine(sb.ToString());

            int n = 0;
            decimal curX = _startX;
            decimal newX = FindNewPoint(curX, n++);
            while (Math.Abs(_der1Func(curX)) > _epsilon)
            {
                curX = newX;
                newX = FindNewPoint(curX, n++);
            }
            return newX;
        }

        private decimal FindNewPoint(decimal curX, int n)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format(@"{0," + _length + "}", n)).Append(" ; ");
            stringBuilder.Append(curX.ToString("F" + (_length + 1)) + " ; ");

            decimal der1Val = _der1Func(curX);
            stringBuilder.Append(string.Format(@"{0," + (_length + 7) + ":F" + (_length + 1) + "}", der1Val)).Append(" ; ");
            decimal der2Val = _der2Func(curX);
            stringBuilder.Append(string.Format(@"{0," + (_length + 7) + ":F" + (_length + 1) + "}", der2Val)).Append(" ; ");
            Console.WriteLine(stringBuilder.ToString());

            return curX - (der1Val / der2Val);
        }
    }
}
