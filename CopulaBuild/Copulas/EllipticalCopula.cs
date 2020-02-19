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
        protected IContinuousDistribution _transformDist;

        protected EllipticalCopula() { }
        protected EllipticalCopula(IContinuousDistribution transformDist)
        {
            _transformDist = transformDist;
            _transformDist.RandomSource = RandomSource;
        }
        protected EllipticalCopula(Matrix<double> rho, CorrelationType correlationType, RandomSource randomSource, IContinuousDistribution transformDist) : this(transformDist)
        {
            Rho = EllipticalCopula.ConvertToPearsonLinearCorrelationMatrix(rho, correlationType);
            if (randomSource != null)
                RandomSource = randomSource;
        }

        /// <summary>
        /// Gets the random number generator which is used to draw random samples.
        /// </summary>
        public sealed override System.Random RandomSource
        {
            get { return _randomSource; }
            set
            {
                var randomSource = value ?? SystemRandomSource.Default;
                base.RandomSource = randomSource;
                if (_transformDist != null)
                    _transformDist.RandomSource = randomSource;
            }
        }

        /// <summary>
        /// Gets the underlying correlation matrix of the copula.
        /// </summary>
        public sealed override Matrix<double> Rho
        {
            protected set
            {
                if (!Copula.IsValidParameterSet(value))
                {
                    throw new Copula.InvalidCorrelationMatrixException();
                }
                base.Rho = value;
                _matrixnormal = CreateMatrixNormal();
            }
        }

        private MatrixNormal CreateMatrixNormal()
        {
            var mu = Matrix<double>.Build.Dense(Dimension, 1);
            var k = Matrix<double>.Build.Dense(1, 1, 1.0);
            return new MatrixNormal(mu, Rho, k, RandomSource);
        }

        /// <summary>
        /// Generates a correlated uniform sample corresponding to the parametrized copula.
        /// </summary>
        /// <returns>a correlated sample from the copula.</returns>
        public override double[] Sample()
        {
            var result = new double[Dimension];
            var mvnSample = _matrixnormal.Sample();
            for (var m = 0; m < Dimension; ++m)
                result[m] = _transformDist.CumulativeDistribution(mvnSample[m, 0]);
            return result;
        }

        /// <summary>
        /// Converts the factors of a given correlation matrix to linear correlation factors
        /// </summary>
        /// <param name="rho">The correlation matrix.</param>
        /// <param name="correlationType">The correlation type of the input matrix.</param>
        /// <returns>a Pearson linear correlation matrix.</returns>
        public static Matrix<double> ConvertToPearsonLinearCorrelationMatrix(Matrix<double> rho, CorrelationType correlationType)
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

        private static double ConvertKendallToPearson(double rho)
        {
            return Math.Sin(rho * Math.PI / 2);
        }
        private static double ConvertSpearmanToPearson(double rho)
        {
            return 2 * Math.Sin(rho * Math.PI / 6);
        }

        private static Matrix<double> ConvertKendallToPearson(Matrix<double> rho)
        {
            return ConvertMatrixToPearsonCorrelation(rho, ConvertKendallToPearson);
        }
        private static Matrix<double> ConvertSpearmanToPearson(Matrix<double> rho)
        {
            return ConvertMatrixToPearsonCorrelation(rho, ConvertSpearmanToPearson);
        }
        private static Matrix<double> ConvertMatrixToPearsonCorrelation(Matrix<double> rho, Func<double, double> converter)
        {
            Matrix<double> pearsonRho = Matrix<double>.Build.Dense(rho.RowCount, rho.ColumnCount);
            pearsonRho.SetDiagonal(Vector<double>.Build.Dense(pearsonRho.RowCount, 1));
            for (int i = 0; i < rho.RowCount; ++i)
            {
                for (int j = 0; j < rho.ColumnCount; ++j)
                {
                    if (i != j)
                        pearsonRho[i, j] = converter(rho[i, j]);
                }
            }
            return pearsonRho;
        }
    }
}