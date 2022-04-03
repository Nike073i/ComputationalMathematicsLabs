using System;
using System.Text;

namespace ComputationalMathematicsLabs.Lab_5_2
{
    public class HalfDivisionMethod
    {
        private readonly Interval _startInterval;
        private readonly decimal _epsilon;
        private readonly Func<decimal, decimal> _func;
        private readonly int _length;

        public HalfDivisionMethod(Interval startInterval, decimal epsilon, Func<decimal, decimal> func)
        {
            if (epsilon <= 0 || func == null)
            {
                throw new ArgumentException("Неверные значения входных данных");
            }
            _startInterval = startInterval;
            _epsilon = epsilon;
            _func = func;
            string epsilonString = string.Format("{0:#,#.#############################}", epsilon);
            _length = epsilonString.Split(',')[1].Length;
        }

        public decimal FindSolution()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"{0,2}", "k"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "l2k"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "ak"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "bk"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "xk"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "yk"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "zk"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "f(xk)"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "f(yk)"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "f(zk)"));
            Console.WriteLine(sb.ToString());

            int k = 0;
            decimal curA = _startInterval.A;
            decimal curB = _startInterval.B;
            decimal curX = (curA + curB) / 2;
            decimal curDistance = (curB - curA);
            return FindNewPoint(curA, curB, curX, curDistance, k);
        }

        private decimal FindNewPoint(decimal curA, decimal curB, decimal curX, decimal curDistance, int k)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("{0,2}", k)).Append(" ;");
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curDistance)).Append(" ;");
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curA)).Append(" ;");
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curB)).Append(" ;");
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curX)).Append(" ;");


            decimal curY = curA + curDistance / 4;
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curY)).Append(" ;");

            decimal curZ = curB - curDistance / 4;
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curZ)).Append(" ;");

            decimal curXVal = _func(curX);
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curXVal)).Append(" ;");

            decimal curYVal = _func(curY);
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curYVal)).Append(" ;");

            decimal curZVal = _func(curZ);
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", curZVal)).Append(" ;");

            decimal newA;
            decimal newX;
            decimal newB;
            if (curXVal > curYVal)
            {
                newB = curX;
                newA = curA;
                newX = curY;
            }
            else
            {
                if (curXVal > curZVal)
                {
                    newB = curB;
                    newA = curX;
                    newX = curZ;
                }
                else
                {
                    newA = curY;
                    newB = curZ;
                    newX = curX;
                }
            }
            Console.WriteLine(stringBuilder.ToString());
            decimal newDistance = Math.Abs(newB - newA);
            if (newDistance > _epsilon)
            {
                curX = FindNewPoint(newA, newB, newX, newDistance, ++k);
            }
            else
            {
                Console.WriteLine(("|L{0}| = {1:F" + _length + "}" + " <= {2:F" + _length + "}"), 2 * (k + 1), newDistance, _epsilon);
                Console.WriteLine("Процесс поиска завершается");
            }
            return curX;
        }
    }
}
