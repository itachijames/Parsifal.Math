namespace Parsifal.Math
{
    public enum LogicProviderType
    {
        /// <summary>
        /// pure c#
        /// </summary>
        Native,
        /// <summary>
        /// Intel MKL
        /// </summary>
        MKL,
        /// <summary>
        /// Nvidia CUDA
        /// </summary>
        CUDA,
        //more to add
    }
}
