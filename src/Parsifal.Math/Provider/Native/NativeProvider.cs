using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Parsifal.Math.Algebra;

namespace Parsifal.Math.Provider.Native
{
    /// <summary>
    /// C#原生算法
    /// </summary>
    /// <remarks>实现均不校验参数异常,如参数为空、输入输出长度不一致、无意义数；但会校验一些逻辑值，如乘法维度匹配等</remarks>
    internal sealed class NativeProvider : ILinearAlgebraProvider//设为internal阻止外部直接调用
    {
        public LogicProviderType ProviderType => LogicProviderType.Native;

        public void ArrayAddScalar(double scalar, double[] x, double[] result)
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

        public double VectorDotProduct(double[] x, double[] y)
        {
            if (x.Length != y.Length)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InconformityParameter);
            double result = 0d;
            for (int i = 0; i < x.Length; i++)
            {
                result += x[i] * y[i];
            }
            return result;
        }

        public void MatrixMultiply(int rowsX, int columnsX, double[] x, int rowsY, int columnsY, double[] y, double[] result)
        {
            if (columnsX != rowsY || rowsX * columnsY != result.Length)
                ThrowHelper.ThrowDimensionDontMatchException();
            if (ParallelHelper.ShouldNotUseParallelism() || MatrixShouldNotUseParallel(rowsX, columnsX) || MatrixShouldNotUseParallel(rowsY, columnsY))
            {//直算
                for (int i = 0; i < rowsX; i++)
                {
                    for (int j = 0; j < columnsY; j++)
                    {
                        double temp = 0d;
                        for (int k = 0; k < columnsX; k++)
                        {
                            temp += x[k * rowsX + i] * y[j * rowsY + k];
                        }
                        result[j * rowsX + i] = temp;
                    }
                }
            }
            else
            {//并行
                Parallel.ForEach(Partitioner.Create(0, rowsX, 1),//按左矩阵的每行来分块
                    ParallelHelper.CreateParallelOptions(),
                    range =>
                    {
                        double[] xRow = new double[columnsX];
                        double[] yCol = new double[rowsY];
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            LinearAlgebraHelper.GetRow(rowsX, columnsX, x, MatrixMajorOrder.Column, MatrixTranspose.NotTranspose, i, xRow);
                            for (int j = 0; j < columnsY; j++)
                            {
                                LinearAlgebraHelper.GetColumn(rowsY, columnsY, y, MatrixMajorOrder.Column, MatrixTranspose.NotTranspose, j, yCol);
                                double temp = 0d;
                                for (int k = 0; k < columnsX; k++)
                                {
                                    temp += xRow[k] * yCol[k];
                                }
                                result[j * rowsX + i] = temp;
                            }
                        }
                    });
            }
        }
        public void MatrixMultiply(double alpha, int rowsX, int columnsX, double[] x, MatrixTranspose transposeX, int rowsY, int columnsY, double[] y, MatrixTranspose transposeY, double beta, double[] result)
        {
            if (transposeX != MatrixTranspose.NotTranspose)
                Swap(ref rowsX, ref columnsX);
            if (transposeY != MatrixTranspose.NotTranspose)
                Swap(ref rowsY, ref columnsY);
            if (columnsX != rowsY || rowsX * columnsY != result.Length)
                ThrowHelper.ThrowDimensionDontMatchException();
            ArrayMultiply(beta, result, result);
            if (beta == 0d)
                Array.Clear(result, 0, result.Length);
            else if (beta != 1d)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = beta * result[i];
                }
            }
            if (alpha == 0d)
                return;
            if (ParallelHelper.ShouldNotUseParallelism() || MatrixShouldNotUseParallel(rowsX, columnsX) || MatrixShouldNotUseParallel(rowsY, columnsY))
            {//直算
                double[] xRow = new double[columnsX];
                double[] yCol = new double[rowsY];
                for (int i = 0; i < rowsX; i++)
                {
                    LinearAlgebraHelper.GetRow(rowsX, columnsX, x, MatrixMajorOrder.Column, transposeX, i, xRow);
                    for (int j = 0; j < columnsY; j++)
                    {
                        LinearAlgebraHelper.GetColumn(rowsY, columnsY, y, MatrixMajorOrder.Column, transposeY, j, yCol);
                        double temp = 0d;
                        for (int k = 0; k < columnsX; k++)
                        {
                            temp += xRow[k] * yCol[k];
                        }
                        result[j * rowsX + i] += alpha * temp;
                    }
                }
            }
            else
            {//并行
                Parallel.ForEach(Partitioner.Create(0, rowsX, 1),
                    ParallelHelper.CreateParallelOptions(),
                    range =>
                    {
                        double[] xRow = new double[columnsX];
                        double[] yCol = new double[rowsY];
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            LinearAlgebraHelper.GetRow(rowsX, columnsX, x, MatrixMajorOrder.Column, transposeX, i, xRow);
                            for (int j = 0; j < columnsY; j++)
                            {
                                LinearAlgebraHelper.GetColumn(rowsY, columnsY, y, MatrixMajorOrder.Column, transposeY, j, yCol);
                                double temp = 0d;
                                for (int k = 0; k < columnsX; k++)
                                {
                                    temp += xRow[k] * yCol[k];
                                }
                                result[j * rowsX + i] += alpha * temp;
                            }
                        }
                    });
            }
        }

        #region helper
        static bool MatrixShouldNotUseParallel(int rows, int cols)
        {
            /* CPU: Intel Core i7-7700K @4.2GHz 8核
             * Memory: 32G @3200MHz
             * 在运算方阵乘法时的运行效率对比
             * Order    直算Time    并行Time
             * 32       34ms        36ms
             * 50       27ms        26ms
             * 64       28ms        27ms
             * 100      32ms        27ms
             * 200      76ms        42ms
             * 500      702ms       201ms
             * 1000     6.1s        1.3s
             * 可以看到在100阶时并行运算已有一定的效率优势
             * 为保证并行算法适应更多型处理器，在较弱处理器上有良好性能，将最小阶设为32
             */
            return System.Math.Max(rows, cols) < 32;
        }
        static void Swap(ref int first, ref int second)
        {
            int temp = first;
            first = second;
            second = temp;
        }
        #endregion
    }
}
