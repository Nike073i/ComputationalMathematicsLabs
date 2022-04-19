using System;
using System.Linq;
using System.Text;

namespace ComputationalMathematicsLabs.Lab_2
{
    public class Matrix : IMatrix
    {
        private int _countSignAfter = 4;
        private readonly double[,] matrix;
        private readonly int _sizeI;
        private readonly int _sizeJ;
        public int SizeI => _sizeI;
        public int SizeJ => _sizeJ;

        public int CountSighAfter
        {
            get => _countSignAfter;
            set
            {
                if (value > 0)
                {
                    _countSignAfter = value;
                }
            }
        }

        public double this[int i, int j]
        {
            get => matrix[i, j];
            set => matrix[i, j] = value;
        }


        public Matrix(double[,] newMatrix)
        {
            if (newMatrix == null)
            {
                throw new ArgumentNullException();
            }

            matrix = newMatrix;
            _sizeI = newMatrix.GetUpperBound(0) + 1;
            _sizeJ = newMatrix.GetUpperBound(0) + 1;
        }
        public Matrix(int sizeI, int sizeJ)
        {
            if (sizeI < 1 || sizeJ < 1)
            {
                throw new ArgumentException("Неправильно задана размерность");
            }

            _sizeI = sizeI;
            _sizeJ = sizeJ;
            matrix = new double[sizeI, sizeJ];
        }

        public Matrix(double[] newMatrix)
        {
            if (newMatrix == null)
            {
                throw new ArgumentNullException();
            }

            _sizeI = newMatrix.Length;
            _sizeJ = 1;
            matrix = new double[_sizeI, _sizeJ];
            for (int i = 0; i < _sizeI; i++)
            {
                SetRowValues(i, newMatrix[i]);
            }
        }

        public void SetRowValues(int i, params double[] values)
        {
            if (values.Length != _sizeJ)
            {
                throw new ArgumentException("Неправильный размер подаваемой строки");
            }

            int j = 0;
            foreach (double value in values)
            {
                matrix[i, j] = value;
                j++;
            }
        }

        public Matrix Additional(Matrix matrixB)
        {
            if (matrixB == null)
            {
                throw new ArgumentNullException();
            }

            if (matrixB.SizeI != _sizeI || matrixB.SizeJ != _sizeJ)
            {
                throw new ArgumentException("Размерность матриц не соответствует");
            }

            Matrix matrixC = new Matrix(_sizeI, _sizeJ);
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < _sizeJ; j++)
                {
                    matrixC[i, j] = matrix[i, j] + matrixB[i, j];
                }
            }

            return matrixC;
        }

        public Matrix Subtraction(Matrix matrixB)
        {
            if (matrixB == null)
            {
                throw new ArgumentNullException();
            }

            if (matrixB.SizeI != _sizeI || matrixB.SizeJ != _sizeJ)
            {
                throw new ArgumentException("Размерность матриц не соответствует");
            }

            Matrix matrixC = new Matrix(_sizeI, _sizeJ);
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < _sizeJ; j++)
                {
                    matrixC[i, j] = matrix[i, j] - matrixB[i, j];
                }
            }

            return matrixC;
        }

        public Matrix Multiply(Matrix matrixB)
        {
            if (matrixB == null)
            {
                throw new ArgumentNullException();
            }

            if (matrixB.SizeI != _sizeJ)
            {
                throw new ArgumentException("Размерность матриц не соответствует");
            }

            Matrix matrixC = new Matrix(_sizeI, matrixB.SizeJ);
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < matrixB.SizeJ; j++)
                {
                    matrixC[i, j] = 0;
                    for (int k = 0; k < _sizeJ; k++)
                    {
                        matrixC[i, j] += matrix[i, k] * matrixB[k, j];
                    }
                }
            }

            return matrixC;
        }

        public Matrix Multiply(double value)
        {
            Matrix matrixC = new Matrix(_sizeI, SizeJ);
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < SizeJ; j++)
                {
                    matrixC[i, j] = matrix[i, j] * value;
                }
            }
            return matrixC;
        }

        public Matrix GetDiagonalMatrix()
        {
            Matrix diagonalMatrix = new Matrix(_sizeI, _sizeJ);
            for (int i = 0; i < Math.Min(_sizeI, _sizeJ); i++)
            {
                diagonalMatrix[i, i] = matrix[i, i];
            }

            return diagonalMatrix;
        }

        public Matrix GetMatrixWithoutDiagonal()
        {
            Matrix result = new Matrix(_sizeI, _sizeJ);
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < _sizeJ; j++)
                {
                    if (i != j)
                    {
                        result[i, j] = matrix[i, j];
                    }
                }
            }

            return result;
        }

        public Matrix GetNegativeMatrix()
        {
            Matrix result = new Matrix(_sizeI, _sizeJ);
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < _sizeJ; j++)
                {
                    result[i, j] = -matrix[i, j];
                }
            }

            return result;
        }

        public double GetNorm()
        {
            double[] vector = new double[SizeI];
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < _sizeJ; j++)
                {
                    vector[i] += Math.Abs(matrix[i, j]);
                }
            }

            return vector.Max();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < _sizeJ; j++)
                {
                    string value = " " + Math.Round(matrix[i, j], _countSignAfter) + " ;";
                    string res = string.Format(@"{0," + (_countSignAfter + 6) + "}", value);
                    stringBuilder.Append(res);
                }

                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        public string ToStringVector()
        {
            if (_sizeJ != 1)
            {
                return ToString();
            }

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _sizeI; i++)
            {
                string value = " " + Math.Round(matrix[i, 0], _countSignAfter) + " ;";
                string res = string.Format(@"{0," + (_countSignAfter + 6) + "}", value);
                stringBuilder.Append(res);
            }

            return stringBuilder.ToString();
        }

        public double GetDeterminant2()
        {
            if (_sizeI != _sizeJ || _sizeI != 2)
            {
                throw new ArgumentException("Матрица не 2х2");
            }

            double mainDiag = matrix[0, 0] * matrix[1, 1];
            double secDiag = matrix[1, 0] * matrix[0, 1];
            return mainDiag - secDiag;
        }

        public double GetEuclidNorm()
        {
            double sum = 0d;
            for (int i = 0; i < _sizeI; i++)
            {
                for (int j = 0; j < _sizeJ; j++)
                {
                    sum += Math.Pow(matrix[i, j], 2d);
                }
            }
            return Math.Sqrt(sum);
        }

        public Matrix GetReverseMatrix2()
        {
            if (_sizeI != _sizeJ || _sizeI != 2)
            {
                throw new ArgumentException("Матрица не 2х2");
            }

            Matrix resMatrix = new Matrix(new[,] { { matrix[1, 1], matrix[1, 0] }, { matrix[0, 1], matrix[0, 0] } });
            return resMatrix;
        }

        public bool SylvesterCriterion2()
        {
            double min1 = matrix[0, 0];
            double min2 = GetDeterminant2();
            if (min1 > 0 && min2 > 0)
            {
                return true;
            }

            return false;
        }
    }
}
