using Parsifal.Math.Geometry;
using Xunit;

namespace Parsifal.Math.UnitTest
{
    public class CommonPartTest
    {

        [Fact]
        public void VectorTest()
        {
            Assert.False(MathUtilHelper.HaveRepeated(1, 2, 3, 4));

            var vector = new Vector2D(1, 3);
            var normalVec = new Vector2D(0, -1);
            var reflectedVec = Vector2D.Reflect(vector, normalVec);

            var vn = vector.Normalize();
            Assert.True(vector.IsParallel(vn));
        }

        [Fact]
        public void LineTest()
        {
            var p1 = new Point2D(0, 1);
            var p2 = new Point2D(2, 3);
            var l1 = new Line(p1, p2);
            var l2 = new Line(p2, p1);
            Assert.Equal(l1, l2);
            var l3 = new Line(1, 1);
            Assert.Equal(l1, l3);
            var l4 = new Line(-1, 0);
            Assert.True(l1.IsVertical(l4));
            var s1 = new Segment(p1, p2);
            Assert.True(l1.IsParallel(s1.GetLine()));

            var l5 = new Line(new Point2D(0, 0), new Point2D(0, 1));
            var l6 = new Line(new Point2D(1, 0), new Point2D(1, 1));
            var l7 = new Line(new Point2D(0, 0), new Point2D(1, 0));
            Assert.True(l5.IsParallel(l6));
            Assert.True(l5.IsVertical(l7));

            var l8 = new Line(1, 0);
            var p3 = new Point2D(2, 0);
            var d1 = l8.Distance(p3);
            var fp = l8.GetFootpoint(p3);
        }

        private Polygon GetPolygon()
        {
            var vertexes = new Point2D[]
            {
                //new Point2D(0,0),
                //new Point2D(5,0),
                //new Point2D(5,5),
                //new Point2D(0,5)

                new Point2D(2,1.5),
                new Point2D(4,-3),
                new Point2D(1.5,-2),
                new Point2D(-2,-3),
                new Point2D(-1,2)
            };
            return new Polygon(vertexes);
        }

        [Fact]
        public void PolygonTest()
        {
            var polygon = GetPolygon();
            Assert.False(polygon.IsConvex());
            //Assert.True(polygon.GetArea().IsEqual(25));
        }

        private Triangle GetTriangle()
        {
            var p1 = new Point2D(0, 0);
            var p2 = new Point2D(2, 0);
            var p3 = new Point2D(4, 2);
            return new Triangle(p1, p2, p3);
        }

        [Fact]
        public void TriangleTest()
        {
            var triangle = GetTriangle();
            Assert.True(triangle.GetTriangleType() == AngleType.Obtuse);
        }


        [Fact]
        public void AngleTest()
        {
            var angle = new Angle(4.2 * 3.14);
            Assert.True(angle.Type == AngleType.Acute);
        }

        [Fact]
        public void Test1()
        {
            var ne = GetNegEpsilon();
            Assert.Equal(CalculateHelper.NegativeMachineEpsilon, ne);
            var pe = GetPosEpsilon();
            Assert.Equal(CalculateHelper.PositiveMachineEpsilon, pe);

            static double GetNegEpsilon()
            {
                double eps = 1.0d;

                while ((1.0d - (eps / 2.0d)) < 1.0d)
                    eps /= 2.0d;

                return eps;
            }

            static double GetPosEpsilon()
            {
                double eps = 1.0d;

                while ((1.0d + (eps / 2.0d)) > 1.0d)
                    eps /= 2.0d;

                return eps;
            }
        }
    }
}
