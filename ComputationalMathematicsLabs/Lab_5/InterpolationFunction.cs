using System;
using System.Text;

namespace ComputationalMathematicsLabs.Lab_5
{
    public class InterpolationFunction
    {
        private readonly double[] _xValues;
        private readonly double[] _yValues;
        private readonly double _x0;
        private readonly int _countSignAfter = 3;
        public delegate double Function(double x);
        public delegate double FunctionBasisPolinomial(double x, int index);

        public InterpolationFunction(double[] xValues, double[] yValues, double x0)
        {
            if (xValues is null || yValues is null
                || !xValues.Length.Equals(yValues.Length)) throw new ArgumentException("Введены неверные значения");
            _xValues = xValues;
            _yValues = yValues;
            _x0 = x0;
        }

        private void PrintBasisPolynomial(double znam, int iter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("p" + iter + " = ");
            stringBuilder.Append((1 / znam).ToString(@"F" + _countSignAfter));
            for (var j = 0; j < _xValues.Length; j++)
            {
                if (iter != j)
                {
                    stringBuilder.Append(" * (x - " + _xValues[j] + ")");
                }
            }
            Console.WriteLine(stringBuilder.ToString());
        }

        private void PrintPolynomialLagrange()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("L" + (_xValues.Length - 1) + "(x) = ");
            for (var i = 0; i < _xValues.Length; i++)
            {
                stringBuilder.Append(" + p" + i + " * " + _yValues[i].ToString(@"F" + _countSignAfter));
            }
            Console.WriteLine(stringBuilder.ToString());
        }

        private void PrintPolinomialNewton(double[] incs, bool finalDifference)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("N" + (_xValues.Length - 1) + "(x) = ");
            if (finalDifference)
            {
                Console.WriteLine("q = (x - " + _xValues[0].ToString(@"F" + _countSignAfter) + ") / " + (_xValues[1] - _xValues[0]).ToString(@"F" + _countSignAfter));
                stringBuilder.Append(incs[0].ToString(@"F" + _countSignAfter));
                for (var i = 1; i < incs.Length; i++)
                {
                    stringBuilder.Append(" + q");
                    for (var j = 1; j < i; j++)
                    {
                        stringBuilder.Append(" * (q - " + j + ")");
                    }
                    stringBuilder.Append(" * " + incs[i].ToString(@"F" + _countSignAfter) + " / " + i + "!");
                }
            }
            else
            {
                stringBuilder.Append(incs[0].ToString(@"F" + _countSignAfter));
                for (var i = 1; i < incs.Length; i++)
                {
                    stringBuilder.Append(" + " + incs[i].ToString(@"F" + _countSignAfter));
                    for (var j = 0; j < i; j++)
                    {
                        stringBuilder.Append(" * " + "(x - " + _xValues[j].ToString(@"F" + _countSignAfter) + ")");
                    }
                }
            }
            Console.WriteLine(stringBuilder.ToString());
        }

        private void PrintLinearSpline(double[] aValues, double[] bValues)
        {
            for (var i = 0; i < aValues.Length; i++)
            {
                Console.WriteLine(aValues[i].ToString(@"F" + _countSignAfter) + " * x + " + bValues[i].ToString(@"F" + _countSignAfter) + ", " + _xValues[i].ToString(@"F" + _countSignAfter) + " <= x <= " + _xValues[i + 1].ToString(@"F" + _countSignAfter));
            }
        }

        private Function CreatePolynomialLagrange()
        {
            var basicPolynomials = new FunctionBasisPolinomial[_xValues.Length];
            for (var i = 0; i < _xValues.Length; i++)
            {
                basicPolynomials[i] = new FunctionBasisPolinomial((x, index) =>
                {
                    double result = 1;
                    double znamInfo = 1;
                    for (var j = 0; j < _xValues.Length; j++)
                    {
                        if (index != j)
                        {
                            result *= (x - _xValues[j]);
                            znamInfo *= (_xValues[index] - _xValues[j]);
                            result /= (_xValues[index] - _xValues[j]);
                        }
                    }
                    PrintBasisPolynomial(znamInfo, index);
                    return result;
                });
            }
            var lagrangePolynomial = new Function(x =>
            {
                double result = 0;
                for (var i = 0; i < _yValues.Length; i++)
                {
                    result += basicPolynomials[i](x, i) * _yValues[i];
                }
                PrintPolynomialLagrange();
                Console.WriteLine("L" + (_xValues.Length - 1) + "(" + x.ToString(@"F" + _countSignAfter) + ") = " + result.ToString(@"F" + _countSignAfter));
                return result;
            });
            return lagrangePolynomial;
        }

        private double[] GetNextDifference(double[] currentDifference, bool finalDifference, int iter)
        {
            var result = new double[currentDifference.Length - 1];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = (currentDifference[i + 1] - currentDifference[i]);
            }
            if (!finalDifference)
            {
                for (var i = 0; i < result.Length; i++)
                {
                    result[i] /= (_xValues[i + iter] - _xValues[i]);
                }
            }
            return result;
        }

        private int Factorial(int n)
        {
            if (n == 1 || n == 0) return 1;
            return n * Factorial(n - 1);
        }

        private Function GetInterpolationFormula(double[] incs, bool finalDifference)
        {
            Function result;
            if (finalDifference)
            {
                result = new Function(x =>
                {
                    double res = 0;
                    double q = (x - _xValues[0]) / (_xValues[1] - _xValues[0]);
                    for (var i = 0; i < incs.Length; i++)
                    {
                        double product = incs[i];
                        for (var j = 1; j <= i; j++)
                        {
                            product *= (q - j + 1);
                        }
                        product /= Factorial(i);
                        res += product;
                    }
                    Console.WriteLine("N" + (_xValues.Length - 1) + "(" + x.ToString(@"F" + _countSignAfter) + ") = " + res.ToString(@"F" + _countSignAfter));
                    return res;
                });
            }
            else
            {
                result = new Function(x =>
                {
                    double res = 0;
                    for (var i = 0; i < incs.Length; i++)
                    {
                        double product = incs[i];
                        for (var j = 0; j < i; j++)
                        {
                            product *= (x - _xValues[j]);
                        }
                        res += product;
                    }
                    Console.WriteLine("N" + (_xValues.Length - 1) + "(" + x.ToString(@"F" + _countSignAfter) + ") = " + res.ToString(@"F" + _countSignAfter));
                    return res;
                });
            }
            return result;
        }

        private bool CheckDistance(double[] xValues)
        {
            double distance = Math.Round(xValues[1] - xValues[0], _countSignAfter);
            for (var i = 1; i < xValues.Length - 1; i++)
            {
                if (Math.Round((xValues[i + 1] - xValues[i]), _countSignAfter) != distance) return false;
            }
            return true;
        }

        private Function CreatePolynomialNewton()
        {
            var incs = new double[_yValues.Length];
            incs[0] = _yValues[0];
            double[] difference = _yValues;
            bool finalDifference = CheckDistance(_xValues);
            for (var i = 1; i < _yValues.Length; i++)
            {
                difference = GetNextDifference(difference, finalDifference, i);
                incs[i] = difference[0];
            }
            PrintPolinomialNewton(incs, finalDifference);
            return GetInterpolationFormula(incs, finalDifference);
        }

        private (double[], double[]) GetSplainParameters()
        {
            var aValues = new double[_yValues.Length - 1];
            var bValues = new double[_yValues.Length - 1];
            for (var i = 0; i < aValues.Length; i++)
            {
                aValues[i] = (_yValues[i + 1] - _yValues[i]) / (_xValues[i + 1] - _xValues[i]);
                bValues[i] = _yValues[i] - aValues[i] * _xValues[i];
            }
            return (aValues, bValues);
        }

        public Function CreateLinearSpline()
        {
            var (aValues, bValues) = GetSplainParameters();
            PrintLinearSpline(aValues, bValues);
            return new Function(x =>
            {
                double res = 0;
                if (x < _xValues[1])
                {
                    res = x * aValues[0] + bValues[0];
                    Console.WriteLine("ф(" + x.ToString(@"F" + _countSignAfter) + ") = " + res.ToString(@"F" + _countSignAfter));
                    return res;
                }
                if (x > _xValues[_xValues.Length - 2])
                {
                    res = x * aValues[aValues.Length - 1] + bValues[bValues.Length - 1];
                    Console.WriteLine("ф(" + x.ToString(@"F" + _countSignAfter) + ") = " + res.ToString(@"F" + _countSignAfter));
                    return res;
                }
                for (var i = 1; i < _xValues.Length - 1; i++)
                {
                    if (x <= _xValues[i + 1])
                    {
                        res = x * aValues[i] + bValues[i];
                        Console.WriteLine("ф(" + x.ToString(@"F" + _countSignAfter) + ") = " + res.ToString(@"F" + _countSignAfter));
                        return res;
                    }
                }
                return double.MaxValue;
            });
        }

        public void StartComputational()
        {
            CreatePolynomialLagrange()(_x0);
            CreatePolynomialNewton()(_x0);
            CreateLinearSpline()(_x0);
        }
    }
}
