#include "pch.h"
#include "blas.h"

extern "C" {
	/* 向量乘法 */
	DLLEXPORT void dVectorScalarProduct(double x[], const int n, const double alpha) {
		cblas_dscal(n, alpha, x, 1);
	}
	/* 向量乘并加 */
	DLLEXPORT void dScalarVectorAddVector(const double x[], const int n, const double alpha, double y[]) {
		cblas_daxpy(n, alpha, x, 1, y, 1);
	}
	/* 向量加法 */
	DLLEXPORT void dVectorAdd(const double a[], const double b[], const int n, double y[]) {
		vdAdd(n, a, b, y);
	}
	/* 向量减法 */
	DLLEXPORT void dVectorSub(const double a[], const double b[], const int n, double y[]) {
		vdSub(n, a, b, y);
	}
	/* 向量点积 */
	DLLEXPORT double dVectorDotProduct(const double x[], const double y[], const int n) {
		return cblas_ddot(n, x, 1, y, 1);
	}
	/* 向量2范数 */
	DLLEXPORT double dVector2Norm(const double x[], const int n) {
		return cblas_dnrm2(n, x, 1);
	}
	/* 矩阵向量乘法 */
	DLLEXPORT void dMatrixVectorProduct(CBLAS_TRANSPOSE trans, const int m, const int n, const double a[], const double alpha,
		const double x[], const double beta, double y[]) {
		MKL_INT lda = trans == CblasNoTrans ? m : n;
		cblas_dgemv(CblasColMajor, trans, m, n, alpha, a, lda, x, 1, beta, y, 1);
	}
	/* 矩阵乘法*/
	DLLEXPORT void dMatrixMatrixProduct(CBLAS_TRANSPOSE transa, CBLAS_TRANSPOSE transb, const int m, const int n, const int k,
		const double a[], const double b[], const double alpha, const double beta, double c[]) {
		MKL_INT	lda = transa == CblasNoTrans ? m : k;
		MKL_INT ldb = transb == CblasNoTrans ? k : n;
		MKL_INT ldc = m;
		cblas_dgemm(CblasColMajor, transa, transb, m, n, k, alpha, a, lda, b, ldb, beta, c, ldc);
	}

}
