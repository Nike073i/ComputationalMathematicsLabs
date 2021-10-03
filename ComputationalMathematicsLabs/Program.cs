using ComputationalMathematicsLabs.Lab_2;
using System;

namespace ComputationalMathematicsLabs
{
    public class Program
    {
        public static void Main()
        {
            double[,] testMatrixOne =
            {
                {2.923, 0.220, 0.159, 0.328},
                {0.363, 4.123, 0.268, 0.327},
                {0.169, 0.271, 3.906, 0.295},
                {0.241, 0.319, 0.257, 3.862}
            };
            Console.WriteLine("Введите эпсилон (например: 0,00001)");
            double.TryParse(Console.ReadLine(), out double epsilon);
            epsilon = epsilon == 0 ? 0.001 : epsilon;
            double[] freeTermsVector = { 0.605, 0.496, 0.590, 0.896 };
            var computational = new JacobiMethod(testMatrixOne, freeTermsVector, epsilon);
            computational.StartComputational();
            Console.ReadKey();
        }
    }
}
