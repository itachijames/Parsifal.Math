using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Parsifal.Math
{
    public class LogicControl
    {
        /// <summary>
        /// 当前算法逻辑提供/实现者
        /// </summary>
        public static LogicProvider CurrentProvider { get; private set; }
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
        }

        /// <summary>
        /// 使用指定算法逻辑提供者
        /// </summary>
        /// <param name="provider"></param>
        public static void Use(LogicProvider provider)
        {
            if (IsAvaliable(provider))
            {
                CurrentProvider = provider;
                //todo
            }
            else
            {
                UseDefault();
            }
        }
        /// <summary>
        /// 使用默认实现
        /// </summary>
        public static void UseDefault()
        {//使用原生实现
            CurrentProvider = LogicProvider.Native;
            //todo
        }
        /// <summary>
        /// 指定算法逻辑是否可用
        /// </summary>
        public static bool IsAvaliable(LogicProvider provider)
        {
            switch (provider)
            {
                case LogicProvider.Native:
                    return true;
                case LogicProvider.MKL:
                    {//todo
                    }
                    break;
                case LogicProvider.CUDA:
                    {//todo
                    }
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
            sb.AppendLine($"Logic Provider: {CurrentProvider}");
            sb.AppendLine($"Operating System: {RuntimeInformation.OSDescription}");
            sb.AppendLine($"Operating System Architecture: {RuntimeInformation.OSArchitecture}");
            sb.AppendLine($"Framework: {RuntimeInformation.FrameworkDescription}");
            sb.AppendLine($"Process Architecture: {RuntimeInformation.ProcessArchitecture}");
            return sb.ToString();
        }
    }
    public enum LogicProvider
    {
        /// <summary>
        /// 原生
        /// </summary>
        Native,
        /// <summary>
        /// Intel MKL
        /// </summary>
        MKL,
        /// <summary>
        /// Nvidia CUDA
        /// </summary>
        CUDA
    }
}
