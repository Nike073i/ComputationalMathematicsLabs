using ComputationalMathematicsLabs.Lab_2;

namespace ComputationalMathematicsLabs
{
    public class Program
    {
        public static void Main()
        {
            double[,] testMatrix =
            {
                {2.923, 0.220, 0.159, 0.328},
                {0.363, 4.123, 0.268, 0.327},
                {0.169, 0.271, 3.906, 0.295},
                {0.241, 0.319, 0.257, 3.862}
            };
            double[] freeTermsVector = { 0.605, 0.496, 0.590, 0.896 };
            var test = new JacobiMethod(testMatrix, freeTermsVector);
        }
    }
}
