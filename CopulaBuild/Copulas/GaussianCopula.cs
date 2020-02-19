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
        /// <summary>
        /// Initializes a new instance of the GaussianCopula class.
        /// </summary>
        /// <param name="rho">The underlying correlation matrix. Must be symmetric and positive semi-definite.</param>
        /// <param name="correlationType">The correlation type of the given correlation matrix.</param>
        /// <param name="randomSource">The random number generator which is used to draw random samples.</param>
        public GaussianCopula(Matrix<double> rho, CorrelationType correlationType, RandomSource randomSource = null) : base(rho, correlationType, randomSource, GaussianCopula.CreateTransformDist())
        {
            
        }
        private GaussianCopula() : base(GaussianCopula.CreateTransformDist()) { }

        /// <summary>
        /// Initializes a new instance of the InternalBuilder class to parametrize a Gaussian Copula.
        /// </summary>
        /// <returns>an InternalBuilder object to build a Gaussian Copula.</returns>
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
                var pearsonRho = EllipticalCopula.ConvertToPearsonLinearCorrelationMatrix(rho, _instance.CorrelationType);
                _instance.Rho = pearsonRho;
                return this;
            }

            public IBuild SetRho(double rho)
            {
                var matrixRho = Copula.CreateCorrelationMatrixFromDouble(rho);
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

        private static IContinuousDistribution CreateTransformDist()
        {
            return new Normal(0, 1);
        }
    }
}