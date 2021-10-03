namespace ComputationalMathematicsLabs.Lab_2
{
    public interface IMatrix
    {
        void SetRowValues(int i, params double[] values);
        Matrix Additional(Matrix matrixB);
        Matrix Subtraction(Matrix matrixB);
        Matrix Multiply(Matrix matrixB);
        Matrix GetDiagonalMatrix();
        Matrix GetMatrixWithoutDiagonal();
        Matrix GetNegativeMatrix();
        double GetNorm();
    }
}
