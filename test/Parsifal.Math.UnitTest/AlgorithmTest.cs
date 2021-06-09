using Parsifal.Math.Algorithm;
using Parsifal.Math.Geometry;
using Xunit;

namespace Parsifal.Math.UnitTest
{
    public class AlgorithmTest
    {
        [Fact]
        public void GeometryInOutTest()
        {
            var point = new Point2D(2, 1.51);
            var polygon = GetPolygon();
            Assert.False(GeometryAlgorithm.IsPointInPolygon(point, polygon));
            point = new Point2D(1, 2.1);
            var distance = GeometryAlgorithm.Point2Polygon(point, polygon, out var nearest);
            Assert.True(point.X == nearest.X && distance == System.Math.Abs(point.Y - nearest.Y));

            static Polygon GetPolygon()
            {
                var boundary = new Point2D[]
                {
                new Point2D(0, 0),
                new Point2D(4, 0),
                new Point2D(4, 4),
                new Point2D(2, 4),
                new Point2D(2, 3),
                new Point2D(3, 1),
                new Point2D(1, 2),
                new Point2D(0, 2)

                    //new Point2D(0, 0),
                    //new Point2D(2, 3),
                    //new Point2D(4, 1),
                    //new Point2D(3, -3),
                    //new Point2D(3, 4),
                    //new Point2D(1, 4)
                };
                return new Polygon(boundary);
            }
        }

        [Fact]
        public void IntersectedTest()
        {
            var polygon = new Point2D[]
            {
                new Point2D(0, 0),
                new Point2D(2, 3),
                new Point2D(4, 1),
                new Point2D(3, -3),
                new Point2D(3, 4),
                new Point2D(1, 4)
            };
            Assert.True(GeometryAlgorithm.IsSelfIntersected(polygon));
        }

        [Fact]
        public void DistanceTest()
        {
            var circle = new Circle(Point2D.Origin, 1);
            var point = new Point2D(-3, 0);

            Assert.True(GeometryAlgorithm.Point2Circle(point, circle, out _).IsEqual(2));
        }
    }
}
