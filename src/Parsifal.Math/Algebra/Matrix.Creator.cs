using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsifal.Math.Algebra
{
    partial class Matrix<T>
    {
        /// <summary>
        /// 创建指定阶单位矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        public Matrix<T> CreateIdentity(int order)
        {
            if (order < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(order));
            T[] data = new T[order * order];
            for (int i = 0; i < order; i++)
            {
                data[i * (order + 1)] = One;
            }
            return new Matrix<T>(order, order, data);
        }
        /// <summary>
        /// 创建指定阶的对角线矩阵
        /// </summary>
        /// <param name="order">矩阵阶数</param>
        /// <param name="initFunc">对角线元素初始化方法</param>
        public Matrix<T> CreateDiagonal(int order, Func<int, T> initFunc)
        {
            return CreateDiagonal(order, order, initFunc);
        }
        /// <summary>
        /// 创建指定行列的对角线矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="initFunc">对角线元素初始化方法</param>
        public Matrix<T> CreateDiagonal(int rows, int columns, Func<int, T> initFunc)
        {
            CheckValidRowAndColumn(rows, columns);
            if (initFunc is null)
                ThrowHelper.ThrowArgumentNullException(nameof(initFunc));
            T[] data = new T[rows * columns];
            int min = rows <= columns ? rows : columns;
            for (int i = 0, index = 0; i < min; i++, index += (rows + 1))
            {
                data[index] = initFunc(i);
            }
            return new Matrix<T>(rows, columns, data);
        }
        /// <summary>
        /// 创建指定元素的矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="initFunc">各元素初始化方法</param>
        public Matrix<T> CreateWithSpecify(int rows, int columns, Func<int, int, T> initFunc)
        {
            CheckValidRowAndColumn(rows, columns);
            if (initFunc is null)
                ThrowHelper.ThrowArgumentNullException(nameof(initFunc));
            T[] data = new T[rows * columns];
            for (int i = 0, offset = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    data[offset++] = initFunc(j, i);
                }
            }
            return new Matrix<T>(rows, columns, data);
        }
        /// <summary>
        /// 根据二维数组创建对应矩阵
        /// </summary>
        /// <param name="array">二维数组</param>
        public static Matrix<T> CreateByArray(in T[,] array)
        {
            if (array is null)
                ThrowHelper.ThrowArgumentNullException(nameof(array));
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            T[] data = new T[rows * cols];
            for (int i = 0, index = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    data[index++] = array[j, i];
                }
            }
            return new Matrix<T>(rows, cols, data);
        }
        /// <summary>
        /// 根据行主序数据创建矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="rowMajorData">行主序数据</param>
        public static Matrix<T> CreateByRowMajorData(int rows, int columns, IEnumerable<T> rowMajorData)
        {
            CheckValidRowAndColumn(rows, columns);
            if (rowMajorData is null)
                ThrowHelper.ThrowArgumentNullException(nameof(rowMajorData));
            T[] data = new T[rows * columns];
            T[] arrayData = rowMajorData.ToArray();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int sIndex = i * columns + j;
                    data[j * rows + i] = sIndex < arrayData.Length ? arrayData[sIndex] : Zero;
                }
            }
            return new Matrix<T>(rows, columns, data);
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix<T> CreateByRows(IEnumerable<IEnumerable<T>> rowsData)
        {
            return CreateByRows(rowsData.Select(rs => (rs as T[]) ?? rs.ToArray()).ToArray());
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix<T> CreateByRows(IEnumerable<T[]> rowsData)
        {
            return CreateByRows(rowsData as T[][] ?? rowsData.ToArray());
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix<T> CreateByRows(params T[][] rowsData)
        {
            if (rowsData.Length < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowsData));
            int rows = rowsData.Length;
            int cols = rowsData[0].Length;
            T[] data = new T[rows * cols];
            for (int i = 0; i < rows; i++)
            {
                int min = System.Math.Min(cols, rowsData[i].Length);
                for (int j = 0; j < min; j++)
                {
                    data[j * rows + i] = rowsData[i][j];
                }
            }
            return new Matrix<T>(rows, cols, data);
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix<T> CreateByRows(IEnumerable<Vector<T>> rowsData)
        {
            return CreateByRows(rowsData.ToArray());
        }
        /// <summary>
        /// 根据行数据创建矩阵
        /// </summary>
        /// <param name="rowsData">行数据</param>
        /// <exception cref="ArgumentException">行长小于首行</exception>
        public static Matrix<T> CreateByRows(params Vector<T>[] rowsData)
        {
            if (rowsData.Length < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(rowsData));
            int rows = rowsData.Length;
            int cols = rowsData[0].Count;
            T[] data = new T[rows * cols];
            for (int i = 0; i < rows; i++)
            {
                int min = System.Math.Min(cols, rowsData[i].Count);
                for (int j = 0; j < min; j++)
                {
                    data[j * rows + i] = rowsData[i].Get(j);
                }
            }
            return new Matrix<T>(rows, cols, data);
        }
        /// <summary>
        /// 根据列主序数据创建矩阵
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="columnMajorData">列主序数据</param>
        public static Matrix<T> CreateByColumnMajorData(int rows, int columns, IEnumerable<T> columnMajorData)
        {
            CheckValidRowAndColumn(rows, columns);
            if (columnMajorData is null)
                ThrowHelper.ThrowArgumentNullException(nameof(columnMajorData));
            T[] data = new T[rows * columns];
            T[] arrayData = columnMajorData.ToArray();
            Array.Copy(arrayData, 0, data, 0, System.Math.Min(data.Length, arrayData.Length));
            return new Matrix<T>(rows, columns, data);
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix<T> CreateByColumns(IEnumerable<IEnumerable<T>> columnsData)
        {
            return CreateByColumns(columnsData.Select(cs => (cs as T[]) ?? cs.ToArray()).ToArray());
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix<T> CreateByColumns(IEnumerable<T[]> columnsData)
        {
            return CreateByColumns(columnsData as T[][] ?? columnsData.ToArray());
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix<T> CreateByColumns(params T[][] columnsData)
        {
            if (columnsData.Length < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnsData));
            int cols = columnsData.Length;
            int rows = columnsData[0].Length;
            T[] data = new T[rows * cols];
            for (int i = 0; i < cols; i++)
            {
                Array.Copy(columnsData[i], 0, data, i * rows, System.Math.Min(rows, columnsData[i].Length));
            }
            return new Matrix<T>(rows, cols, data);
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix<T> CreateByColumns(IEnumerable<Vector<T>> columnsData)
        {
            return CreateByColumns(columnsData.ToArray());
        }
        /// <summary>
        /// 根据列数据创建矩阵
        /// </summary>
        /// <param name="columnsData">列数据</param>
        /// <exception cref="ArgumentException">列长小于首列</exception>
        public static Matrix<T> CreateByColumns(params Vector<T>[] columnsData)
        {
            if (columnsData.Length < 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(columnsData));
            int cols = columnsData.Length;
            int rows = columnsData[0].Count;
            T[] data = new T[rows * cols];
            for (int i = 0; i < cols; i++)
            {
                Array.Copy(columnsData[i].Storage, 0, data, i * rows, System.Math.Min(rows, columnsData[i].Count));
            }
            return new Matrix<T>(rows, cols, data);
        }
    }
}
