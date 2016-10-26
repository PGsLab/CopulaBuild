using System;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Copulas
{
    public class ClaytonCopula : ArchimedeanCopula
    {
        private ClaytonCopula() { }
        public ClaytonCopula(double theta, System.Random randomSource = null) : base(theta, randomSource) { }

        public override Matrix<double> Sample()
        {
            Matrix<double> result = Matrix<double>.Build.Dense(1, Dimension);
            result[0, 0] = RandomSource.NextDouble();
            double indepentendUniform = RandomSource.NextDouble();
            if (Theta < MathNet.Numerics.Precision.MachineEpsilon)
                result[0, 1] = indepentendUniform;
            else
            {
                result[0, 1] = result[0, 0]*
                               Math.Pow(Math.Pow(indepentendUniform, (-Theta)/(1 + Theta)) - 1 + Math.Pow(result[0, 0], Theta),(-1 / Theta));
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

        public static ICorrelationType Builder()
        {
            return new InternalBuilder();
        }
        public interface ICorrelationType
        {
            IRho SetCorrelationType(RankCorrelationType correlationType);
        }
        public interface IRho
        {
            IBuild SetRho(Matrix<double> rho);
            IBuild SetRho(double rho);
        }
        public interface IBuild
        {
            IBuild SetRandomSource(System.Random randomSource);
            ClaytonCopula Build();
        }

        private class InternalBuilder : IBuild, ICorrelationType, IRho
        {
            private readonly ClaytonCopula _instance = new ClaytonCopula();
            public InternalBuilder() { }

            public IRho SetCorrelationType(RankCorrelationType correlationType)
            {
                _instance.CorrelationType = (CorrelationType)correlationType;
                return this;
            }
            public IBuild SetRho(Matrix<double> rho)
            {
                if (!IsValidParameterSet(rho) || rho.RowCount != 2)
                {
                    throw new Copula.InvalidCorrelationMatrixException();
                }
                if (_instance.CorrelationType == CorrelationType.KendallRank)
                    _instance.Theta = ThetafromKendall(rho[0,1]);
                else
                {
                    throw new System.NotImplementedException();
                }
                _instance.Rho = rho;
                return this;
            }
            public IBuild SetRho(double rho)
            {
                var matrixRho = Copula.CreateCorrMatrixFromDouble(rho);
                if (!IsValidParameterSet(matrixRho))
                {
                    throw new Copula.InvalidCorrelationMatrixException();
                }
                if (_instance.CorrelationType == CorrelationType.KendallRank)
                    _instance.Theta = ThetafromKendall(rho);
                else
                {
                    throw new System.NotImplementedException();
                }
                _instance.Rho = matrixRho;
                return this;
            }
            public IBuild SetRandomSource(System.Random randomSource)
            {
                _instance.RandomSource = randomSource;
                return this;
            }
            public ClaytonCopula Build()
            {
                return _instance;
            }
        }

        public static double ThetafromKendall(double rho)
        {
            return 2 * rho / (1 - rho);
        }
    }
}