using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using System.Diagnostics;

namespace MathNet.Numerics.Copulas
{
    class Program
    {
        static void Main(string[] args)
        {
            var rho = Matrix<double>.Build.Dense(3, 3);
            rho[0,0] = 1;
            rho[1,1] = 1;
            rho[2,2] = 1;
            rho[0,1] = 0.5;
            rho[1,0] = 0.5;

            var thisCopula = new Copulas.TCopula(rho,4.5);
            var samples = thisCopula.GetSamples(10000);
            double[][] convertedSamples = new double[samples.ColumnCount][];
            for (var j = 0; j < samples.ColumnCount; ++j)
            {
                convertedSamples[j] = new double[samples.RowCount];
                for (var k = 0; k < samples.RowCount; ++k)
                {
                    convertedSamples[j][k] = samples[k, j];
                }
            }

            var correlation = MathNet.Numerics.Statistics.Correlation.PearsonMatrix(convertedSamples);
            Console.WriteLine("Rho:");
            Console.WriteLine(rho.ToString());
            Console.WriteLine("Estimation:");
            Console.WriteLine(correlation.ToString());

            Stopwatch sw = new Stopwatch();

            sw.Start();
            Matrix<double> test;
            for (var k = 0; k < 100; ++k)
                test = thisCopula.GetSamples(10000);
            Console.WriteLine("New method: " + sw.Elapsed.TotalSeconds.ToString());

            Console.ReadKey();
        }
    }
}
