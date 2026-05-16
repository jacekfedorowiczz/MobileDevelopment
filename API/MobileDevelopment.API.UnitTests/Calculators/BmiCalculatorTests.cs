using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Services.Calculators;

namespace MobileDevelopment.API.UnitTests.Calculators
{
    public sealed class BmiCalculatorTests
    {
        [Fact]
        public void Calculate_ShouldReturnRoundedBmi_WhenWeightAndHeightAreValid()
        {
            // Arrange
            var calculator = new BmiCalculator();

            // Act
            var result = calculator.Calculate(80m, 180m);

            // Assert
            Assert.Equal(24.7m, result);
        }

        [Theory]
        [InlineData(18.4, BmiCategory.Underweight)]
        [InlineData(18.5, BmiCategory.Normal)]
        [InlineData(24.9, BmiCategory.Normal)]
        [InlineData(25.0, BmiCategory.Overweight)]
        [InlineData(30.0, BmiCategory.Obesity)]
        public void GetCategory_ShouldReturnExpectedCategory_WhenBmiIsProvided(decimal bmi, BmiCategory expected)
        {
            // Arrange
            var calculator = new BmiCalculator();

            // Act
            var result = calculator.GetCategory(bmi);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
