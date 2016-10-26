using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class TCopula:EllipticalCopula
    {
        public double DFreedom { get; protected set; }

        private TCopula() : base() { }

        public static ICorrelationType Builder()
        {
            return new InternalBuilder();
        }
        public interface ICorrelationType
        {
            IRho SetCorrelationType(CorrelationType correlationType);
        }
        public interface IRho
        {
            IDFreedom SetRho(Matrix<double> rho);
        }
        public interface IDFreedom
        {
            IBuild SetDFreedom(double dFreedom);
        }
        public interface IBuild
        {
            IBuild SetRandomSource(System.Random randomSource);
            TCopula Build();
        }

        private class InternalBuilder : IBuild, ICorrelationType, IRho, IDFreedom
        {
            private readonly TCopula _instance = new TCopula();
            public InternalBuilder()
            {
            }
            public IRho SetCorrelationType(CorrelationType correlationType)
            {
                _instance.CorrelationType = correlationType;
                return this;
            }
            public IDFreedom SetRho(Matrix<double> rho)
            {
                var pearsonRho = EllipticalCopula.GetPearsonRho(rho, _instance.CorrelationType);
                _instance.Rho = pearsonRho;
                return this;
            }
            public IBuild SetDFreedom(double dFreedom)
            {
                _instance.DFreedom = dFreedom;
                _instance.SetTransformDist(TCopula.GetTransFormDist(dFreedom));
                return this;
            }
            public IBuild SetRandomSource(System.Random randomSource)
            {
                _instance.RandomSource = randomSource;
                return this;
            }
            public TCopula Build()
            {
                return _instance;
            }
        }
        private static IContinuousDistribution GetTransFormDist(double dfreedom)
        {
            return new StudentT(0.0, 1.0, dfreedom);
        }
    }
}