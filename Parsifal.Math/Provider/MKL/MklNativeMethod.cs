using System.Runtime.InteropServices;

namespace Parsifal.Math.Provider.MKL
{
    /* MKL库的一些常用信息
     * 1. Level 1 为向量-向量运算；Level 2 为矩阵-向量运算；Level 3 为矩阵-矩阵运算
     * 2. s 实数单精度；d 实数双精度；c 复数单精度；z 复数双精度
     * 3. ge 一般矩阵；gb 一般带状矩阵；sy 对称矩阵；sp 对称矩阵(压缩存储)；sb 对称带状矩阵；tr 三角矩阵； tp 三角矩阵(压缩存储)； tb 三角带状矩阵
     * 4. mv 矩阵向量乘；mm 矩阵矩阵乘；sv 解线性方程
     * 5. 
     */
    internal class MklNativeMethod
    {
        private const string DLLNAME = "Parsifal.Math.Mkl.Win.dll";

        /// <summary>向量标量积</summary>
        /// <remarks><paramref name="x"/>=<paramref name="alpha"/>*<paramref name="x"/></remarks>
        /// <param name="x">向量</param>
        /// <param name="n">向量长</param>
        /// <param name="alpha">缩放</param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dVectorScalarProduct([Out] double[] x, int n, double alpha);
        /// <summary>标量向量积并加向量</summary>
        /// <remarks><paramref name="y"/>=<paramref name="alpha"/>*<paramref name="x"/>+<paramref name="y"/></remarks>
        /// <param name="x">向量x</param>
        /// <param name="n">向量长</param>
        /// <param name="alpha">缩放</param>
        /// <param name="y">向量y</param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dScalarVectorAddVector(double[] x, int n, double alpha, [In, Out] double[] y);
        /// <summary>向量相加</summary>
        /// <remarks><paramref name="y"/>=<paramref name="a"/>+<paramref name="b"/></remarks>
        /// <param name="a">向量a</param>
        /// <param name="b">向量b</param>
        /// <param name="n">向量长</param>
        /// <param name="y">向量y</param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dVectorAdd(double[] a, double[] b, int n, [In, Out] double[] y);
        /// <summary>向量相减</summary>
        /// <remarks><paramref name="y"/>=<paramref name="a"/>-<paramref name="b"/></remarks>
        /// <param name="a">向量a</param>
        /// <param name="b">向量b</param>
        /// <param name="n">向量长</param>
        /// <param name="y">向量y</param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dVectorSub(double[] a, double[] b, int n, [In, Out] double[] y);
        /// <summary>向量点积</summary>
        /// <remarks>x•y</remarks>
        /// <param name="x">向量x</param>
        /// <param name="y">向量y</param>
        /// <param name="n">向量长</param>
        /// <returns>点积</returns>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double dVectorDotProduct(double[] x, double[] y, int n);
        /// <summary>矩阵向量乘法</summary>
        /// <remarks><paramref name="y"/>=<paramref name="alpha"/>*<paramref name="a"/>*<paramref name="x"/>+<paramref name="beta"/>*<paramref name="y"/></remarks>
        /// <param name="trans">a转置</param>
        /// <param name="m">a行数</param>
        /// <param name="n">a列数</param>
        /// <param name="a">矩阵a</param>
        /// <param name="alpha">alpha</param>
        /// <param name="x">向量x</param>
        /// <param name="beta">beta</param>
        /// <param name="y">向量y</param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dMatrixVectorProduct(CBLAS_TRANSPOSE trans, int m, int n, double[] a, double alpha, double[] x, double beta, [In, Out] double[] y);
        /// <summary>矩阵矩阵乘法</summary>
        /// <remarks><paramref name="c"/>=<paramref name="alpha"/>*<paramref name="a"/>*<paramref name="b"/>+<paramref name="beta"/>*<paramref name="c"/></remarks>
        /// <param name="transa">a转置</param>
        /// <param name="transb">b转置</param>
        /// <param name="m">a行数/c行数</param>
        /// <param name="n">b列数/c列数</param>
        /// <param name="k">a列数/b行数</param>
        /// <param name="a">矩阵a</param>
        /// <param name="b">矩阵b</param>
        /// <param name="alpha">alpha</param>
        /// <param name="beta">beta</param>
        /// <param name="c">矩阵c</param>
        [DllImport(DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dMatrixMatrixProduct(CBLAS_TRANSPOSE transa, CBLAS_TRANSPOSE transb, int m, int n, int k, double[] a, double[] b, double alpha, double beta, [In, Out] double[] c);

    }
}
