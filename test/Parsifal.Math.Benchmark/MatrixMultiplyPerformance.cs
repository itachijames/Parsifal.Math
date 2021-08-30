using System;
using BenchmarkDotNet.Attributes;

namespace Parsifal.Math.Benchmark
{
    [MemoryDiagnoser]
    public class MatrixMultiplyPerformance
    {
        readonly LinearAlgebraProvider _csLogic;
        readonly LinearAlgebraProvider _mklLogic;
        readonly LinearAlgebraProvider _cudaLogic;

        [Params(4, 8, 16, 32, 64, 128, 256, 512, 1024)]
        int MatOrder;
        double[] _left;
        double[] _right;
        double[] _result;

        public MatrixMultiplyPerformance()
        {
            //NEED TO DO: 将以下类转换为 public
            //    _csLogic = new NativeProvider();
            //    _mklLogic = new MklProvider();
            //    _cudaLogic = new CudaProvider();
        }

        [GlobalSetup]
        public void Setup()
        {
            const double max = 255;
            const double min = -256;
            var random = new Random(Guid.NewGuid().GetHashCode());
            int total = MatOrder * MatOrder;
            _left = new double[total];
            for (int i = 0; i < total; i++)
            {
                _left[i] = random.NextDouble() * (max - min) + min;
            }
            _right = new double[total];
            for (int i = 0; i < total; i++)
            {
                _right[i] = random.NextDouble() * (max - min) + min;
            }
            _result = new double[total];
        }

        [Benchmark]
        public void UseNative() => _csLogic.MatrixMultiply(MatOrder, MatOrder, _left, MatOrder, MatOrder, _right, _result);

        [Benchmark]
        public void UseMKL() => _mklLogic.MatrixMultiply(MatOrder, MatOrder, _left, MatOrder, MatOrder, _right, _result);

        [Benchmark]
        public void UseCUDA() => _cudaLogic.MatrixMultiply(MatOrder, MatOrder, _left, MatOrder, MatOrder, _right, _result);
    }
}
