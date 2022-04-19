using ComputationalMathematicsLabs.Lab_2;
using System;
using System.Text;

namespace ComputationalMathematicsLabs.Lab_4
{
    public class SteepestDescentMethod
    {
        private readonly double _epsilon;
        private readonly Matrix _startX;
        private readonly int _length;
        private readonly double _alpha;

        private int _iter;

        public delegate double Function(double x1, double x2);
        private readonly Function _functionOne;
        private readonly Function _functionTwo;


        public SteepestDescentMethod(double epsilon, double[] startX, double alpha, Function functionOne, Function functionTwo)
        {
            if (epsilon <= 0 || epsilon >= 1 || alpha <= 0 || alpha > 1)
            {
                throw new ArgumentException("Неверные значения входных данных");
            }

            _epsilon = epsilon;
            _alpha = alpha;
            _startX = new Matrix(startX);
            _functionOne = functionOne;
            _functionTwo = functionTwo;
            string epsilonString = $"{epsilon:#,#.#############################}";
            _length = epsilonString.Split(',')[1].Length;
        }

        private Matrix SteepestDescent(Matrix x, Matrix oldX)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + "}", _iter)).Append(" ; ");

            double x1 = x[0, 0];
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", x1)).Append(" ; ");

            double x2 = x[1, 0];
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", x2)).Append(" ; ");

            double fx1 = _functionOne(x1, x2);
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", fx1)).Append(" ; ");

            double fx2 = _functionTwo(x1, x2);
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", fx2)).Append(" ; ");

            Matrix newX = new Matrix(new[] { x1 - _alpha * fx1, x2 - _alpha * fx2 });

            string delta = x == _startX ? "" : oldX.Subtraction(x).GetNorm().ToString("F" + (_length + 1));
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", delta)).Append(" ; ");

            Console.WriteLine(stringBuilder.ToString());
            if (_iter == 0 || oldX.Subtraction(x).GetNorm() > _epsilon)
            {
                _iter++;
                x = SteepestDescent(newX, x);
            }

            return x;
        }

        public Matrix StartComputational()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"{0," + (_length + 4) + "}", "n"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "x1"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "x2"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "Фx1"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "Фx2"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "Norma"));
            Console.WriteLine(sb.ToString());
            _iter = 0;
            return SteepestDescent(_startX, new Matrix(2, 1));
        }
    }
}
