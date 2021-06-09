namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// 矩阵存储主序
    /// </summary>
    public enum MatrixMajorOrder
    {
        /// <summary>
        /// 行主序
        /// </summary>
        Row,
        /// <summary>
        /// 列主序
        /// </summary>
        Column
    }
    /// <summary>
    /// 矩阵转置
    /// </summary>
    public enum MatrixTranspose
    {
        /// <summary>
        /// 不转置
        /// </summary>
        NotTranspose,
        /// <summary>
        /// 转置
        /// </summary>
        Transpose
    }
    /// <summary>
    /// 矩阵范数
    /// </summary>
    public enum MatrixNorm
    {
        /// <summary>
        /// 1范数
        /// </summary>
        /// <remarks>列和范数</remarks>
        OneNorm,
        /// <summary>
        /// 2范数
        /// </summary>
        /// <remarks>谱范数</remarks>
        TwoNorm,
        /// <summary>
        /// F范数
        /// </summary>
        /// <remarks>Euclidean范数、Schur范数、Hilbert-Schmidt范数</remarks>
        FrobeniusNorm,
        /// <summary>
        /// 无穷范数
        /// </summary>
        /// <remarks>行和范数</remarks>
        InfinityNorm
    }
}
