using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math.Algebra
{
    public partial class Matrix
    {
        /// <summary>
        /// 创建指定阶的空矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        public static Matrix Create(int order)
        {
            return Create(order, order);
        }
        /// <summary>
        /// 创建指定行列的空矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        public static Matrix Create(int rows, int columns)
        {
            return new Matrix(rows, columns);
        }
        /// <summary>
        /// 创建指定元素的矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="init">各元素初始化方法</param>
        public static Matrix Create(int rows, int columns, Func<int, int, double> init)
        {
            CheckValidRowAndColumn(rows, columns);
            if (init is null)
                ThrowHelper.ThrowArgumentNullException(nameof(init));
            double[] data = new double[rows * columns];
            for (int i = 0, index = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    data[index++] = init(i, j);
                }
            }
            return new Matrix(rows, columns, data, false);
        }
        /// <summary>
        /// 创建指定阶的对角线矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        /// <param name="init">对角线元素初始化方法</param>
        public static Matrix CreateDiagonal(int order, Func<int, double> init)
        {
            return CreateDiagonal(order, order, init);
        }
        /// <summary>
        /// 创建指定行列的对角线矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="init">对角线元素初始化方法</param>
        public static Matrix CreateDiagonal(int rows, int columns, Func<int, double> init)
        {
            if (rows < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rows));
            if (columns < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columns));
            if (init is null)
                ThrowHelper.ThrowArgumentNullException(nameof(init));
            double[] data = new double[rows * columns];
            int min = rows <= columns ? rows : columns;
            for (int i = 0, index = 0; i < min; i++, index += columns + 1)
            {
                data[index] = init(i);
            }
            return new Matrix(rows, columns, data, false);
        }
        /// <summary>
        /// 创建指定阶单位矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        public static Matrix CreateIdentity(int order)
        {
            if (order < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(order));
            double[] data = new double[order * order];
            for (int i = 0; i < order; i++)
            {
                data[i * (order + 1)] = 1d;
            }
            return new Matrix(order, order, data, false);
        }
        /// <summary>
        /// 创建指定阶、元素值随机的矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        /// <param name="minimum">元素最小值</param>
        /// <param name="maximum">元素最大值</param>
        public static Matrix CreateRandom(int order, double minimum, double maximum)
        {
            return CreateRandom(order, order, minimum, maximum);
        }
        /// <summary>
        /// 创建指定行列、元素值随机的矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="minimum">最小值</param>
        /// <param name="maximum">最大值</param>
        public static Matrix CreateRandom(int rows, int columns, double minimum, double maximum)
        {
            if (maximum < minimum)
                ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InvalidParameter, nameof(maximum));
            double range = maximum - minimum;
            var random = new Random(Guid.NewGuid().GetHashCode());
            double[] data = new double[rows * columns];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = random.NextDouble() * range + minimum;
            }
            return new Matrix(rows, columns, data, false);
        }
        /// <summary>
        /// 根据行主序数据创建矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="rowMajorData">行主序数据</param>
        public static Matrix CreateByRowMajorData(int rows, int columns, IEnumerable<double> rowMajorData)
        {
            CheckValidRowAndColumn(rows, columns);
            if (rowMajorData is null)
                ThrowHelper.ThrowArgumentNullException(nameof(rowMajorData));
            var enumerator = rowMajorData.GetEnumerator();
            double[] data = new double[rows * columns];
            for (int i = 0; enumerator.MoveNext() && i < data.Length; i++)
            {
                data[i] = enumerator.Current;
            }
            return new Matrix(rows, columns, data, false);
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix CreateByRows(IEnumerable<IEnumerable<double>> rowsData)
        {
            return CreateByRows(rowsData.Select(rs => (rs as double[]) ?? rs.ToArray()).ToArray());
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix CreateByRows(IEnumerable<double[]> rowsData)
        {
            return CreateByRows(rowsData as double[][] ?? rowsData.ToArray());
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix CreateByRows(params double[][] rowsData)
        {
            if (rowsData.Length < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowsData));
            int rows = rowsData.Length;
            int cols = rowsData[0].Length;
            double[] data = new double[rows * cols];
            for (int i = 0; i < rows; i++)
            {
                if (rowsData[i].Length < cols)
                    ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InconformityParameter, nameof(rowsData));
                Buffer.BlockCopy(rowsData[i], 0, data, i * cols * DoubleSize, cols * DoubleSize);
            }
            return new Matrix(rows, cols, data, false);
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix CreateByRows(IEnumerable<Vector> rowsData)
        {
            return CreateByRows(rowsData.ToArray());
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix CreateByRows(params Vector[] rowsData)
        {
            if (rowsData.Length < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowsData));
            int rows = rowsData.Length;
            int cols = rowsData[0].Dimension;
            double[] data = new double[rows * cols];
            for (int i = 0; i < rows; i++)
            {
                if (rowsData[i].Dimension < cols)
                    ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InconformityParameter, nameof(rowsData));
                Buffer.BlockCopy(rowsData[i].Storage, 0, data, i * cols * DoubleSize, cols * DoubleSize);
            }
            return new Matrix(rows, cols, data, false);
        }
        /// <summary>
        /// 根据列主序数据创建矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="columnMajorData">列主序数据</param>
        public static Matrix CreateByColumnMajorData(int rows, int columns, IEnumerable<double> columnMajorData)
        {
            CheckValidRowAndColumn(rows, columns);
            if (columnMajorData is null)
                ThrowHelper.ThrowArgumentNullException(nameof(columnMajorData));
            var enumerator = columnMajorData.GetEnumerator();
            double[] data = new double[rows * columns];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (enumerator.MoveNext())
                    {
                        data[j * columns + i] = enumerator.Current;
                    }
                }
            }
            return new Matrix(rows, columns, data, false);
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix CreateByColumns(IEnumerable<IEnumerable<double>> columnsData)
        {
            return CreateByColumns(columnsData.Select(cs => (cs as double[]) ?? cs.ToArray()).ToArray());
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix CreateByColumns(IEnumerable<double[]> columnsData)
        {
            return CreateByColumns(columnsData as double[][] ?? columnsData.ToArray());
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix CreateByColumns(params double[][] columnsData)
        {
            if (columnsData.Length < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnsData));
            int cols = columnsData.Length;
            int rows = columnsData[0].Length;
            double[] data = new double[rows * cols];
            for (int i = 0; i < cols; i++)
            {
                if (columnsData[i].Length < rows)
                    ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InconformityParameter, nameof(columnsData));
                for (int j = 0; j < rows; j++)
                {
                    data[j * cols + i] = columnsData[i][j];
                }
            }
            //for (int i = 0; i < rows; i++)
            //{
            //    for (int j = 0; j < cols; j++)
            //    {
            //        data[i * cols + j] = columnsData[j][i];
            //    }
            //}
            return new Matrix(rows, cols, data, false);
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix CreateByColumns(IEnumerable<Vector> columnsData)
        {
            return CreateByColumns(columnsData.ToArray());
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix CreateByColumns(params Vector[] columnsData)
        {
            if (columnsData.Length < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnsData));
            int cols = columnsData.Length;
            int rows = columnsData[0].Dimension;
            double[] data = new double[rows * cols];
            for (int i = 0; i < cols; i++)
            {
                if (columnsData[i].Dimension < rows)
                    ThrowHelper.ThrowIllegalArgumentException(ErrorReason.InconformityParameter, nameof(columnsData));
                for (int j = 0; j < rows; j++)
                {
                    data[j * cols + i] = columnsData[i].Get(j);
                }
            }
            return new Matrix(rows, cols, data, false);
        }
    }
}
