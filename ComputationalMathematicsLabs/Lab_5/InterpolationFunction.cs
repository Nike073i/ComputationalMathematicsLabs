using System;

namespace ComputationalMathematicsLabs.Lab_5
{
    public class InterpolationFunction
    {
        private readonly double[] _xValues;
        private readonly double[] _yValues;
        private readonly double _x0;
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

        private Function CreatePolynomialLagrange()
        {
            var basicPolynomials = new FunctionBasisPolinomial[_xValues.Length];
            for (var i = 0; i < _xValues.Length; i++)
            {
                basicPolynomials[i] = new FunctionBasisPolinomial((x, index) =>
                {
                    double result = 1;
                    for (var j = 0; j < _xValues.Length; j++)
                    {
                        if (index != j)
                        {
                            result *= (x - _xValues[j]);
                            result /= (_xValues[index] - _xValues[j]);
                        }
                    }
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
                    for (var i = 0; i< incs.Length; i++)
                    {
                        double product = incs[i];
                        for (var j = 1; j <= i; j++)
                        {
                            product *= (q - j + 1);
                        }
                        product /= Factorial(i);
                        res += product;
                    }
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
                    return res;
                });
            }
            return result;
        }

        private bool CheckDistance(double[] xValues)
        {
            double distance = xValues[1] - xValues[0];
            for (var i = 1; i < xValues.Length - 1; i++)
            {
                if (xValues[i + 1] - xValues[i] != distance) return false;
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

            return GetInterpolationFormula(incs, finalDifference);
        }

        public void StartComputational()
        {
            double resLagrange = CreatePolynomialLagrange()(_x0);
            double resNewton = CreatePolynomialNewton()(_x0);
            Console.WriteLine(resLagrange);
            Console.WriteLine(resNewton);
        }
    }
}
