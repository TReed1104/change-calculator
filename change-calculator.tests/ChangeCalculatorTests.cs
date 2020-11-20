using System;
using Xunit;
using change_calculator;

namespace change_calculator.tests
{
    public class ChangeCalculatorTests
    {
        [Theory]
        [InlineData(1, 100)]
        [InlineData(10, 1000)]
        [InlineData(55, 5500)]
        public void Test_ConvertToPence_CorrectValue(double input, int expected)
        {
            // Correct expected result
            Assert.Equal(expected, Program.ConvertToPence(input));
        }

        [Theory]
        [InlineData(2, 100)]
        [InlineData(200, 100)]
        [InlineData(232, 55500)]
        public void Test_ConvertToPence_IncorrectValue(double input, int expected)
        {
            // Incorrect expected result
            Assert.NotEqual(expected, Program.ConvertToPence(input));
        }

        [Theory]
        [InlineData(5.5, 550)]
        [InlineData(3.3, 330)]
        [InlineData(3.42, 342)]
        public void Test_ConvertToPence_DecimalValue(double input, int expected)
        {
            Assert.Equal(expected, Program.ConvertToPence(input));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("�", "")]
        [InlineData("�50", "50")]
        [InlineData("50�", "50")]
        [InlineData("%50", "%50")]
        [InlineData("�����", "")]
        [InlineData("dsa�", "dsa")]
        [InlineData("a b c", "a b c")]
        public void Test_RemovePoundSymbol_Equal(string input, string expected)
        {
            Assert.Equal(expected, Program.RemovePoundSymbol(input));
        }

        [Theory]
        [InlineData("5�0", "50")]
        [InlineData("�5�0", "50")]
        [InlineData("50", "�50")]
        [InlineData("5 0", "�50")]
        public void Test_RemovePoundSymbol_NotEqual(string input, string expected)
        {
            Assert.NotEqual(expected, Program.RemovePoundSymbol(input));
        }

    }

}
