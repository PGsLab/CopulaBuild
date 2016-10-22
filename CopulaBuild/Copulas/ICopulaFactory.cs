using System.Runtime.InteropServices;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public interface ICopulaFactory
    {
        Copula CreateGaussianCopula(Matrix<double> rho, System.Random randomSource = null);
        Copula CreateTCopula(Matrix<double> rho, double dFreedom, System.Random randomSource = null);
    }
}