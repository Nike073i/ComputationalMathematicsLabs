using System;

namespace ComputationalMathematicsLabs.Lab_2
{
    public class Matrix : IMatrix
    {
        private double[,] matrix;
        private int _sizeI;
        private int _sizeJ;
        public int SizeI => _sizeI;
        public int SizeJ => _sizeJ;

        public double this[int i, int j]
        {
            get => matrix[i, j];
            set => matrix[i, j] = value;
        }


        public Matrix(double[,] newMatrix)
        {
            if (newMatrix == null) throw new ArgumentNullException();
            matrix = newMatrix;
            _sizeI = newMatrix.GetUpperBound(0) + 1;
            _sizeJ = newMatrix.GetUpperBound(0) + 1;
        }
        public Matrix(int sizeI, int sizeJ)
        {
            if (sizeI < 1 || sizeJ < 1) throw new ArgumentException("Неправильно задана размерность");
            _sizeI = sizeI;
            _sizeJ = sizeJ;
            matrix = new double[sizeI, sizeJ];
        }

        public void SetRowValues(int i, params double[] values)
        {
            if (values.Length != _sizeJ) throw new ArgumentException("Неправильный размер подаваемой строки");
            var j = 0;
            foreach (var value in values)
            {
                matrix[i, j] = value;
                j++;
            }
        }

        public Matrix Additional(Matrix matrixB)
        {
            if (matrixB == null) throw new ArgumentNullException();
            if (matrixB.SizeI != _sizeI || matrixB.SizeJ != _sizeJ) throw new ArgumentException("Размерность матриц не соответствует");
            var matrixC = new Matrix(_sizeI, _sizeJ);
            for (var i = 0; i < _sizeI; i++)
            {
                for (var j = 0; j < _sizeJ; j++)
                {
                    matrixC[i, j] = matrix[i, j] + matrixB[i, j];
                }
            }

            return matrixC;
        }

        public Matrix Subtraction(Matrix matrixB)
        {
            if (matrixB == null) throw new ArgumentNullException();
            if (matrixB.SizeI != _sizeI || matrixB.SizeJ != _sizeJ) throw new ArgumentException("Размерность матриц не соответствует");
            var matrixC = new Matrix(_sizeI, _sizeJ);
            for (var i = 0; i < _sizeI; i++)
            {
                for (var j = 0; j < _sizeJ; j++)
                {
                    matrixC[i, j] = matrix[i, j] - matrixB[i, j];
                }
            }

            return matrixC;
        }

        public Matrix Multiply(Matrix matrixB)
        {
            if (matrixB == null) throw new ArgumentNullException();
            if (matrixB.SizeI != _sizeJ) throw new ArgumentException("Размерность матриц не соответствует");
            var matrixC = new Matrix(_sizeI, matrixB.SizeJ);
            for (var i = 0; i < _sizeI; i++)
            {
                for (var j = 0; j < matrixB.SizeJ; j++)
                {
                    matrixC[i, j] = 0;
                    for (var k = 0; k < _sizeJ; k++)
                    {
                        matrixC[i, j] += matrix[i, k] * matrixB[k, j];
                    }
                }
            }

            return matrixC;
        }

        public Matrix GetDiagonalMatrix()
        {
            var diagonalMatrix = new Matrix(_sizeI, _sizeJ);
            for (var i = 0; i < Math.Min(_sizeI, _sizeJ); i++)
            {
                diagonalMatrix[i, i] = matrix[i, i];
            }

            return diagonalMatrix;
        }

        public Matrix GetMatrixWithoutDiagonal()
        {
            var result = new Matrix(_sizeI, _sizeJ);
            for (var i = 0; i < _sizeI; i++)
            {
                for (var j = 0; j < _sizeJ; j++)
                {
                    if (i != j)
                        result[i, j] = matrix[i, j];
                }
            }

            return result;
        }

        public Matrix GetNegativeMatrix()
        {
            var result = new Matrix(_sizeI, _sizeJ);
            for (var i = 0; i < _sizeI; i++)
            {
                for (var j = 0; j < _sizeJ; j++)
                {
                    result[i, j] = -matrix[i, j];
                }
            }

            return result;
        }
    }
}
