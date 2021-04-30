using System;
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
            var mat1 = new Matrix(3, 3, element1);
            var ss = mat1.ToString();
            Assert.Equal(mat1.Count, element1.Length);
            var lowT1 = mat1.LowerTriangle();
            var upT1 = mat1.UpperTriangle();
            var ls1 = lowT1.ToString();
            var us1 = upT1.ToString();

            var element2 = new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } };
            var mat2 = new Matrix(element2);
            var tempArr = mat2.ToRowMajorArray();
            var strF = mat2.ToString();
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
            var mat1 = Matrix.CreateByColumnMajorData(3, 4, data1);
            var mat2 = Matrix.CreateByRowMajorData(4, 3, data1);
            Assert.Equal(mat1, mat2.Transpose());

            var data2 = new List<IEnumerable<double>>
            {
                Enumerable.Range(1, 4).Select(i=>(double)i),
                Enumerable.Range(11,4).Select(i=>(double)i),
                Enumerable.Range(21,4).Select(i=>(double)i)
            };
            var mat3 = Matrix.CreateByColumns(data2);
            var mat4 = Matrix.CreateByRows(data2);
            Assert.Equal(mat3, mat4.Transpose());

            var data3 = new List<Vector>
            {
                Enumerable.Range(5,5).Select(i=>(double)i).ToArray(),
                Enumerable.Range(15,6).Select(i=>(double)i).ToArray(),//必须大于等于首行
                Enumerable.Range(25,7).Select(i=>(double)i).ToArray()
            };
            var mat5 = Matrix.CreateByColumns(data3);
            var mat6 = Matrix.CreateByRows(data3);
            Assert.True(mat5.Transpose().Equals(mat6));

            data3[1] = new Vector(Enumerable.Range(15, 4).Select(i => (double)i).ToArray());
            Assert.Throws<ArgumentException>(() =>
            {
                _ = Matrix.CreateByColumns(data3);
            });
        }

        [Fact]
        public void VectorTest()
        {
            var element = new double[] { 1, 3, 5, 7 };
            var vec = new Vector(element);
            vec.Clear();
        }
    }
}
