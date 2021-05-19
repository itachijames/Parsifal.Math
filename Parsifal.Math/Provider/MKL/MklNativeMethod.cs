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
        const string _DllName = "MKL";

        [DllImport(_DllName)]
        public static extern void cblas_dgemv(CBLAS_LAYOUT layout, CBLAS_TRANSPOSE trans,
            int m, int n, double alpha, double[] a, int lda, double[] x, int incx,
            double beta, double[] y, int incy);

        [DllImport(_DllName)]
        public static extern void cblas_dgemm(CBLAS_LAYOUT layout, CBLAS_TRANSPOSE transa, CBLAS_TRANSPOSE transb,
            int m, int n, int k,
            double alpha, double[] a, int lda, double[] b, int ldb,
            double beta, double[] c, int ldc);
    }
}
