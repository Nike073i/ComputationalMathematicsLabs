using ComputationalMathematicsLabs.Lab_2;
using ComputationalMathematicsLabs.Lab_4;
using System;

namespace ComputationalMathematicsLabs
{
    public class Program
    {
        public static void Main()
        {
            var alphaOne = 0.2005;
            var alphaTwo = 0.18;

            Console.WriteLine("Введите эпсилон (например: 0,000001)");
            double.TryParse(Console.ReadLine(), out double epsilon);
            epsilon = epsilon == 0 ? 0.000001 : epsilon;
            Console.WriteLine("Введите x1 (например: 1,0)");
            double.TryParse(Console.ReadLine(), out double startX1);
            Console.WriteLine("Введите x1 (например: 1,0)");
            double.TryParse(Console.ReadLine(), out double startX2);
            Console.WriteLine("1ый корень или 2ой: (например: 2)");
            int.TryParse(Console.ReadLine(), out int changeResult);
            var alpha = changeResult == 2 ? alphaTwo : alphaOne;
            var startXMatrix = new [] {startX1, startX2 };
            var sdm = new SteepestDescentMethod(epsilon, startXMatrix, alpha,
                (x1, x2) => 2 * (Math.Sin(x1 + x2) - 1.2 * x1 - 0.1) * (Math.Cos(x1 + x2) - 1.2) +
                            2 * (x1 * x1 + x2 * x2 - 1) * 2 * x1,
                (x1, x2) => 2 * (Math.Sin(x1 + x2) - 1.2 * x1 - 0.1) * Math.Cos(x1 + x2) +
                            2 * (x1 * x1 + x2 * x2 - 1) * 2 * x2);
            sdm.StartComputational();
            Console.ReadKey();
        }
    }
}
