﻿    using System;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public abstract class EllipticalCopula : Copula
    {
        public readonly Matrix<double> Rho;
        private readonly MatrixNormal _matrixnormal;
        private readonly IContinuousDistribution _transformDist;

        protected EllipticalCopula(Matrix<double> rho, IContinuousDistribution transformDist, System.Random randomSource = null)
        {
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
    }
}