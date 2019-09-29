using static System.Math;

namespace MathNet.Numerics.Copulas
{
    public class GumbelCopula : ArchimedeanCopula
    {
        private GumbelCopula()
        {
        }

        public GumbelCopula(double theta, System.Random randomSource = null) : base(theta, randomSource)
        {
        }

        public override double[] Sample()
        {
            throw new System.NotImplementedException();
        }

        public override double Generator(double t)
        {
            return Pow(-Log(t), Theta);
        }

        public override double InverseGenerator(double t)
        {
            return Exp(Pow(-t, 1 / Theta));
        }

        public static IRankCorrelationType Builder()
        {
            return new GumbelBuilder();
        }

        private class GumbelBuilder : ArchimedeanBuilder
        {
            public GumbelBuilder() : base(new GumbelCopula())
            {
            }
        }

        public override double ThetafromKendall(double rho)
        {
            return 1 / (1 - rho);
        }
    }
}