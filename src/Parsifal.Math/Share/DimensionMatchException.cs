using System;
using System.Text;
using Parsifal.Math.Algebra;

namespace Parsifal.Math
{
    /// <summary>
    /// 维度匹配异常
    /// </summary>
    public class DimensionMatchException : Exception
    {
        public DimensionMatchException() : base(ErrorReason.DimensionDoesNoMatch)
        { }

        public DimensionMatchException(params object[] arguments) : base(GetDescribe(arguments))
        { }

        private static string GetDescribe(params object[] arguments)
        {
            var sb = new StringBuilder($"{ErrorReason.DimensionDoesNoMatch}!");
            if (arguments != null)
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    var type = arguments[i].GetType();
                    if (type.IsAssignableFrom(typeof(Matrix)))
                    {
                        var matrix = (Matrix)arguments[i];
                        sb.Append($" Argument {i + 1} is a {matrix.Rows}×{matrix.Columns} matrix.");
                    }
                    else if (type.IsAssignableFrom(typeof(Vector)))
                    {
                        var vector = (Vector)arguments[i];
                        sb.Append($" Argument {i + 1} is a {vector.Count} dimension vector.");
                    }
                    else if (type.IsAssignableFrom(typeof(Tensor)))
                    {
                        var tensor = (Tensor)arguments[i];
                        sb.Append($" Argument {i + 1} is a {tensor.Rank} rank tensor");
                    }
                    else
                    {
                        sb.Append($" Argument {i + 1} is a {nameof(type)} variable.");
                    }
                }
            }
            return sb.ToString();
        }
    }
}
