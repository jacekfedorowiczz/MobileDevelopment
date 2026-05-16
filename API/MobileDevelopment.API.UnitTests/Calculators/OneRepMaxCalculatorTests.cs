using MobileDevelopment.API.Services.Calculators;

namespace MobileDevelopment.API.UnitTests.Calculators
{
    public sealed class OneRepMaxCalculatorTests
    {
        [Fact]
        public void Calculate_ShouldReturnInputWeight_WhenRepsEqualOne()
        {
            // Arrange
            var calculator = new OneRepMaxCalculator();

            // Act
            var result = calculator.Calculate(100m, 1);

            // Assert
            Assert.Equal(100m, result);
        }

        [Fact]
        public void Calculate_ShouldUseEpleyFormula_WhenRepsAreGreaterThanOne()
        {
            // Arrange
            var calculator = new OneRepMaxCalculator();

            // Act
            var result = calculator.Calculate(100m, 5);

            // Assert
            Assert.Equal(116.7m, result);
        }
    }
}
