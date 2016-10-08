using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Copulas
{
    public interface ICopula
    {
        Matrix<double> GetSamples(int nrSamples);
    }
}
