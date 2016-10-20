using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class TCopula:EllipticalCopula
    {
        public TCopula(Matrix<double> rho, double dfreedom) : base(rho)
        {
            TransformDist = new StudentT(0.0,1.0,dfreedom, RandomSource);
        }

        public TCopula(Matrix<double> rho, double dfreedom, System.Random randomSource) : base(rho, randomSource)
        {
            TransformDist = new StudentT(0.0, 1.0, dfreedom, RandomSource);
        }
    }
}