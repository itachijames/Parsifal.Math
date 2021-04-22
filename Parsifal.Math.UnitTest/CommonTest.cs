using Xunit;

namespace Parsifal.Math.UnitTest
{
    public class CommonTest
    {
        [Fact]
        public void AngleTest()
        {
            var angle = new Angle(4.2 * 3.14);
            Assert.True(angle.Type == AngleType.Acute);
        }

        [Fact]
        public void CalculateConstTest()
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

        [Theory]
        [InlineData(309.71)]
        public void MagnitudeTest(double value)
        {
            Assert.Equal(2, value.Magnitude());
        }
    }
}
