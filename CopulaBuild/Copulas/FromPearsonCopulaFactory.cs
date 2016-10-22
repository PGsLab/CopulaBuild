using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class FromPearsonCopulaFactory : ICopulaFactory
    {
        public Copula CreateGaussianCopula(Matrix<double> rho, System.Random randomSource = null)
        {
            return new GaussianCopula(rho,randomSource);
        }

        public Copula CreateTCopula(Matrix<double> rho, double dFreedom, System.Random randomSource = null)
        {
            return new TCopula(rho, dFreedom, randomSource);
        }
    }
}