using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class TCopula:EllipticalCopula
    {
        public double DFreedom { get; protected set; }

        public TCopula(Matrix<double> rho, double dfreedom)
            : base(rho, TCopula.GetTransFormDist(dfreedom))
        {
            DFreedom = dfreedom;
        }

        public TCopula(Matrix<double> rho, double dfreedom, System.Random randomSource)
            : base(rho, TCopula.GetTransFormDist(dfreedom), randomSource)
        {
            DFreedom = dfreedom;
        }
        private static IContinuousDistribution GetTransFormDist(double dfreedom)
        {
            return new StudentT(0.0, 1.0, dfreedom);
        }
    }
}