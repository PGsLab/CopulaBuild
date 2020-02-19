using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class tCopula:EllipticalCopula
    {
        /// <summary>
        /// Gets the degrees of freedom of the copula.
        /// </summary>
        public double DFreedom { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the tCopula class.
        /// </summary>
        /// <param name="dFreedom">The degrees of freedom of the copula. Range: dFreedom > 0.</param>
        /// <param name="rho">The underlying correlation matrix. Must be symmetric and positive semi-definite.</param>
        /// <param name="correlationType">The correlation type of the given correlation matrix.</param>
        /// <param name="randomSource">The random number generator which is used to draw random samples.</param>
        public tCopula(double dFreedom, Matrix<double> rho, CorrelationType correlationType, RandomSource randomSource = null) : base(rho, correlationType, randomSource, tCopula.CreateTransformDist(dFreedom))
        {
            DFreedom = dFreedom;
        }
        private tCopula() : base() { }

        /// <summary>
        /// Initializes a new instance of the InternalBuilder class to parametrize a t Copula.
        /// </summary>
        /// <returns>an InternalBuilder object to build a t Copula.</returns>
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
                _instance._transformDist = tCopula.CreateTransformDist(dFreedom);
                return this;
            }
            public IRho SetCorrelationType(CorrelationType correlationType)
            {
                _instance.CorrelationType = correlationType;
                return this;
            }
            public IBuild SetRho(Matrix<double> rho)
            {
                var pearsonRho = EllipticalCopula.ConvertToPearsonLinearCorrelationMatrix(rho, _instance.CorrelationType);
                _instance.Rho = pearsonRho;
                return this;
            }
            public IBuild SetRho(double rho)
            {
                var matrixRho = Copula.CreateCorrelationMatrixFromDouble(rho);
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
        private static IContinuousDistribution CreateTransformDist(double dfreedom)
        {
            return new StudentT(0.0, 1.0, dfreedom);
        }
    }
}