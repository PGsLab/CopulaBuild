using MathNet.Numerics.Copulas;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using MathNet.Numerics.Statistics;
using NUnit.Framework;
using System;
using System.Linq;

namespace CopulaTests
{
    /// <summary>
    /// This class will perform various tests on copulas.
    /// </summary>
    [TestFixture, Category("Copulas")]
    public class CommonCopulaTests
    {
        public const int NumberOfTestSamples = 3500000;
        public const int NumberOfHistogramBuckets = 100;
        public const double ErrorTolerance = 0.01;
        public const double ErrorProbability = 0.001;

        private static readonly Copula[] copulas =
        {
            new GaussianCopula(Matrix<double>.Build.DenseOfColumnMajor(3, 3, new double[] { 1, 0.5, 0.1, 0.5, 1, 0, 0.1, 0, 1 }), CorrelationType.SpearmanRank),
            new tCopula(4.5, Matrix<double>.Build.DenseOfColumnMajor(3, 3, new double[] { 1, 0.5, 0.1, 0.5, 1, 0, 0.1, 0, 1 }), CorrelationType.SpearmanRank)
        };

        [Test, TestCaseSource(nameof(copulas))]
        public void HasRandomSourceContinuous(Copula copula)
        {
            Assert.IsNotNull(copula.RandomSource);
        }

        [Test, TestCaseSource(nameof(copulas))]
        public void CanSetRandomSourceContinuous(Copula copula)
        {
            copula.RandomSource = MersenneTwister.Default;
        }

        [Test, TestCaseSource(nameof(copulas))]
        public void HasRandomSourceEvenAfterSetToNullContinuous(Copula copula)
        {
            Assert.DoesNotThrow(() => copula.RandomSource = null);
            Assert.IsNotNull(copula.RandomSource);
        }

        [Test, Category("LongRunning"), TestCaseSource(nameof(copulas))]
        public void SampleIsDistributedCorrectlyContinuous(Copula copula)
        {
            copula.RandomSource = new SystemRandomSource(1, false);
            var samples = copula.GetSampleMatrix(NumberOfTestSamples);
            for (var j = 0; j < samples.ColumnCount; ++j)
            {
                ContinuousVapnikChervonenkisTest(ErrorTolerance, ErrorProbability, samples.Column(j).ToArray(), copula);
            }
        }

        [Test, Category("LongRunning"), TestCaseSource(nameof(copulas))]
        public void SampleSequenceIsDistributedCorrectlyContinuous(Copula copula)
        {
            copula.RandomSource = new SystemRandomSource(1, false);
            var samples = copula.Samples(NumberOfTestSamples).ToArray();
            var transformedSamples = Matrix<double>.Build.DenseOfRowArrays(samples);
            for (var j = 0; j < transformedSamples.ColumnCount; ++j)
            {
                ContinuousVapnikChervonenkisTest(ErrorTolerance, ErrorProbability, transformedSamples.Column(j).ToArray(), copula);
            }
        }

        /// <summary>
        /// Vapnik Chervonenkis test.
        /// </summary>
        /// <param name="epsilon">The error we are willing to tolerate.</param>
        /// <param name="delta">The error probability we are willing to tolerate.</param>
        /// <param name="s">The samples to use for testing.</param>
        /// <param name="copula">The distribution we are testing.</param>
        public static void ContinuousVapnikChervonenkisTest(double epsilon, double delta, double[] s, Copula copula)
        {
            // Using VC-dimension, we can bound the probability of making an error when estimating empirical probability
            // distributions. We are using Theorem 2.41 in "All Of Nonparametric Statistics".
            // http://books.google.com/books?id=MRFlzQfRg7UC&lpg=PP1&dq=all%20of%20nonparametric%20statistics&pg=PA22#v=onepage&q=%22shatter%20coe%EF%AC%83cients%20do%20not%22&f=false .</para>
            // For intervals on the real line the VC-dimension is 2.
            Assert.Greater(s.Length, Math.Ceiling(32.0 * Math.Log(16.0 / delta) / epsilon / epsilon));

            var histogram = new Histogram(s, NumberOfHistogramBuckets);
            for (var i = 0; i < NumberOfHistogramBuckets; i++)
            {
                var p = histogram[i].UpperBound - histogram[i].LowerBound;
                var pe = histogram[i].Count / (double)s.Length;
                Assert.Less(Math.Abs(p - pe), epsilon, copula.ToString());
            }
        }
    }
}