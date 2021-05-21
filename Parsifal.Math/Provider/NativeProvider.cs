using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Parsifal.Math
{
    /// <summary>
    /// C#原生算法
    /// </summary>
    /// <remarks>实现均不校验参数异常,如参数为空、输入输出长度不一致、无意义数；但会校验一些逻辑值，如乘法维度匹配等</remarks>
    internal sealed class NativeProvider : ILogicProvider//设为internal阻止外部直接调用
    {
        public LogicProviderType Provider => LogicProviderType.Native;

        public void ArrayAdd(double scalar, double[] x, double[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = scalar + x[i];
            }
        }
        public void ArrayAdd(double[] x, double[] y, double[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] + y[i];
            }
        }
        public void ArraySubtract(double[] x, double[] y, double[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] - y[i];
            }
        }
        public void ArrayMultiply(double scalar, double[] x, double[] result)
        {
            if (scalar == 0d)
                Array.Clear(result, 0, result.Length);
            else if (scalar == 1d)
            {
                x.CopyToWithoutCheck(result);
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = scalar * x[i];
                }
            }
        }

        public void VectorDotProduct(double[] x, double[] y, double result)
        {
            if (x.Length != y.Length)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InconformityParameter);
            result = 0d;
            for (int i = 0; i < x.Length; i++)
            {
                result += x[i] * y[i];
            }
        }

        public void MatrixMultiply(int rowsX, int columnsX, double[] x, int rowsY, int columnsY, double[] y, double[] result)
        {
            if (columnsX != rowsY || rowsX * columnsY != result.Length)
                ThrowHelper.ThrowDimensionDontMatchException();
            if (ParallelHelper.ShouldNotUseParallelism() || MatrixShouldNotUseParallel(rowsX, columnsX) || MatrixShouldNotUseParallel(rowsY, columnsY))
            {
                for (int i = 0; i < rowsX; i++)
                {
                    for (int j = 0; j < columnsY; j++)
                    {
                        double temp = 0d;
                        for (int k = 0; k < columnsX; k++)
                        {
                            temp += GetMatrixItem(x, rowsX, columnsX, i, k) * GetMatrixItem(y, rowsY, columnsY, k, j);
                        }
                        SetMatrixItem(result, rowsX, columnsY, i, j, temp);
                    }
                }
            }
            else
            {//并行
                Parallel.ForEach(Partitioner.Create(0, rowsX, 1),
                    ParallelHelper.CreateParallelOptions(),
                    range =>
                    {
                        //double[] row;
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            //row=
                        }
                    });
            }
        }

        #region helper
        static double GetMatrixItem(double[] mat, int rows, int cols, int rowIndex, int colIndex)
        {
            return mat[colIndex * rows + rowIndex];//列主序
            //return mat[rowIndex * cols + colIndex];//行主序
        }
        static void SetMatrixItem(double[] mat, int rows, int cols, int rowIndex, int colIndex, double value)
        {
            mat[colIndex * rows + rowIndex] = value;//列主序
            //mat[rowIndex * cols + colIndex] = value;//行主序
        }
        static bool MatrixShouldNotUseParallel(int rows, int cols)
        {
            return System.Math.Max(rows, cols) <= 32;
        }
        static bool VectorShouldNotUseParallel(int length)
        {
            return length <= 1024;
        }
        #endregion
    }
}
