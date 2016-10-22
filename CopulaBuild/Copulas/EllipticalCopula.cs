    using System;
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

        protected EllipticalCopula(Matrix<double> rho, IContinuousDistribution transformDist)
        {
            //ToDo implement checks
            Rho = rho;
            Dimension = Rho.ColumnCount;
            _matrixnormal = GetMatrixNormal();
            _transformDist = transformDist;
            _transformDist.RandomSource = RandomSource;
        }

        protected EllipticalCopula(Matrix<double> rho, IContinuousDistribution transformDist, System.Random randomSource)
            : this(rho, transformDist)
        {
            RandomSource = randomSource ?? SystemRandomSource.Default;
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
    }
}