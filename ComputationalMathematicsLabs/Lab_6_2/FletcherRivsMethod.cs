using ComputationalMathematicsLabs.Lab_2;
using ComputationalMathematicsLabs.Lab_3;
using System;
using System.Text;

namespace ComputationalMathematicsLabs.Lab_6_2
{
    public class FletcherRivsMethod
    {
        private readonly Matrix _startX;
        private readonly double _epsilon1;
        private readonly double _epsilon2;
        private readonly int _m;

        private readonly Func<double, double, double> _func;
        private readonly Func<double, double, double> _der1xFunc;
        private readonly Func<double, double, double> _der1yFunc;
        private readonly int _length;
        private Matrix _oldD;
        private bool _isFinished = false;

        public FletcherRivsMethod(double[] startX, double epsilon1, double epsilon2, int m,
            Func<double, double, double> func,
            Func<double, double, double> der1xFunc,
            Func<double, double, double> der1yFunc)
        {
            if (epsilon1 <= 0 || epsilon2 <= 0 || m < 1 || func == null || der1xFunc == null || der1yFunc == null)
            {
                throw new ArgumentException("Неверные значения входных данных");
            }
            _startX = new Matrix(startX);
            _epsilon1 = epsilon1;
            _epsilon2 = epsilon2;
            _m = m;
            _func = func;
            _der1xFunc = der1xFunc;
            _der1yFunc = der1yFunc;

            string epsilonString = string.Format("{0:#,#.#############################}", epsilon1);
            int length1 = epsilonString.Split(',')[1].Length;
            epsilonString = string.Format("{0:#,#.#############################}", epsilon2);
            int length2 = epsilonString.Split(',')[1].Length;
            _length = length1 > length2 ? length1 : length2;
        }

        public Matrix FindSolution()
        {
            _isFinished = false;
            Console.WriteLine("Начинаем поиск");
            int k = 0;
            Matrix curX = _startX;
            Matrix oldX = null;
            Matrix newX = FindNewPoints(oldX, curX, k++);
            Console.WriteLine("Проверяем условия окончания: ");
            string stringInfo = "||xk+1 - xk|| < e2 && |f(xk+1) - f(xk)| < e2";
            string stringXCond = "||x{0} - x{1}|| = {2," + (_length + 7) + ":F" + (_length + 1) + "} < {3," + (_length + 7) + ":F" + (_length) + "}";
            string stringFCond = "|f{0} - f{1}| = {2," + (_length + 7) + ":F" + (_length + 1) + "} < {3," + (_length + 7) + ":F" + (_length) + "}";

            double xNorm = Math.Abs(newX.Subtraction(curX).GetEuclidNorm());
            double fDelta = Math.Abs(_func(newX[0, 0], newX[1, 0]) - _func(curX[0, 0], curX[1, 0]));

            Console.WriteLine(stringInfo);
            Console.WriteLine(stringXCond, k, k - 1, xNorm, _epsilon2);
            Console.WriteLine(stringFCond, k, k - 1, fDelta, _epsilon2);
            Console.WriteLine();


            while ((xNorm >= _epsilon2 || fDelta >= _epsilon2) && !_isFinished)
            {
                Console.WriteLine("Условия окончания не выполнено. Продолжаем итерации");
                Console.WriteLine();

                oldX = curX;
                curX = newX;
                newX = FindNewPoints(oldX, curX, k++);

                xNorm = Math.Abs(newX.Subtraction(curX).GetEuclidNorm());
                fDelta = Math.Abs(_func(newX[0, 0], newX[1, 0]) - _func(curX[0, 0], curX[1, 0]));
                if (!_isFinished)
                {
                    Console.WriteLine("Проверяем условия окончания: ");
                    Console.WriteLine(stringInfo);
                    Console.WriteLine(stringXCond, k, k - 1, xNorm, _epsilon2);
                    Console.WriteLine(stringFCond, k, k - 1, fDelta, _epsilon2);
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Итерационный процесс закончен.");
            Console.WriteLine();
            return newX;
        }

        private Matrix FindNewPoints(Matrix oldX, Matrix curX, int k)
        {
            Console.WriteLine("Новая итерация:");
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"{0," + _length + "}", "k"));
            sb.Append(string.Format(@"{0," + (_length + 7) + "}", "x1"));
            sb.Append(string.Format(@"{0," + (_length + 7) + "}", "x2"));
            sb.Append(string.Format(@"{0," + (_length + 10) + "}", "f'(x1)"));
            sb.Append(string.Format(@"{0," + (_length + 10) + "}", "f'(x2)"));

            Console.WriteLine(sb.ToString());
            sb.Clear();

            double x1 = curX[0, 0];
            double x2 = curX[1, 0];

            double fx1 = _der1xFunc(x1, x2);
            double fx2 = _der1yFunc(x1, x2);

            sb.Append(string.Format(@"{0," + _length + "}", k)).Append(" ; ");
            sb.Append(string.Format(@"{0," + (_length + 7) + ":F" + (_length + 1) + "}", x1)).Append(" ; ");
            sb.Append(string.Format(@"{0," + (_length + 7) + ":F" + (_length + 1) + "}", x2)).Append(" ; ");
            sb.Append(string.Format(@"{0," + (_length + 7) + ":F" + (_length + 1) + "}", fx1)).Append(" ; ");
            sb.Append(string.Format(@"{0," + (_length + 7) + ":F" + (_length + 1) + "}", fx2)).Append(" ; ");

            Console.WriteLine(sb.ToString());
            sb.Clear();
            Console.WriteLine();

            Matrix grad = new Matrix(new double[] { fx1, fx2 });
            double gradNorm = grad.GetEuclidNorm();

            string stringInfo = "Норма градиента : {0:F" + (_length + 1) + "}";
            Console.WriteLine(string.Format(stringInfo, gradNorm));
            Console.WriteLine();

            if (gradNorm < _epsilon1)
            {
                Console.WriteLine("Норма градиента меньше eps1, вычисления закончены");
                _isFinished = true;
                return curX;
            }

            if (k >= _m)
            {
                Console.WriteLine("Итерация достигла максимально разрешимого числа, вычисления закончены");
                _isFinished = true;
                return curX;
            }

            Matrix d;
            if (k == 0)
            {
                d = grad.Multiply(-1);
                _oldD = d;
            }
            else
            {
                double oldX1 = oldX[0, 0];
                double oldX2 = oldX[1, 0];

                double oldFx1 = _der1xFunc(oldX1, oldX2);
                double oldFx2 = _der1yFunc(oldX1, oldX2);
                Matrix gradOld = new Matrix(new double[] { oldFx1, oldFx2 });
                double gradOldNorm = gradOld.GetEuclidNorm();
                double oldB = Math.Pow(gradNorm, 2) / Math.Pow(gradOldNorm, 2);

                stringInfo = "B{0} = {1," + (_length + 7) + ":F" + (_length + 1) + "}";
                Console.WriteLine(stringInfo, k, oldB-1);

                d = grad.Multiply(-1).Additional(_oldD.Multiply(oldB));
                _oldD = d;
                Console.WriteLine();
            }

            stringInfo = "d{0} = ({1," + (_length + 7) + ":F" + (_length + 1) + "}" + " ; " + "{2," + (_length + 7) + ":F" + (_length + 1) + "})";
            Console.WriteLine(string.Format(stringInfo, k, d[0, 0], d[1, 0]));
            Console.WriteLine();

            СonstantStepNewtonMethod newtonMethod = new СonstantStepNewtonMethod(_epsilon1, 1, 0.001, (t) => 4d * Math.Pow((x1 - fx1 * t), 3) * (-fx1)
                + 4d * Math.Pow((x2 - fx2 * t), 3) * (-fx2)
                + (2d * (x1 - fx1 * t) * (-fx1) + 2d * (x2 - fx2 * t) * (-fx2))
                / (2 * Math.Sqrt(2 + Math.Pow(x1 - fx1 * t, 2) + Math.Pow(x2 - fx2 * t, 2))) + 2d * fx1 - 3d * fx2);

            double tk = newtonMethod.Calc();

            stringInfo = "t{0} = {1," + (_length + 7) + ":F" + (_length + 1) + "}";
            Console.WriteLine(stringInfo, k, tk);
            Console.WriteLine();

            Matrix newX = curX.Additional(d.Multiply(tk));

            Console.WriteLine("Новый х:");
            stringInfo = "x{0} = ({1," + (_length + 7) + ":F" + (_length + 1) + "}" + " ; " + "{2," + (_length + 7) + ":F" + (_length + 1) + "})";
            Console.WriteLine(stringInfo, k + 1, newX[0, 0], newX[1, 0]);
            Console.WriteLine();

            return newX;
        }
    }
}
