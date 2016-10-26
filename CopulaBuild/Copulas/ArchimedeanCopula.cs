// <copyright file="ArchimedeanCopula.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// 
// Copyright (c) 2009-2016 Math.NET
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public abstract class ArchimedeanCopula : Copula
    {
        public double Theta;
        public abstract double Generator(double t);
        public abstract double InverseGenerator(double t);
        public abstract double ThetafromKendall(double rho);
        public sealed override System.Random RandomSource
        {
            get { return base.RandomSource; }
            protected set { base.RandomSource = value; }
        }

        protected ArchimedeanCopula()
        {
            Dimension = 2;
        }
        protected ArchimedeanCopula(double theta, System.Random randomSource = null)
        {
            Dimension = 2;
            Theta = theta;
            RandomSource = randomSource ?? SystemRandomSource.Default;
        }
        protected class ArchimedeanBuilder : IBuild, IRankCorrelationType, IRho
        {
            private readonly ArchimedeanCopula _instance;

            public ArchimedeanBuilder(ArchimedeanCopula archimedeanCopula)
            {
                _instance = archimedeanCopula;
            }

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
                    _instance.Theta = _instance.ThetafromKendall(rho[0, 1]);
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
                    _instance.Theta = _instance.ThetafromKendall(rho);
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
            public Copula Build()
            {
                return _instance;
            }
        }
    }
}