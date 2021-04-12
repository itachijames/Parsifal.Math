using Parsifal.Math.Algebra;
using Xunit;

namespace Parsifal.Math.UnitTest
{
    public class MatrixTest
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
            var tempArr = mat2.ToArray();
            var strF = mat2.ToString();
            var matT = mat2.Transpose();
            var strMatT = matT.ToString();

            var matSub = mat2.GetSubMatrix(1, 2, 0, 3);
            var strMatSub = matSub.ToString();
            Assert.Equal(6, matSub.Count);

        }
    }
}
