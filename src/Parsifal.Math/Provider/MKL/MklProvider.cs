using Parsifal.Math.Algebra;

namespace Parsifal.Math.Provider.MKL
{
    internal class MklProvider : ILogicProvider
    {
        public LogicProviderType Provider => LogicProviderType.MKL;

        public void ArrayAddScalar(double scalar, double[] x, double[] result)
        {

        }

        public void ArrayAdd(double[] x, double[] y, double[] result)
        {
            MklNativeMethod.dVectorAdd(x, y, x.Length, result);
        }

        public void ArrayMultiply(double scalar, double[] x, double[] result)
        {
            x.CopyToWithoutCheck(result);
            MklNativeMethod.dVectorScalarProduct(result, result.Length, scalar);
        }

        public void ArraySubtract(double[] x, double[] y, double[] result)
        {
            MklNativeMethod.dVectorSub(x, y, x.Length, result);
        }

        public void MatrixMultiply(int rowsX, int columnsX, double[] x, int rowsY, int columnsY, double[] y, double[] result)
        {
            if (columnsX != rowsY || rowsX * columnsY != result.Length)
                ThrowHelper.ThrowDimensionDontMatchException();
            MklNativeMethod.dMatrixMatrixProduct(CBLAS_TRANSPOSE.CblasNoTrans, CBLAS_TRANSPOSE.CblasNoTrans, rowsX, columnsX, columnsY, x, y, 1, 0, result);
        }

        public double VectorDotProduct(double[] x, double[] y)
        {
            return MklNativeMethod.dVectorDotProduct(x, y, x.Length);
        }

        public void MatrixMultiply(double alpha, int rowsX, int columnsX, double[] x, MatrixTranspose transposeX, int rowsY, int columnsY, double[] y, MatrixTranspose transposeY, double beta, double[] result)
        {
            throw new System.NotImplementedException();
        }
    }
}
