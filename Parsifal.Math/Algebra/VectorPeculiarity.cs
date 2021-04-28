namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// 向量类型
    /// </summary>
    public enum VectorType
    {
        /// <summary>
        /// 行向量
        /// </summary>
        Row,
        /// <summary>
        /// 列向量
        /// </summary>
        Column
    }
    /// <summary>
    /// 向量范数
    /// </summary>
    public enum VectorNorm
    {
        /// <summary>
        /// 0范数
        /// </summary>
        ZeroNorm,
        /// <summary>
        /// 1范数
        /// </summary>
        /// <remarks>和范数</remarks>
        OneNorm,
        /// <summary>
        /// 2范数
        /// </summary>
        /// <remarks>Euclidean范数、Frobenius范数</remarks>
        TwoNorm,
        /// <summary>
        /// 无穷范数/极大范数
        /// </summary>
        InfinityNorm
    }
}
