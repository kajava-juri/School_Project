using Xunit;

namespace KooliProjekt.UnitTests
{
    public class Calculator
    {
        public float Divide(float x, float y)
        {
            return x / y;
        }
    }

    public class UnitTest1
    {
        [Fact(DisplayName = "6 / 2 = 3")]
        public void Divide_should_return_2_for_6_and_3()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Divide(6, 3);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact(DisplayName = "3 / 0 = Infinity")]
        public void Crash()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Divide(3, 0);

            // Assert
            Assert.True(float.IsInfinity(result));
        }

        [Fact(DisplayName = "1 / 2 = 0")]
        public void Divide_should_return_zero_point_five_for_1_and_2()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Divide(1, 2);

            // Assert
            Assert.Equal(0.5, result);
        }
    }
}