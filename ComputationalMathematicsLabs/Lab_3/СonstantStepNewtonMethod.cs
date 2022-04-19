using System;
using System.Text;

namespace ComputationalMathematicsLabs.Lab_3
{
    public class СonstantStepNewtonMethod
    {
        private readonly double _epsilon;
        private readonly double _startX;
        private readonly double _step;
        private readonly int _length;
        private int _iter;

        public delegate double Function(double x);
        private readonly Function myFuction;

        public СonstantStepNewtonMethod(double epsilon, double startX, double step, Function function)
        {
            if (epsilon <= 0 || epsilon >= 1 || step <= 0) throw new ArgumentException("Неверные значения входных данных");
            _epsilon = epsilon;
            _startX = startX;
            _step = step;
            myFuction = function;
            string epsilonString = string.Format("{0:#,#.#############################}", epsilon);
            _length = epsilonString.Split(',')[1].Length;
        }

        private double StepMethod(double x, double oldX)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + "}", _iter)).Append(" ; ");

            var result = x;
            stringBuilder.Append(result.ToString("F" + (_length + 1)) + " ; ");

            double fxn = myFuction(x);
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", fxn)).Append(" ; ");

            double denominator = fxn * _step;
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", denominator)).Append(" ; ");

            double xn_h = x + _step;
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", xn_h)).Append(" ; ");

            double fxn_h = myFuction(xn_h);
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", fxn_h)).Append(" ; ");

            double divider = fxn_h - fxn;
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", divider)).Append(" ; ");

            double newX = x - (denominator / divider);
            string delta = x == _startX ? "" : Math.Abs(oldX - x).ToString("F" + (_length + 1));
            stringBuilder.Append(string.Format(@"{0," + (_length + 4) + ":F" + (_length + 1) + "}", delta)).Append(" ; ");

            Console.WriteLine(stringBuilder.ToString());
            if (Math.Abs(oldX - x) > _epsilon)
            {
                _iter++;
                result = StepMethod(newX, x);
            }

            return result;
        }

        private double StepMethodWithout(double x, double oldX)
        {
            var result = x;

            double fxn = myFuction(x);
            double denominator = fxn * _step;
            double xn_h = x + _step;
            double fxn_h = myFuction(xn_h);
            double divider = fxn_h - fxn;
            double newX = x - (denominator / divider);
            if (Math.Abs(oldX - x) > _epsilon)
            {
                _iter++;
                result = StepMethodWithout(newX, x);
            }

            return result;
        }

        public void StartComputational()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"{0," + (_length + 4) + "}", "n"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "xn"));
            sb.Append(string.Format(@"{0," + (_length + 7) + "}", "f(xn)"));
            sb.Append(string.Format(@"{0," + (_length + 7) + "}", "f(xn)*h"));
            sb.Append(string.Format(@"{0," + (_length + 7) + "}", "xn+h"));
            sb.Append(string.Format(@"{0," + (_length + 7) + "}", "f(xn+h)"));
            sb.Append(string.Format(@"{0," + (_length + 9) + "}", "f(xn+h)-f(xn)"));
            sb.Append(string.Format(@"{0," + (_length + 6) + "}", "Delta"));
            Console.WriteLine(sb.ToString());
            _iter = 0;
            StepMethod(_startX, 0);
        }

        public double Calc()
        {
            _iter = 0;
            return StepMethodWithout(_startX, 0);
        }
 
    }
}
