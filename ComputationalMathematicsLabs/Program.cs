using ComputationalMathematicsLabs.Lab_5;
using System;

namespace ComputationalMathematicsLabs
{
    public class Program
    {
        public static void Main()
        {
            double[] xValues = new[] { 0.079, 0.637, 1.345, 2.095, 2.782 };
            double[] yValues = new[] { -4.308, -0.739, 1.697, 4.208, 6.203 };
            //double[] xValues = new[] { 0.737, 0.837, 0.937, 1.037, 1.137, 1.237 };
            //double[] yValues = new[] { -0.323, 0.055, 0.405, 0.735, 1.052, 1.363 };

            Console.WriteLine("Введите x0 (например: 1,0)");
            double.TryParse(Console.ReadLine(), out double startX0);
            var interpolation = new InterpolationFunction(xValues, yValues, startX0);
            interpolation.StartComputational();
            Console.ReadKey();
        }
    }
}
