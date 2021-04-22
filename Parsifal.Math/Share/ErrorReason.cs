namespace Parsifal.Math
{
    /// <summary>
    /// 错误原因
    /// </summary>
    internal class ErrorReason
    {
        public const string UnknowType = "未知类型";
        public const string WrongType = "类型错误";
        public const string NotSupportedOperation = "不支持的操作";

        public const string InvalidParameter = "无效参数";
        public const string NegativeParameter = "参数为负";
        public const string NotPositiveParameter = "参数不为正数";
        public const string SameParameter = "参数相同";
        public const string IncorrectParameterQuantity = "参数数量错误";

        public const string IndexOutOfRange = "索引越界";
        public const string NotEfficientSolution = "无有效解";
        public const string DimensionDoesNoMatch = "维度不匹配";
        public const string OnlyForSquareMatrix = "仅适用于方阵";
        public const string NoninvertibleMatrix = "不可逆矩阵";
    }
}
