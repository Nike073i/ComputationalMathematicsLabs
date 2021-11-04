using System;

namespace ComputationalMathematicsLabs.Lab_5
{
    public class InterpolationFunction
    {
        private readonly double[] _xValues;
        private readonly double[] _yValues;
        private readonly double _x0;
        public delegate double Function(double x); 
        public delegate double FunctionBasisPolinomial(double x,int index);

        public InterpolationFunction(double[] xValues, double[] yValues, double x0)
        {
            if (xValues is null || yValues is null
                || !xValues.Length.Equals(yValues.Length)) throw new ArgumentException("Введены неверные значения");
            _xValues = xValues;
            _yValues = yValues;
            _x0 = x0;
        }

        private Function CreatePolynomialLagrangePolynomial()
        {
            var basicPolynomials = new FunctionBasisPolinomial[_xValues.Length];
            for (var i = 0; i < _xValues.Length; i++)
            {
                basicPolynomials[i] = new FunctionBasisPolinomial((x,index) =>
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
                    result += basicPolynomials[i](x,i) * _yValues[i];
                }
                return result;
            });
            return lagrangePolynomial;
        }
        public double StartComputational()
        {
            return CreatePolynomialLagrangePolynomial()(_x0);
        }
    }
}
