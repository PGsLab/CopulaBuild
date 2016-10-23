using System.Runtime.InteropServices;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public interface ICopulaFactory : IEllipticalCopulaFactory, IArchimedeanCopulaFactory
    {
        
    }
    public interface IEllipticalCopulaFactory
    {
        Copula CreateGaussianCopula(Matrix<double> rho, System.Random randomSource = null);
        Copula CreateGaussianCopula(double rho, System.Random randomSource = null);
        Copula CreateTCopula(Matrix<double> rho, double dFreedom, System.Random randomSource = null);
        Copula CreateTCopula(double rho, double dFreedom, System.Random randomSource = null);
    }
    public interface IArchimedeanCopulaFactory
    {
        Copula CreateClaytonCopula(double rho, System.Random randomSource = null);
        Copula CreateGumbelCopula(double rho, System.Random randomSource = null);
    }
}