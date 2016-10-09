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
    public class GaussianCopula : ICopula
    {
        public readonly Matrix<double> Rho;
        private int nrVariables;
        private System.Random _random = SystemRandomSource.Default;
        private MatrixNormal _matrixnormal;
        private Normal _normal;
        

        public GaussianCopula(Matrix<double> rho)
        {
            //ToDo implement checks
            Rho = rho;
            nrVariables = Rho.ColumnCount;
            var mu = Matrix<double>.Build.Sparse(nrVariables, 1);
            var k = Matrix<double>.Build.Sparse(1, 1);
            k[0, 0] = 1;
            _matrixnormal = new MatrixNormal(mu, Rho, k, _random);
            _normal = new Normal(0,1,_random);
        }
        public GaussianCopula(Matrix<double> rho, System.Random randomSource) : this(rho)
        {
            _random = randomSource ?? SystemRandomSource.Default;
        }

        public Matrix<double> Sample()
        {
            Matrix<double> result = Matrix<double>.Build.Dense(1, nrVariables);
            var mvnSample = _matrixnormal.Sample();
            for (var m = 0; m < nrVariables; ++m)
                result[1, m] = _normal.CumulativeDistribution(mvnSample[m, 0]);
            return result;
        }

        public Matrix<double> GetSamples(int nrSamples)
        {
            Matrix<double> result = Matrix<double>.Build.Dense(nrSamples, nrVariables);
            for (var n = 0; n < nrSamples; ++n)
            {
                var mvnSample = _matrixnormal.Sample();
                for (var m = 0; m < nrVariables; ++m)
                    result[n, m] = _normal.CumulativeDistribution(mvnSample[m,0]);
            }
            return result;
        }
    }
}