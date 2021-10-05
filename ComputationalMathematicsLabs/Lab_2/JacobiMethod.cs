using System;
using System.Text;

namespace ComputationalMathematicsLabs.Lab_2
{
    public class JacobiMethod
    {
        private int _iter;
        private Matrix _matrixA;
        private Matrix _matrixFreeTerms;
        private double _epsilon;
        private int lengthEps = 3;
        public JacobiMethod(double[,] aValues, double[] freeTermsVector, double epsilon)
        {
            if (aValues == null || freeTermsVector == null) throw new ArgumentNullException();
            if (epsilon <= 0) throw new ArgumentException("Неверное значение эпсилон");
            _matrixA = new Matrix(aValues);
            _matrixFreeTerms = new Matrix(freeTermsVector.Length, 1);
            _epsilon = epsilon;
            string epsilonString = string.Format("{0:#,#.#############################}", epsilon);
            lengthEps = epsilonString.Split(',')[1].Length;
            for (var i = 0; i < freeTermsVector.Length; i++)
            {
                _matrixFreeTerms.SetRowValues(i, freeTermsVector[i]);
            }
        }

        public Matrix StartComputational()
        {
            Matrix result = null;
            var inverseDiagonalMatrixA = _matrixA.GetDiagonalMatrix();
            for (var i = 0; i < inverseDiagonalMatrixA.SizeI; i++)
            {
                inverseDiagonalMatrixA[i, i] = 1 / inverseDiagonalMatrixA[i, i];
            }

            var negativeDiagonalMatrix = inverseDiagonalMatrixA.GetNegativeMatrix();
            var matrixWithoutDiagonal = _matrixA.GetMatrixWithoutDiagonal();
            var matrixB = negativeDiagonalMatrix.Multiply(matrixWithoutDiagonal);
            var matrixC = inverseDiagonalMatrixA.Multiply(_matrixFreeTerms);
            if (СheckConvergence(_matrixA))
            {
                Console.WriteLine("Матрица B:");
                Console.WriteLine(matrixB.ToString());
                Console.WriteLine("");
                Console.WriteLine("Вектор c:");
                Console.WriteLine(matrixC.ToStringVector());
                Console.WriteLine("");
                Console.WriteLine("Эпсилон:");
                Console.WriteLine(_epsilon);
                Console.WriteLine("");
                Console.WriteLine("Таблица итераций");
                var normMatrixB = matrixB.GetNorm();
                var bound = ((1 - normMatrixB) / normMatrixB) * _epsilon;
                Console.WriteLine("      k       x1        x2        x3        x4     Error");
                _iter = 0;
                result = IterativeMethodJacobi(matrixB, matrixC, new Matrix(4, 1), double.MaxValue, bound);
            }

            return result;
        }

        private bool СheckConvergence(Matrix matrix)
        {
            for (int i = 0; i < matrix.SizeI; i++)
            {
                double sum = 0;
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(matrix[i, i] + "#");
                for (var j = 0; j < matrix.SizeJ; j++)
                {
                    if (j != i)
                    {
                        sum += matrix[i, j];
                    }

                    if (j == 0) stringBuilder.Append(" " + matrix[i, j] + " ");
                    else stringBuilder.Append("+ " + matrix[i, j] + " ");
                }

                if (sum >= matrix[i, i])
                {
                    stringBuilder.Replace('#', '<');
                    Console.WriteLine(stringBuilder + "\n Условие нарушено");
                    return false;
                }
                stringBuilder.Replace('#', '>');
                Console.WriteLine(stringBuilder.ToString());
            }
            Console.WriteLine("");
            return true;
        }

        private Matrix IterativeMethodJacobi(Matrix B, Matrix freeTermsVector, Matrix x, double error, double bound)
        {
            var result = x;
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("      " + _iter);
            result.CountSighAfter = lengthEps;
            stringBuilder.Append(result.ToStringVector());
            stringBuilder.Append(error != double.MaxValue ? error.ToString() : "");
            Console.WriteLine(stringBuilder.ToString());
            if (!(error >= bound)) return result;
            var newX = B.Multiply(x).Additional(freeTermsVector);
            error = newX.Subtraction(x).GetNorm();
            _iter++;
            result = IterativeMethodJacobi(B, freeTermsVector, newX, error, bound);
            return result;
        }
    }
}
