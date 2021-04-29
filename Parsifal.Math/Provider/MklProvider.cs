using System;

namespace Parsifal.Math
{
    internal class MklProvider //: ILogicProvider
    {
        public LogicProviderType Provider => LogicProviderType.MKL;

        public void MatrixMultiply(int rowsX, int columnsX, double[] x, int rowsY, int columnsY, double[] y, double[] result)
        {
            throw new NotImplementedException();
        }
    }
}
