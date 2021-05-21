using System.Threading.Tasks;

namespace Parsifal.Math
{
    internal class ParallelHelper
    {
        /// <summary>
        /// 是否不应使用并行计算
        /// </summary>
        /// <returns>不应使用并行时返回true;否则false</returns>
        public static bool ShouldNotUseParallelism()
        {
            return LogicControl.MaxDegreeOfParallelism < 2;
        }
        /// <summary>
        /// 并行任务选项
        /// </summary>
        public static ParallelOptions CreateParallelOptions()
        {
            return new ParallelOptions
            {
                MaxDegreeOfParallelism = LogicControl.MaxDegreeOfParallelism,
                TaskScheduler = TaskScheduler.Default
            };
        }
    }
}
