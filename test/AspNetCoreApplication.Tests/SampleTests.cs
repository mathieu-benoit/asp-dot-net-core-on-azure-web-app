using Xunit;

namespace AspNetCoreApplication.Tests
{
    public class SampleTests
    {
        [Fact]
        public void TwoPlusTwoEqualsFour()
        {
            //Arrange
            var expectedResult = 4;

            //Act
            var result = Add(2, 2);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TwoPlusTwoNotEqualsFive()
        {
            //Arrange
            var unExpectedResult = 5;

            //Act
            var result = Add(2, 2);

            //Assert
            Assert.NotEqual(unExpectedResult, result);
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
