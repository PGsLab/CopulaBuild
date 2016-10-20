using System;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class EllipticalCopula : Copula
    {
        public readonly Matrix<double> Rho;
        protected readonly System.Random RandomSource = SystemRandomSource.Default;
        private readonly MatrixNormal _matrixnormal;
        protected IContinuousDistribution TransformDist;

        public EllipticalCopula(Matrix<double> rho)
        {
            //ToDo implement checks
            Rho = rho;
            NrVariables = Rho.ColumnCount;
            var mu = Matrix<double>.Build.Sparse(NrVariables, 1);
            var k = Matrix<double>.Build.Sparse(1, 1, 1.0);
            _matrixnormal = new MatrixNormal(mu, Rho, k, RandomSource);
            
        }
        public EllipticalCopula(Matrix<double> rho, System.Random randomSource) : this(rho)
        {
            RandomSource = randomSource ?? SystemRandomSource.Default;
        }
        public override Matrix<double> Sample()
        {
            Matrix<double> result = Matrix<double>.Build.Dense(1, NrVariables);
            var mvnSample = _matrixnormal.Sample();
            for (var m = 0; m < NrVariables; ++m)
                result[0, m] = TransformDist.CumulativeDistribution(mvnSample[m, 0]);
            return result;
        }
    }
}