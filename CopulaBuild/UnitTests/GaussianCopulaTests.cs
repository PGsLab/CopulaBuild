// <copyright file="EllipticalCopulaTests.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
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
using System.Linq;
using MathNet.Numerics.Copulas;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;

namespace MathNet.Numerics.UnitTests.CopulaTests
{
    /// <summary>
    /// Gaussian Copula distribution tests.
    /// </summary>
    [TestFixture, Category("Copulas")]
    public class GaussianCopulaTests
    {
        private IEllipticalCopulaFactory _copulaFactory;

        [SetUp]
        public void RunBeforeAnyTests()
        {
            _copulaFactory = new FromPearsonCopulaFactory();
        }
        /// <summary>
        /// Can create Gaussian Copulas.
        /// </summary>
        /// <param name="rho">Rho value.</param>
        [TestCase(new double[] { 1, 0.5, 0.5, 1})]
        [TestCase(new double[] { 1, 0.5, 0.1, 0.5, 1, 0, 0.1, 0, 1})]
        public void CanCreateGaussian(double[] rho)
        {
            var size = (int)Math.Sqrt(rho.GetLength(0));
            var matrixRho = Matrix<double>.Build.DenseOfColumnMajor(size, size, rho);
            var g = _copulaFactory.CreateGaussianCopula(matrixRho);
            Assert.AreEqual(size, g.Dimension);
            Assert.AreEqual(matrixRho, g.Rho);
        }
        /// <summary>
        /// Can create Gaussian Copulas.
        /// </summary>
        /// <param name="rho">Rho value.</param>
        [TestCase( 0.5 )]
        public void CanCreateGaussianFromDouble(double rho)
        {
            var g = _copulaFactory.CreateGaussianCopula(rho);
            Assert.AreEqual(2, g.Dimension);
            Assert.AreEqual(rho, g.Rho[0, 1]);
            Assert.AreEqual(rho, g.Rho[1, 0]);
        }


        /// <summary>
        /// GaussianCopula create fails with bad parameter.
        /// </summary>
        /// <param name="rho">Rho value.</param>
        [TestCase(new double[] { 1, 0.5, 0.3, 1 })]
        [TestCase(new double[] { 1.1, 0.5, 0.1, 0.5, 1, 0, 0.1, 0, 1 })]
        public void GaussianCreateFailsWithBadParameters(double[] rho)
        {
            var size = (int)Math.Sqrt(rho.GetLength(0));
            var matrixRho = Matrix<double>.Build.DenseOfColumnMajor(size, size, rho);

            Assert.That(() => _copulaFactory.CreateGaussianCopula(matrixRho), Throws.ArgumentException);
        }

        /// <summary>
        /// GaussianCopula create fails with bad double parameter.
        /// </summary>
        /// <param name="rho">Rho value.</param>
        [TestCase(1.5)]
        [TestCase(-15)]
        [TestCase (Double.NaN)]
        public void GaussianCreateFailsWithBadDoubleParameters(double rho)
        {
            Assert.That(() => _copulaFactory.CreateGaussianCopula(rho), Throws.ArgumentException);
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        /// <param name="rho">Rho value.</param>
        [TestCase(new double[] { 1, 0.5, 0.5, 1 })]
        public void CanSample(double[] rho)
        {
            var size = (int)Math.Sqrt(rho.GetLength(0));
            var matrixRho = Matrix<double>.Build.DenseOfColumnMajor(size, size, rho);
            var g = _copulaFactory.CreateGaussianCopula(matrixRho);
            g.Sample();
        }
        /// <summary>
        /// Can sample 2 dimensional.
        /// </summary>
        /// <param name="rho">Rho value.</param>
        [TestCase( 0.5 )]
        public void CanSample2Dim(double rho)
        {
            var g = _copulaFactory.CreateGaussianCopula(rho);
            g.Sample();
        }

    }
}
