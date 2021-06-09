using System;
using Parsifal.Math.Algebra;

namespace Parsifal.Math
{
    public class CudaProvider : ILogicProvider
    {
        public LogicProviderType Provider => LogicProviderType.CUDA;

        public void ArrayAdd(double[] x, double[] y, double[] result)
        {
            throw new NotImplementedException();
        }

        public void ArrayAddScalar(double scalar, double[] x, double[] result)
        {
            throw new NotImplementedException();
        }

        public void ArrayMultiply(double scalar, double[] x, double[] result)
        {
            throw new NotImplementedException();
        }

        public void ArraySubtract(double[] x, double[] y, double[] result)
        {
            throw new NotImplementedException();
        }

        public void MatrixMultiply(int rowsX, int columnsX, double[] x, int rowsY, int columnsY, double[] y, double[] result)
        {
            throw new NotImplementedException();
        }

        public void MatrixMultiply(double alpha, int rowsX, int columnsX, double[] x, MatrixTranspose transposeX, int rowsY, int columnsY, double[] y, MatrixTranspose transposeY, double beta, double[] result)
        {
            throw new NotImplementedException();
        }

        public double VectorDotProduct(double[] x, double[] y)
        {
            throw new NotImplementedException();
        }
    }
}
