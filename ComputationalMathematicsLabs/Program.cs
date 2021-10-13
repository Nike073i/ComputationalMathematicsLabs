using ComputationalMathematicsLabs.Lab_3;
using System;

namespace ComputationalMathematicsLabs
{
    public class Program
    {
        public static void Main()
        {
            
            Console.WriteLine("Введите эпсилон (например: 0,00001)");
            double.TryParse(Console.ReadLine(), out double epsilon);
            epsilon = epsilon == 0 ? 0.000001 : epsilon;
            Console.WriteLine("Введите шаг (например: 0,001)");
            double.TryParse(Console.ReadLine(), out double step);
            step = step == 0 ? 0.001 : step;
            Console.WriteLine("Введите начальное приближение (например: 2,0)");
            double.TryParse(Console.ReadLine(), out double startX);
            startX = startX == 0 ? 2 : startX;
            СonstantStepNewtonMethod csnm = new СonstantStepNewtonMethod(epsilon, startX, step, (x) => Math.Exp(-0.5 * x) - 0.2 * x * x + 1);
            csnm.StartComputational();
            Console.ReadKey();
        }
    }
}
