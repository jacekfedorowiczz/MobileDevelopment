using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Services.Calculators;

namespace MobileDevelopment.API.UnitTests.Calculators
{
    public sealed class BmrCalculatorTests
    {
        [Fact]
        public void Calculate_ShouldUseMaleMifflinStJeorOffset_WhenGenderIsMale()
        {
            // Arrange
            var calculator = new BmrCalculator();

            // Act
            var result = calculator.Calculate(80m, 180m, 30, Gender.Male);

            // Assert
            Assert.Equal(1780m, result);
        }

        [Fact]
        public void Calculate_ShouldUseFemaleMifflinStJeorOffset_WhenGenderIsFemale()
        {
            // Arrange
            var calculator = new BmrCalculator();

            // Act
            var result = calculator.Calculate(65m, 165m, 30, Gender.Female);

            // Assert
            Assert.Equal(1320m, result);
        }
    }
}
