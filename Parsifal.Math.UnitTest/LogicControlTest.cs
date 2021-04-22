using Xunit;

namespace Parsifal.Math.UnitTest
{
    public class LogicControlTest
    {
        [Fact]
        public void EnvironmentTest()
        {
            var curEnv = LogicControl.EnvironmentDescribe();
        }
    }
}
