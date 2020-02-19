// <copyright file="Copula.cs" company="Math.NET">
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
using System;
using System.Collections.Generic;

namespace MathNet.Numerics.Copulas
{
    public abstract partial class Copula
    {
        protected System.Random _randomSource = SystemRandomSource.Default;

        /// <summary>
        /// Gets the underlying correlation matrix of the copula.
        /// </summary>
        public virtual Matrix<double> Rho { get; protected set; }

        /// <summary>
        /// Gets the dimension of the underlying correlation matrix of the copula.
        /// </summary>
        public int Dimension { get { return Rho.RowCount; } }

        /// <summary>
        /// Gets the random number generator which is used to draw random samples.
        /// </summary>
        public virtual System.Random RandomSource
        {
            get { return _randomSource; }
            set { _randomSource = value ?? SystemRandomSource.Default; }
        }

        /// <summary>
        /// Gets the correlation type the underlying correlation matrix of the copula.
        /// </summary>
        public CorrelationType CorrelationType { get; protected set; }

        /// <summary>
        /// Generates a correlated uniform sample corresponding to the parametrized copula.
        /// </summary>
        /// <returns>a correlated sample from the copula.</returns>
        public abstract double[] Sample();

        /// <summary>
        /// Generates a sequence of correlated uniform samples corresponding to the parametrized copula.
        /// </summary>
        /// <param name="nrSamples">The desired number of samples.</param>
        /// <returns>a sequence of correlated samples from the copula.</returns>
        public IEnumerable<double[]> Samples(int nrSamples)
        {
            for (var n = 0; n < nrSamples; ++n)
                yield return Sample();
        }

        /// <summary>
        /// Generates a matrix filled with correlated uniform samples corresponding to the parametrized copula.
        /// </summary>
        /// <param name="nrSamples">The desired number of samples.</param>
        /// <returns>a matrix of correlated samples from the copula, where each row corresponds to one uniform sample.</returns>
        public Matrix<double> GetSampleMatrix(int nrSamples)
        {
            Matrix<double> result = Matrix<double>.Build.Dense(nrSamples, Dimension);
            for (var n = 0; n < nrSamples; ++n)
                result.SetRow(n, Sample());
            return result;
        }

        /// <summary>
        /// Tests whether the provided values are valid parameters for the copula.
        /// </summary>
        /// <param name="rho">The correlation matrix.</param>
        public static bool IsValidParameterSet(Matrix<double> rho)
        {
            var n = rho.RowCount;
            var p = rho.ColumnCount;

            for (var i = 0; i < rho.RowCount; i++)
            {
                if (!rho.At(i, i).Equals(1.0))
                    return false;
            }

            for (var i = 0; i < rho.RowCount; i++)
            {
                for (var j = i + 1; j < rho.ColumnCount; j++)
                {
                    if (!rho.At(i, j).AlmostEqual(rho.At(j, i)) || rho.At(i, j) <= -1 || rho.At(i, j) >= 1)
                        return false;
                }
            }
            return true;
        }

        protected static Matrix<double> CreateCorrelationMatrixFromDouble(double rho)
        {
            var result = Matrix<double>.Build.DenseDiagonal(2, 2, 1);
            result[0, 1] = rho;
            result[1, 0] = rho;
            return result;
        }
    }
}