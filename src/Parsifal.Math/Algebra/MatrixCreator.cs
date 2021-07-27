using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math.Algebra
{
    public class MatrixCreator
    {
        const int DoubleSize = sizeof(double);
        static void CheckValidRowAndColumn(int rows, int columns)
        {
            if (rows < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rows));
            if (columns < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columns));
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
            return new Matrix(order, order, data);
        }
        /// <summary>
        /// 创建指定阶的对角线矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        /// <param name="initFunc">对角线元素初始化方法</param>
        public static Matrix CreateDiagonal(int order, Func<int, double> initFunc)
        {
            return CreateDiagonal(order, order, initFunc);
        }
        /// <summary>
        /// 创建指定行列的对角线矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="initFunc">对角线元素初始化方法</param>
        public static Matrix CreateDiagonal(int rows, int columns, Func<int, double> initFunc)
        {
            CheckValidRowAndColumn(rows, columns);
            if (initFunc is null)
                ThrowHelper.ThrowArgumentNullException(nameof(initFunc));
            double[] data = new double[rows * columns];
            int min = rows <= columns ? rows : columns;
            for (int i = 0, index = 0; i < min; i++, index += (rows + 1))
            {
                data[index] = initFunc(i);
            }
            return new Matrix(rows, columns, data);
        }
        /// <summary>
        /// 创建指定元素的矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="initFunc">各元素初始化方法</param>
        public static Matrix CreateWithSpecify(int rows, int columns, Func<int, int, double> initFunc)
        {
            CheckValidRowAndColumn(rows, columns);
            if (initFunc is null)
                ThrowHelper.ThrowArgumentNullException(nameof(initFunc));
            double[] data = new double[rows * columns];
            for (int i = 0, offset = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    data[offset++] = initFunc(j, i);
                }
            }
            return new Matrix(rows, columns, data);
        }
        /// <summary>
        /// 根据二维数组创建对应矩阵
        /// </summary>
        /// <param name="array">二维数组</param>
        public static Matrix CreateByArray(in double[,] array)
        {
            if (array is null)
                ThrowHelper.ThrowArgumentNullException(nameof(array));
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            double[] data = new double[rows * cols];
            for (int i = 0, index = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    data[index++] = array[j, i];
                }
            }
            return new Matrix(rows, cols, data);
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
            double[] data = new double[rows * columns];
            double[] arrayData = rowMajorData.ToArray();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int sIndex = i * columns + j;
                    data[j * rows + i] = sIndex < arrayData.Length ? arrayData[sIndex] : 0;
                }
            }
            return new Matrix(rows, columns, data);
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
                int min = System.Math.Min(cols, rowsData[i].Length);
                for (int j = 0; j < min; j++)
                {
                    data[j * rows + i] = rowsData[i][j];
                }
            }
            return new Matrix(rows, cols, data);
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
            int cols = rowsData[0].Count;
            double[] data = new double[rows * cols];
            for (int i = 0; i < rows; i++)
            {
                int min = System.Math.Min(cols, rowsData[i].Count);
                for (int j = 0; j < min; j++)
                {
                    data[j * rows + i] = rowsData[i].Get(j);
                }
            }
            return new Matrix(rows, cols, data);
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
            double[] data = new double[rows * columns];
            double[] arrayData = columnMajorData.ToArray();
            Buffer.BlockCopy(arrayData, 0, data, 0, System.Math.Min(data.Length, arrayData.Length) * DoubleSize);
            return new Matrix(rows, columns, data);
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
                Buffer.BlockCopy(columnsData[i], 0,
                    data, i * rows * DoubleSize,
                    System.Math.Min(rows, columnsData[i].Length) * DoubleSize);
            }
            return new Matrix(rows, cols, data);
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
            int rows = columnsData[0].Count;
            double[] data = new double[rows * cols];
            for (int i = 0; i < cols; i++)
            {
                Buffer.BlockCopy(columnsData[i].Storage, 0,
                    data, i * rows * DoubleSize,
                    System.Math.Min(rows, columnsData[i].Count) * DoubleSize);
            }
            return new Matrix(rows, cols, data);
        }
    }
}
