using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class tCopula:EllipticalCopula
    {
        public double DFreedom { get; protected set; }

        private tCopula() : base() { }

        public static IDFreedom Builder()
        {
            return new InternalBuilder();
        }

        public interface IDFreedom
        {
            ICorrelationType SetDFreedom(double dFreedom);
        }

        private class InternalBuilder : IBuild, ICorrelationType, IRho, IDFreedom
        {
            private readonly tCopula _instance = new tCopula();
            public InternalBuilder()
            {
            }
            public ICorrelationType SetDFreedom(double dFreedom)
            {
                _instance.DFreedom = dFreedom;
                _instance.SetTransformDist(tCopula.GetTransFormDist(dFreedom));
                return this;
            }
            public IRho SetCorrelationType(CorrelationType correlationType)
            {
                _instance.CorrelationType = correlationType;
                return this;
            }
            public IBuild SetRho(Matrix<double> rho)
            {
                var pearsonRho = EllipticalCopula.GetPearsonRho(rho, _instance.CorrelationType);
                _instance.Rho = pearsonRho;
                return this;
            }
            public IBuild SetRho(double rho)
            {
                var matrixRho = Copula.CreateCorrMatrixFromDouble(rho);
                return SetRho(matrixRho);
            }
            public IBuild SetRandomSource(System.Random randomSource)
            {
                _instance.RandomSource = randomSource;
                return this;
            }
            public Copula Build()
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