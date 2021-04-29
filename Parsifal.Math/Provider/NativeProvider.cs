using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Parsifal.Math
{
    /// <summary>
    /// C#原生算法
    /// </summary>
    /// <remarks>实现均不校验参数异常,如未空、输入输出长度不一致、无意义数；但会校验一些逻辑值，如乘法维度匹配等</remarks>
    internal class NativeProvider : ILogicProvider//设为internal阻止外部直接调用
    {
        public LogicProviderType Provider => LogicProviderType.Native;

        public void AddArray(double[] x, double[] y, double[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] + y[i];
            }
        }
        public void ScalarArray(double scalar, double[] source, double[] result)
        {
            if (scalar == 0d)
                Array.Clear(result, 0, result.Length);
            else if (scalar == 1d)
            {
                CopyToTarget(source, result);
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = scalar * source[i];
                }
            }
        }
        public void SubtractArray(double[] x, double[] y, double[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = x[i] - y[i];
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
            if (ParallelHelper.ShouldNotUseParallelism() || ShouldMatrixNotUseParallel(rowsX, columnsX) || ShouldMatrixNotUseParallel(rowsY, columnsY))
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
            {
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
        const int DoubleSize = sizeof(double);
        static double GetMatrixItem(double[] mat, int rows, int cols, int rowIndex, int colIndex)
        {
            return mat[rowIndex * cols + cols];//行主序
        }
        static void SetMatrixItem(double[] mat, int rows, int cols, int rowIndex, int colIndex, double value)
        {
            mat[rowIndex * cols + cols] = value;//行主序
        }
        static bool ShouldMatrixNotUseParallel(int rows, int cols)
        {
            return System.Math.Max(rows, cols) <= 32;
        }
        static void CopyToTarget(double[] source, double[] target)
        {
            Buffer.BlockCopy(source, 0, target, 0, source.Length * DoubleSize);
        }
        #endregion
    }
}
