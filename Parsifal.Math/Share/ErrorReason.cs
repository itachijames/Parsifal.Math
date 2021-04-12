namespace Parsifal.Math
{
    /// <summary>
    /// 错误原因
    /// </summary>
    internal class ErrorReason
    {
        internal const string NotSupportedOperation = "不支持的操作";
        internal const string WrongType = "类型错误";
        internal const string InvalidParameter = "无效参数";
        internal const string NegativeParameter = "参数为负";
        internal const string NotPositiveParameter = "参数不为正数";
        internal const string SameParameter = "参数相同";
        internal const string IncorrectParameterQuantity = "参数数量错误";
        internal const string IndexOutOfRange = "索引越界";
        internal const string NotEfficientSolution = "无有效解";
        internal const string DimensionDoesNoMatch = "维度不匹配";
        internal const string OnlyForSquareMatrix = "仅适用于方阵";
        internal const string InvertibleMatrix = "不可逆矩阵";
    }
}
