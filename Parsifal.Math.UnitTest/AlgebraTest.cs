using System.Collections.Generic;
using System.Linq;
using Parsifal.Math.Algebra;
using Xunit;

namespace Parsifal.Math.UnitTest
{
    public class AlgebraTest
    {
        [Fact]
        public void MatrixBaseTest()
        {
            var element1 = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var mat1 = MatrixCreator.CreateByColumnMajorData(3, 3, element1);
            var ss = mat1.ToString();
            Assert.Equal(mat1.Count, element1.Length);
            var lowT1 = mat1.LowerTriangle();
            var upT1 = mat1.UpperTriangle();
            var ls1 = lowT1.ToString();
            var us1 = upT1.ToString();

            var element2 = new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } };
            var mat2 = MatrixCreator.CreateByArray(element2);
            var strF = mat2.ToString();
            var lowT2 = mat2.LowerTriangle();
            var ls2 = lowT2.ToString();
            var upT2 = mat2.UpperTriangle();
            var us2 = upT2.ToString();
            var matT = mat2.Transpose();
            var strMatT = matT.ToString();

            var matSub = mat2.GetSubMatrix(1, 2, 0, 3);
            var strMatSub = matSub.ToString();
            Assert.Equal(6, matSub.Count);
        }

        [Fact]
        public void MatrixCreateTest()
        {
            var data1 = Enumerable.Range(0, 12).Select(i => (double)i);
            var mat1 = MatrixCreator.CreateByColumnMajorData(3, 4, data1);
            var mat2 = MatrixCreator.CreateByRowMajorData(4, 3, data1);
            Assert.Equal(mat1, mat2.Transpose());

            var data2 = new List<IEnumerable<double>>
            {
                Enumerable.Range(1, 4).Select(i=>(double)i),
                Enumerable.Range(11,4).Select(i=>(double)i),
                Enumerable.Range(21,4).Select(i=>(double)i)
            };
            var mat3 = MatrixCreator.CreateByColumns(data2);
            var mat4 = MatrixCreator.CreateByRows(data2);
            Assert.Equal(mat3, mat4.Transpose());

            var data3 = new List<Vector>
            {
                Enumerable.Range(5,5).Select(i=>(double)i).ToArray(),
                Enumerable.Range(15,6).Select(i=>(double)i).ToArray(),
                Enumerable.Range(25,7).Select(i=>(double)i).ToArray()
            };
            var mat5 = MatrixCreator.CreateByColumns(data3);
            var mat6 = MatrixCreator.CreateByRows(data3);
            Assert.True(mat5.Transpose().Equals(mat6));

            data3[1] = new Vector(Enumerable.Range(15, 4).Select(i => (double)i).ToArray());
            var mat7 = MatrixCreator.CreateByColumns(data3[0], data3[1], data3[2]);
        }

        [Theory]
        [InlineData(LogicProviderType.Native)]
        //[InlineData(LogicProviderType.MKL)]
        //[InlineData(LogicProviderType.CUDA)]
        public void MatrixTest(LogicProviderType providerType)
        {
            var data1 = Enumerable.Range(1, 12).Select(i => (double)i);
            var mat1 = MatrixCreator.CreateByColumnMajorData(3, 4, data1);//3*4
            var data2 = Enumerable.Range(-5, 12).Select(i => (double)i);
            var mat2 = MatrixCreator.CreateByRowMajorData(4, 3, data2);//4*3

            var addS = mat1.Add(100);
            //var addM = mat1.Add(mat2);
            var subS = mat1.Subtract(-10);
            //var subM = mat1.Subtract(mat2);
            var m1 = mat1 * mat2;//3*3

            if (LogicControl.Use(providerType))
            {
                var mulT = mat1 * mat2;
                var mulTStr = mulT.ToString();
                Assert.Equal(m1, mulT);
            }

            var mat1T = mat1.Transpose();//4*3
            var mat2T = mat2.Transpose();//3*4
            var m2 = Matrix.MultiplyTranspose(mat1T, mat2);//4*4
            var ms1 = m2.ToString();
            var m3 = Matrix.TransposeMultiply(mat1T, mat2);//3*3
            Assert.Equal(m1, m3);
            var m4 = Matrix.TransposeMultiply(mat1, mat2T);//4*4
            Assert.Equal(m2, m4);
        }

        [Fact]
        public void VectorTest()
        {
            var element = new double[] { 1, 3, 5, 7 };
            var vec = new Vector(element);
            vec.Clear();
        }

        //[Theory]
        //[InlineData(32)]
        //[InlineData(50)]
        //[InlineData(64)]
        //[InlineData(100)]
        //[InlineData(200)]
        //[InlineData(500)]
        //[InlineData(1000)]
        //public void LALogicProviderTest(int order)
        //{
        //    ILogicProvider provider = new NativeProvider();

        //    var random = new Random(Guid.NewGuid().GetHashCode());
        //    var data1 = Enumerable.Range(1, 1_000_000).Select(i => random.NextDouble() * i / 100);
        //    var mX = MatrixCreator.CreateByColumnMajorData(order, order, data1);
        //    var data2 = Enumerable.Range(-5000, 1_000_000).Select(i => random.NextDouble() * i / 100);
        //    var mY = MatrixCreator.CreateByColumnMajorData(order, order, data2);

        //    var result1 = new double[order * order];
        //    provider.MatrixMultiply(mX.Rows, mX.Columns, mX.Storage, mY.Rows, mY.Columns, mY.Storage, result1);
        //    var result2 = new double[order * order];
        //    provider.MatrixMultiply(1, mX.Rows, mX.Columns, mX.Storage, MatrixTranspose.NotTranspose, mY.Rows, mY.Columns, mY.Storage, MatrixTranspose.NotTranspose, 0, result2);
        //    Assert.Equal(result1, result2);

        //    //var mXT = mX.Transpose();
        //    //var result3 = new double[250000];
        //    //provider.MatrixMultiply(mXT.Rows, mXT.Columns, mXT.Storage, mY.Rows, mY.Columns, mY.Storage, result3);
        //    //var result4 = new double[250000];
        //    //provider.MatrixMultiply(1, mX.Rows, mX.Columns, mX.Storage, MatrixTranspose.Transpose, mY.Rows, mY.Columns, mY.Storage, MatrixTranspose.NotTranspose, 0, result4);
        //    //Assert.Equal(result3, result4);
        //}
    }
}
