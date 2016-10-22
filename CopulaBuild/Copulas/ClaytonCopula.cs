using System;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Copulas
{
    public class ClaytonCopula : ArchimedeanCopula
    {
        public ClaytonCopula(double theta)
        {
            Theta = theta;
        }
        public override Matrix<double> Sample()
        {
            Matrix<double> result = Matrix<double>.Build.Dense(1, Dimension);
            result[0, 0] = RandomSource.NextDouble();
            double indepentendUniform = RandomSource.NextDouble();
            if (Theta < MathNet.Numerics.Precision.MachineEpsilon)
                result[0, 1] = indepentendUniform;
            else
            {
                result[0, 1] = result[0, 1]*
                               Math.Pow(Math.Pow(indepentendUniform, (-Theta)/(1 + Theta)) - 1 + Math.Pow(result[0, 1], Theta),(-1 / Theta));
            }
            return result;
        }

        public override double Generator(double t)
        {
            return (1/Theta)*(Math.Pow(t, -Theta) - 1);
        }

        public override double InverseGenerator(double t)
        {
            return Math.Pow(1 + Theta * t, -(1/Theta));
        }
    }
}