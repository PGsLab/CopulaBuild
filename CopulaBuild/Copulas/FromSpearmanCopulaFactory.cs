using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Copulas
{
    public class FromSpearmanCopulaFactory : ICopulaFactory
    {
        public Copula CreateGaussianCopula(Matrix<double> rho, System.Random randomSource = null)
        {
            Matrix<double> pearsonRho = EllipticalCopula.ConvertSpearmanToPearson(rho);
            return new GaussianCopula(pearsonRho, randomSource);
        }

        public Copula CreateTCopula(Matrix<double> rho, double dFreedom, System.Random randomSource = null)
        {
            Matrix<double> pearsonRho = EllipticalCopula.ConvertSpearmanToPearson(rho);
            return new TCopula(rho, dFreedom, randomSource);
        }

        public Copula CreateClaytonCopula(double rho, System.Random randomSource = null)
        {
            throw new System.NotImplementedException();
        }
        public Copula CreateGumbelCopula(double rho, System.Random randomSource = null)
        {
            throw new System.NotImplementedException();
        }
    }
}