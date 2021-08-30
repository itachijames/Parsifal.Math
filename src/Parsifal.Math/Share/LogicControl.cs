using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Parsifal.Math.Provider.Native;

namespace Parsifal.Math
{
    public static class LogicControl
    {
        /// <summary>
        /// 线性代数算法实现
        /// </summary>
        public static LinearAlgebraProvider LinearAlgebraProvider { get; private set; }

        private static int _maxDegreeOfParallelism;
        /// <summary>
        /// 最大并行数
        /// </summary>
        public static int MaxDegreeOfParallelism
        {
            get => _maxDegreeOfParallelism;
            set
            {
                _maxDegreeOfParallelism = System.Math.Max(1, System.Math.Min(1024, value));//1~1024
            }
        }

        static LogicControl()
        {
            _maxDegreeOfParallelism = Environment.ProcessorCount;
            UseDefault();
        }

        /// <summary>
        /// 使用指定算法逻辑
        /// </summary>
        /// <param name="providerType"></param>
        public static void Use(LinearAlgebraProviderType providerType)
        {
            if (IsAvaliable(providerType))
            {
                switch (providerType)
                {
                    case LinearAlgebraProviderType.Native:
                        UseDefault();
                        break;
                    case LinearAlgebraProviderType.MKL:
                        //todo
                        break;
                    case LinearAlgebraProviderType.CUDA:
                        //todo
                        break;
                }
            }
            ThrowHelper.ThrowNotSupportedException(ErrorReason.ServiceUnavailable);
        }
        /// <summary>
        /// 使用默认实现
        /// </summary>
        public static void UseDefault()
        {//使用原生实现
            if (LinearAlgebraProvider != null && typeof(NativeProvider) == LinearAlgebraProvider.GetType())
                return;
            LinearAlgebraProvider = new NativeProvider();
        }
        /// <summary>
        /// 指定算法逻辑是否可用
        /// </summary>
        public static bool IsAvaliable(LinearAlgebraProviderType providerType)
        {
            switch (providerType)
            {
                case LinearAlgebraProviderType.Native:
                    return true;
                case LinearAlgebraProviderType.MKL:
                    return true;
                //break;
                case LinearAlgebraProviderType.CUDA:
                    break;
                default:
                    throw new Exception(ErrorReason.UnknowType);
            }
            return false;
        }
        /// <summary>
        /// 当前环境描述
        /// </summary>
        /// <returns></returns>
        internal static string EnvironmentDescribe()
        {
            var versionAttribute = typeof(LogicControl).GetTypeInfo().Assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute;
            var sb = new StringBuilder();
            sb.AppendLine("Parsifal.Math Configuration:");
            sb.AppendLine($"Version {versionAttribute?.InformationalVersion}");
#if NETSTANDARD2_1
            sb.AppendLine("Built With .Net Standard 2.1");
#elif NET5_0_OR_GREATER
            sb.AppendLine("Built for .Net 5.0+");
#endif
            sb.AppendLine($"Logic Provider: {LinearAlgebraProvider}");
            sb.AppendLine($"Operating System: {RuntimeInformation.OSDescription}");
            sb.AppendLine($"Operating System Architecture: {RuntimeInformation.OSArchitecture}");
            sb.AppendLine($"Framework: {RuntimeInformation.FrameworkDescription}");
            sb.AppendLine($"Process Architecture: {RuntimeInformation.ProcessArchitecture}");
            return sb.ToString();
        }
    }
}
