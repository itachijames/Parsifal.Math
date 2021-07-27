using BenchmarkDotNet.Running;

namespace Parsifal.Math.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<MatrixMultiplyPerformance>();
        }
    }
}
