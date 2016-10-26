using System;
using MathNet.Numerics.Copulas;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Copulas
{
    public interface ICorrelationType
    {
        IRho SetCorrelationType(CorrelationType correlationType);
    }
    public interface IRankCorrelationType
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
        Copula Build();
    }

}