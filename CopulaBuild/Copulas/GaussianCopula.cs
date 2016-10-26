// <copyright file="GaussianCopula.cs" company="Math.NET">
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

using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public class GaussianCopula : EllipticalCopula
    {
        private GaussianCopula() : base(GaussianCopula.GetTransFormDist()) { }

        public static ICorrelationType Builder()
        {
            return new InternalBuilder();
        }

        private class InternalBuilder : IBuild, ICorrelationType, IRho
        {
            private readonly GaussianCopula _instance = new GaussianCopula();
            public InternalBuilder() { }

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

            public IRho SetCorrelationType(CorrelationType correlationType)
            {
                _instance.CorrelationType = correlationType;
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

        private static IContinuousDistribution GetTransFormDist()
        {
            return new Normal(0, 1);
        }
    }
}