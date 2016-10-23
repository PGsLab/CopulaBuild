using System;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Copulas
{
    public class FromKendallCopulaFactory : ICopulaFactory
    {
        public Copula CreateGaussianCopula(Matrix<double> rho, System.Random randomSource = null)
        {
            Matrix<double> pearsonRho = EllipticalCopula.ConvertKendallToPearson(rho);
            return new GaussianCopula(pearsonRho, randomSource);
        }
        public Copula CreateGaussianCopula(double rho, System.Random randomSource = null)
        {
            var matrixRho = EllipticalCopula.CreateCorrMatrixFromDouble(rho);
            Matrix<double> pearsonRho = EllipticalCopula.ConvertKendallToPearson(matrixRho);
            return new GaussianCopula(pearsonRho, randomSource);
        }

        public Copula CreateTCopula(Matrix<double> rho, double dFreedom, System.Random randomSource = null)
        {
            Matrix<double> pearsonRho = EllipticalCopula.ConvertKendallToPearson(rho);
            return new TCopula(pearsonRho, dFreedom, randomSource);
        }
        public Copula CreateTCopula(double rho, double dFreedom, System.Random randomSource = null)
        {
            var matrixRho = EllipticalCopula.CreateCorrMatrixFromDouble(rho);
            Matrix<double> pearsonRho = EllipticalCopula.ConvertKendallToPearson(matrixRho);
            return new TCopula(pearsonRho, dFreedom, randomSource);
        }
        
        public Copula CreateClaytonCopula(double rho, System.Random randomSource = null)
        {
            double theta = ClaytonCopula.ThetafromKendall(rho);
            return new ClaytonCopula(theta,randomSource);
        }
        public Copula CreateGumbelCopula(double rho, System.Random randomSource = null)
        {
            double theta = GumbelCopula.ThetafromKendall(rho);
            return new GumbelCopula(theta, randomSource);
        }
    }
}