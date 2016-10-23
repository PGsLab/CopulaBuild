using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class FromPearsonCopulaFactory : IEllipticalCopulaFactory
    {
        public Copula CreateGaussianCopula(Matrix<double> rho, System.Random randomSource = null)
        {
            return new GaussianCopula(rho,randomSource);
        }
        public Copula CreateGaussianCopula(double rho, System.Random randomSource = null)
        {
            var matrixRho = EllipticalCopula.CreateCorrMatrixFromDouble(rho);
            return new GaussianCopula(matrixRho, randomSource);
        }

        public Copula CreateTCopula(Matrix<double> rho, double dFreedom, System.Random randomSource = null)
        {
            return new TCopula(rho, dFreedom, randomSource);
        }
        public Copula CreateTCopula(double rho, double dFreedom, System.Random randomSource = null)
        {
            var matrixRho = EllipticalCopula.CreateCorrMatrixFromDouble(rho);
            return new TCopula(matrixRho, dFreedom, randomSource);
        }
    }
}