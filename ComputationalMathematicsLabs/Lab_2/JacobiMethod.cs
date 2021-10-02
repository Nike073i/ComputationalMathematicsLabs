using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalMathematicsLabs.Lab_2
{
    public class JacobiMethod
    {
        public JacobiMethod(double[,] aValues, double[] freeTermsVector)
        {
            if (aValues == null || freeTermsVector == null) throw new ArgumentNullException();
            var matrixA = new Matrix(aValues);
            var matrixFreeTerms = new Matrix(freeTermsVector.Length, 1);
            for (var i = 0; i < freeTermsVector.Length;i++)
            {
                matrixFreeTerms.SetRowValues(i,freeTermsVector[i]);
            }
            var inverseDiagonalMatrixA = matrixA.GetDiagonalMatrix();
            for (var i = 0; i < inverseDiagonalMatrixA.SizeI; i++)
            {
                inverseDiagonalMatrixA[i, i] = 1 / inverseDiagonalMatrixA[i, i];
            }

            var negativeDiagonalMatrix = inverseDiagonalMatrixA.GetNegativeMatrix();
            var matrixWithoutDiagonal = matrixA.GetMatrixWithoutDiagonal();
            var matrixB = negativeDiagonalMatrix.Multiply(matrixWithoutDiagonal);
            var matrixC = inverseDiagonalMatrixA.Multiply(matrixFreeTerms);
        }
    }
}
