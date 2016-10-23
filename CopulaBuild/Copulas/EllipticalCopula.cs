    using System;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using MathNet.Numerics;
    using MathNet.Numerics.Properties;

namespace MathNet.Numerics.Copulas
{
    public abstract class EllipticalCopula : Copula
    {
        private readonly MatrixNormal _matrixnormal;
        private readonly IContinuousDistribution _transformDist;

        protected EllipticalCopula(Matrix<double> rho, IContinuousDistribution transformDist, System.Random randomSource = null)
        {
            if (!IsValidParameterSet(rho))
            {
                throw new ArgumentException(Resources.InvalidDistributionParameters);
            }
            //ToDo implement checks
            Rho = rho;
            Dimension = Rho.ColumnCount;
            RandomSource = randomSource ?? SystemRandomSource.Default;
            _matrixnormal = GetMatrixNormal();
            _transformDist = transformDist;
            _transformDist.RandomSource = RandomSource;
        }

        private MatrixNormal GetMatrixNormal()
        {
            var mu = Matrix<double>.Build.Sparse(Dimension, 1);
            var k = Matrix<double>.Build.Sparse(1, 1, 1.0);
            return new MatrixNormal(mu, Rho, k, RandomSource);
        }
        public override Matrix<double> Sample()
        {
            Matrix<double> result = Matrix<double>.Build.Dense(1, Dimension);
            var mvnSample = _matrixnormal.Sample();
            for (var m = 0; m < Dimension; ++m)
                result[0, m] = _transformDist.CumulativeDistribution(mvnSample[m, 0]);
            return result;
        }

        public static double ConvertKendallToPearson(double rho)
        {
            return Math.Sin(rho * Math.PI / 2);
        }
        public static double ConvertSpearmanToPearson(double rho)
        {
            return 2 * Math.Sin(rho * Math.PI / 6);
        }

        public static Matrix<double> ConvertKendallToPearson(Matrix<double> rho)
        {
            Matrix<double> pearsonRho = Matrix<double>.Build.Dense(rho.RowCount, rho.ColumnCount);
            for (int i = 0; i < rho.RowCount; ++i)
            {
                for (int j = 0; j < rho.ColumnCount; ++j)
                {
                    pearsonRho[i, j] = ConvertKendallToPearson(rho[i, j]);
                }
            }
            return pearsonRho;
        }
        public static Matrix<double> ConvertSpearmanToPearson(Matrix<double> rho)
        {
            Matrix<double> pearsonRho = Matrix<double>.Build.Dense(rho.RowCount, rho.ColumnCount);
            for (int i = 0; i < rho.RowCount; ++i)
            {
                for (int j = 0; j < rho.ColumnCount; ++j)
                {
                    pearsonRho[i, j] = ConvertSpearmanToPearson(rho[i, j]);
                }
            }
            return pearsonRho;
        }

        public static Matrix<double> CreateCorrMatrixFromDouble(double rho)
        {
            var result = Matrix<double>.Build.DenseDiagonal(2, 2, 1);
            result[0, 1] = rho;
            result[1, 0] = rho;
            return result;
        }

        /// <summary>
        /// Tests whether the provided values are valid parameters for these Copulas.
        /// </summary>
        /// <param name="rho">The correlation matrix.</param>
        public static bool IsValidParameterSet(Matrix<double> rho)
        {
            var n = rho.RowCount;
            var p = rho.ColumnCount;

            for (var i = 0; i < rho.RowCount; i++)
            {
                if (!rho.At(i, i).Equals(1.0))
                {
                    return false;
                }
            }

            for (var i = 0; i < rho.RowCount; i++)
            {
                for (var j = i+1; j < rho.ColumnCount; j++)
                {
                    if (!rho.At(i, j).AlmostEqual(rho.At(j,i)) || rho.At(i, j) <= -1 || rho.At(i, j) >= 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}