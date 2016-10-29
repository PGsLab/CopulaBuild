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
        private MatrixNormal _matrixnormal;
        private IContinuousDistribution _transformDist;

        protected EllipticalCopula() { }
        protected EllipticalCopula(IContinuousDistribution transformDist)
        {
            _transformDist = transformDist;
            _transformDist.RandomSource = RandomSource;
        }

        public sealed override System.Random RandomSource
        {
            protected set
            {
                base.RandomSource = value;
                if (_transformDist != null)
                    _transformDist.RandomSource = value;
            }
        }

        protected void SetTransformDist(IContinuousDistribution transformDist)
        {
            _transformDist = transformDist;
        }

        public sealed override Matrix<double> Rho
        {
            protected set
            {
                if (!Copula.IsValidParameterSet(value))
                {
                    throw new Copula.InvalidCorrelationMatrixException();
                }
                base.Rho = value;
                Dimension = Rho.ColumnCount;
                _matrixnormal = GetMatrixNormal();
            }
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

        public static Matrix<double> GetPearsonRho(Matrix<double> rho, CorrelationType correlationType)
        {
            Matrix<double> pearsonRho;
            switch (correlationType)
            {
                case CorrelationType.PearsonLinear:
                    pearsonRho = rho;
                    break;
                case CorrelationType.KendallRank:
                    pearsonRho = EllipticalCopula.ConvertKendallToPearson(rho);
                    break;
                case CorrelationType.SpearmanRank:
                    pearsonRho = EllipticalCopula.ConvertSpearmanToPearson(rho);
                    break;
                default:
                    throw new System.ArgumentException();
            }
            return pearsonRho;
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
            pearsonRho.SetDiagonal(Vector<double>.Build.Dense(pearsonRho.RowCount,1));
            for (int i = 0; i < rho.RowCount; ++i)
            {
                for (int j = 0; j < rho.ColumnCount; ++j)
                {
                    if (i != j)
                        pearsonRho[i, j] = ConvertKendallToPearson(rho[i, j]);
                }
            }
            return pearsonRho;
        }
        public static Matrix<double> ConvertSpearmanToPearson(Matrix<double> rho)
        {
            Matrix<double> pearsonRho = Matrix<double>.Build.Dense(rho.RowCount, rho.ColumnCount);
            pearsonRho.SetDiagonal(Vector<double>.Build.Dense(pearsonRho.RowCount, 1));
            for (int i = 0; i < rho.RowCount; ++i)
            {
                for (int j = 0; j < rho.ColumnCount; ++j)
                {
                    if (i != j)
                        pearsonRho[i, j] = ConvertSpearmanToPearson(rho[i, j]);
                }
            }
            return pearsonRho;
        }
    }
}