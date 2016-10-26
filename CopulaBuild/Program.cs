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

            var thisCopula = TCopula.Builder()
                .SetCorrelationType(CorrelationType.PearsonLinear)
                .SetRho(rho)
                .SetDFreedom(4.5)
                .Build();
            var samples = thisCopula.GetSamples(10000);
            var convertedSamples = ConvertMatrix(samples);

            var correlation = MathNet.Numerics.Statistics.Correlation.PearsonMatrix(convertedSamples);
            Console.WriteLine("Rho:");
            Console.WriteLine(rho.ToString());
            Console.WriteLine("Estimation:");
            Console.WriteLine(correlation.ToString());

            //var kendallCopulaFactory = new FromKendallCopulaFactory();
            //var corr = 0.3;
            //var claytonC = kendallCopulaFactory.CreateClaytonCopula(corr);

            //samples = claytonC.GetSamples(10000);
            //convertedSamples = ConvertMatrix(samples);

            //correlation = MathNet.Numerics.Statistics.Correlation.PearsonMatrix(convertedSamples);
            //Console.WriteLine("Rho:");
            //Console.WriteLine(corr.ToString());
            //Console.WriteLine("Estimation:");
            //Console.WriteLine(correlation.ToString());

            var test = GaussianCopula.Builder()
                .SetCorrelationType(CorrelationType.PearsonLinear)
                .SetRho(rho)
                .SetRandomSource(SystemRandomSource.Default)
                .Build();

            Console.ReadKey();
        }

        private static double[][] ConvertMatrix(Matrix<double> matrix)
        {
            double[][] convertedSamples = new double[matrix.ColumnCount][];
            for (var j = 0; j < matrix.ColumnCount; ++j)
            {
                convertedSamples[j] = new double[matrix.RowCount];
                for (var k = 0; k < matrix.RowCount; ++k)
                {
                    convertedSamples[j][k] = matrix[k, j];
                }
            }
            return convertedSamples;
        }
    }
}
