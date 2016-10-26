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

using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Copulas
{
    public abstract class Copula
    {
        private System.Random _randomSource = SystemRandomSource.Default;
        public virtual Matrix<double> Rho { get; protected set; }
        public int Dimension { get; protected set; }

        public virtual System.Random RandomSource
        {
            get { return _randomSource; }
            protected set { _randomSource = value; }
        }

        public CorrelationType CorrelationType { get; protected set; }

        public abstract Matrix<double> Sample();
        public Matrix<double> GetSamples(int nrSamples)
        {
            Matrix<double> result = Matrix<double>.Build.Dense(nrSamples, Dimension);
            for (var n = 0; n < nrSamples; ++n)
            {
                var singleSim = Sample();
                for (var m = 0; m < Dimension; ++m)
                    result[n, m] = singleSim[0, m];
            }
            return result;
        }

        /// <summary>
        /// Tests whether the provided values are valid parameters for these Copulas.
        /// </summary>
        /// <param name="rho">The correlation matrix.</param>
        public static bool IsValidParameterSet(Matrix<double> rho)
        {
            var n = rho.RowCount;
            var p = rho.ColumnCount;

            for (var i = 0; i < rho.RowCount; i++)
            {
                if (!rho.At(i, i).Equals(1.0))
                {
                    return false;
                }
            }

            for (var i = 0; i < rho.RowCount; i++)
            {
                for (var j = i + 1; j < rho.ColumnCount; j++)
                {
                    if (!rho.At(i, j).AlmostEqual(rho.At(j, i)) || rho.At(i, j) <= -1 || rho.At(i, j) >= 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static Matrix<double> CreateCorrMatrixFromDouble(double rho)
        {
            var result = Matrix<double>.Build.DenseDiagonal(2, 2, 1);
            result[0, 1] = rho;
            result[1, 0] = rho;
            return result;
        }

        [Serializable]
        public class InvalidCorrelationMatrixException : ArgumentException
        {
            public InvalidCorrelationMatrixException()
            {
            }

            public InvalidCorrelationMatrixException(string message)
                : base(message)
            {
            }

            public InvalidCorrelationMatrixException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}