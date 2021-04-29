using System;

namespace Parsifal.Math
{
    public class CudaProvider : ILogicProvider
    {
        public LogicProviderType Provider => LogicProviderType.CUDA;

        public void AddArray(double[] x, double[] y, double[] result)
        {
            throw new NotImplementedException();
        }

        public void MatrixMultiply(int rowsX, int columnsX, double[] x, int rowsY, int columnsY, double[] y, double[] result)
        {
            throw new NotImplementedException();
        }

        public void ScalarArray(double scalar, double[] source, double[] result)
        {
            throw new NotImplementedException();
        }

        public void SubtractArray(double[] x, double[] y, double[] result)
        {
            throw new NotImplementedException();
        }

        public double VectorDorProduct(double[] x, double[] y)
        {
            throw new NotImplementedException();
        }

        public void VectorDotProduct(double[] x, double[] y, double result)
        {
            throw new NotImplementedException();
        }
    }
}
