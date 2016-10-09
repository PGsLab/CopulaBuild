using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    class Program
    {
        static void Main(string[] args)
        {
            var Rho = Matrix<double>.Build.Dense(3, 3);
            Rho[0,0] = 1;
            Rho[1,1] = 1;
            Rho[2,2] = 1;
            Rho[0,1] = 0.5;
            Rho[1,0] = 0.5;

            var ThisCopula = new Copulas.GaussianCopula(Rho);
            var samples = ThisCopula.GetSamples(10000);
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
            Console.WriteLine(Rho.ToString());
            Console.WriteLine("Estimation:");
            Console.WriteLine(correlation.ToString());
            Console.ReadKey();

        }
    }
}
