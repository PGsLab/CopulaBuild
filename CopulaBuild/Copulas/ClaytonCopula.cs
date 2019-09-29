using static System.Math;

namespace MathNet.Numerics.Copulas
{
    public class ClaytonCopula : ArchimedeanCopula
    {
        private ClaytonCopula()
        {
        }

        public ClaytonCopula(double theta, System.Random randomSource = null) : base(theta, randomSource)
        {
        }

        public override double[] Sample()
        {
            var result = new double[Dimension];
            result[0] = RandomSource.NextDouble();
            double indepentendUniform = RandomSource.NextDouble();
            if (Theta < MathNet.Numerics.Precision.MachineEpsilon)
                result[1] = indepentendUniform;
            else
                result[1] = result[0] * Pow(Pow(indepentendUniform, (-Theta) / (1 + Theta)) - 1 + Pow(result[0], Theta), (-1 / Theta));
            return result;
        }

        public override double Generator(double t)
        {
            return (1 / Theta) * (Pow(t, -Theta) - 1);
        }

        public override double InverseGenerator(double t)
        {
            return Pow(1 + Theta * t, -(1 / Theta));
        }

        public static IRankCorrelationType Builder()
        {
            return new ClaytonBuilder();
        }

        private class ClaytonBuilder : ArchimedeanBuilder
        {
            public ClaytonBuilder() : base(new ClaytonCopula())
            {
            }
        }

        public override double ThetafromKendall(double rho)
        {
            return 2 * rho / (1 - rho);
        }
    }
}