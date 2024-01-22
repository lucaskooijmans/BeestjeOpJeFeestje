using System.Diagnostics.CodeAnalysis;
using BeestjeOpJeFeestje.Domain;
using Domain.Models;
using Moq;

namespace PROG6_BeestjeOpJeFeestje.Test
{
    [ExcludeFromCodeCoverage]
    public class DiscountCalculatorTest
    {
        private DiscountCalculator _calc;

        [Fact]
        public void TypeDiscount_DiscountTrue_Test()
        {
            //1. Arange
            _calc = new DiscountCalculator();

            var animals = new List<Animal>
            {
                new Animal
                {
                    Name = "Koe",
                    Price = 100,
                    Type = "Boerderij",
                    Id = 1,
                    ImageUrl = "https://www.creativemotion.nl/wp-content/uploads/2023/08/unnamed.png",
                    IsVip = false
                },
                new Animal
                {
                    Name = "Paard",
                    Price = 100,
                    Type = "Boerderij",
                    Id = 2,
                    ImageUrl = "https://www.creativemotion.nl/wp-content/uploads/2023/08/unnamed.png",
                    IsVip = false
                },
                new Animal
                {
                    Name = "Varken",
                    Price = 100,
                    Type = "Boerderij",
                    Id = 0,
                    ImageUrl = null,
                    IsVip = false
                }
            };

            //2. Act
            var result = _calc.TypeDiscount(animals);

            //3. Assert
            Assert.Equal(10, result.PercentageDiscount);
        }

        [Fact]
        public void TypeDiscount_DiscountFalse_Test()
        {
            //1. Arange
            _calc = new DiscountCalculator();

            var animals = new List<Animal>
            {
                new Animal
                {
                    Name = "Koe",
                    Price = 100,
                    Type = "Boerderij",
                    Id = 1,
                    ImageUrl = "https://www.creativemotion.nl/wp-content/uploads/2023/08/unnamed.png",
                    IsVip = false
                },
                new Animal
                {
                    Name = "Paard",
                    Price = 100,
                    Type = "Boerderij",
                    Id = 2,
                    ImageUrl = "https://www.creativemotion.nl/wp-content/uploads/2023/08/unnamed.png",
                    IsVip = false
                },
                new Animal
                {
                    Name = "Hagedis",
                    Price = 200,
                    Type = "Woestijn",
                    Id = 3,
                    ImageUrl = "https://www.creativemotion.nl/wp-content/uploads/2023/08/unnamed.png",
                    IsVip = false
                }
            };

            //2. Act
            var result = _calc.TypeDiscount(animals);

            //3. Assert
            Assert.Null(result);
        }
        
        // public void CalculateCharacterDiscount_DiscountFalse_Test()
        // {
        //     //1. Arrange
        //     _calc = new DiscountCalculator();
        //     const string bono = "bono";
        //     const int bonoDiscountShouldBe = 0;
        //
        //     //2. Act
        //     var actualResult = _calc.CalculateCharacterDiscount(bono);
        //
        //     //3. Assert
        //     Assert.Equal(bonoDiscountShouldBe, actualResult);
        // }

        // public void CalculateCharacterDiscount_DiscountTrue_Test()
        // {
        //     //1. Arrange
        //     _calc = new DiscountCalculator();
        //     const string abc = "abc";
        //     const int abcDiscountShouldBe = 6;
        //
        //     //2. Act
        //     var actualResult = _calc.CalculateCharacterDiscount(abc);
        //
        //     //3. Assert
        //     Assert.Equal(abcDiscountShouldBe, actualResult);
        // }

        [Fact]
        public void DuckDiscount_DuckDiscountTrue_Test()
        {
            //1. Arrange
            _calc = new DiscountCalculator();
            const string duckName = "Eend"; 
            const int expectedDuckDiscount = 50;
            var randomMock = new Mock<Random>();
            randomMock.Setup(r => r.Next(6)).Returns(1);

            //2. Act
            var discount = _calc.DuckDiscount(duckName, randomMock.Object.Next(6));

            //3. Assert
            if (discount != null) Assert.Equal(expectedDuckDiscount, discount.PercentageDiscount);
        }
        
        [Fact]
        public void DuckDiscount_DuckDiscountFalse_Test()
        {
            //1. Arrange
            _calc = new DiscountCalculator();
            const string duckName = "Eend";
            var randomMock = new Mock<Random>();
            randomMock.Setup(r => r.Next(6)).Returns(3);

            //2. Act
            var discount = _calc.DuckDiscount(duckName, randomMock.Object.Next(6));

            //3. Assert
            Assert.Null(discount);
        }

        [Fact]
        public void DateDiscount_DiscountTrue_Test()
        {
            //1. Arrange
            _calc = new DiscountCalculator();
            const int expectedDateDiscount = 15;
            var monday = new DateTime(2020, 01, 06);
            var tuesday = new DateTime(2020, 01, 07);

            //2. Act
            var resultMonday = _calc.DateDiscount(monday);
            var resultTuesday = _calc.DateDiscount(tuesday);

            //3. Assert
            Assert.Equal(expectedDateDiscount, resultMonday.PercentageDiscount);
            Assert.Equal(expectedDateDiscount, resultTuesday.PercentageDiscount);
        }
        
        [Fact]
        public void DateDiscount_DiscountFalse_Test()
        {
            //1. Arrange
            _calc = new DiscountCalculator();
            var wednesday = new DateTime(2020, 01, 08);
            var thursday = new DateTime(2020, 01, 09);
            var friday = new DateTime(2020, 01, 10);
            var saturday = new DateTime(2020, 01, 11);
            var sunday = new DateTime(2020, 01, 12);

            //2. Act
            var resultWednesday = _calc.DateDiscount(wednesday);
            var resultThursday = _calc.DateDiscount(thursday);
            var resultFriday = _calc.DateDiscount(friday);
            var resultSaturday = _calc.DateDiscount(saturday);
            var resultSunday = _calc.DateDiscount(sunday);

            //3. Assert
            Assert.Null(resultWednesday);
            Assert.Null(resultThursday);
            Assert.Null(resultFriday);
            Assert.Null(resultSaturday);
            Assert.Null(resultSunday);
        }

        [Fact]
        public void CalculateMaxDiscount_ShouldLimitDiscountTo60()
        {
            // Arrange
            var calculator = new DiscountCalculator();

            // Act
            var result = calculator.CalculateMaxDiscount(70, 15); // Assuming total discount is 70 and discount is 15

            // Assert
            Assert.Equal(5, result); // The discount should be adjusted to make the total discount 60
            
            Assert.Equal(60, calculator.GetTotalDiscount()); // Total discount should be limited to 60
        }
    }
}