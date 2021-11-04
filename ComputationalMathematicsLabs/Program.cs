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
            Console.WriteLine("Введите x0 (например: 1,0)");
            double.TryParse(Console.ReadLine(), out double startX0);
            startX0 = startX0 == 0 ? 1.982 : startX0;
            var interpolation = new InterpolationFunction(xValues,yValues,startX0);
            Console.WriteLine(interpolation.StartComputational());
            Console.ReadKey();
        }
    }
}
