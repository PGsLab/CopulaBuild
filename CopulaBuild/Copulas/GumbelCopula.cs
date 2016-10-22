using System;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Copulas
{
    public class GumbelCopula : ArchimedeanCopula
    {
        public GumbelCopula(double theta, System.Random randomSource = null) : base(theta, randomSource)
        {
        }

        public override Matrix<double> Sample()
        {
            throw new System.NotImplementedException();
        }

        public override double Generator(double t)
        {
            return Math.Pow(-Math.Log(t),Theta);
        }

        public override double InverseGenerator(double t)
        {
            return Math.Exp(Math.Pow(-t,1/Theta));
        }

        public static double ThetafromKendall(double rho)
        {
            return 1 / (1 - rho);
        }
    }
}