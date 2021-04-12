using System;

namespace Parsifal.Math
{
    /// <summary>
    /// 维度匹配异常
    /// </summary>
    public class DimensionMatchException : Exception
    {
        public DimensionMatchException() : base(ErrorReason.DimensionDoesNoMatch)
        { }
    }
}
