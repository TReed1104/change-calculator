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
        [InlineData("£", "")]
        [InlineData("£50", "50")]
        [InlineData("50£", "50")]
        [InlineData("%50", "%50")]
        [InlineData("£££££", "")]
        [InlineData("dsa£", "dsa")]
        [InlineData("a b c", "a b c")]
        public void Test_RemovePoundSymbol_Equal(string input, string expected)
        {
            Assert.Equal(expected, Program.RemovePoundSymbol(input));
        }

        [Theory]
        [InlineData("5£0", "50")]
        [InlineData("£5£0", "50")]
        [InlineData("50", "£50")]
        [InlineData("5 0", "£50")]
        public void Test_RemovePoundSymbol_NotEqual(string input, string expected)
        {
            Assert.NotEqual(expected, Program.RemovePoundSymbol(input));
        }

        [Theory]
        [InlineData("50", 50)]
        [InlineData("£50", 50)]
        [InlineData("200", 200)]
        [InlineData("5.1", 5.1)]
        [InlineData("2.939393", 2.939393)]
        [InlineData("5£0", -1)]
        [InlineData("abcdef", -1)]
        public void Test_ParseUserInput_Equal(string input, double expected)
        {
            Assert.Equal(expected, Program.ParseUserInput(input));
        }

        [Theory]
        [InlineData("5£0", 50)]
        [InlineData("£5£0", 50)]
        [InlineData("£abc", 50)]
        [InlineData("abcdef", double.MaxValue)]
        [InlineData("abcdef", double.MinValue)]
        public void Test_ParseUserInput_NotEqual(string input, double expected)
        {
            Assert.NotEqual(expected, Program.ParseUserInput(input));
        }


        [Theory]
        [InlineData(20, 5.5, new int[] { 0, 0, 1, 0, 2, 0, 1, 0, 0, 0, 0, 0 })]
        [InlineData(50, 10, new int[] { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(20, 10, new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(5, 3.53, new int[] { 0, 0, 0, 0, 0, 1, 0, 2, 0, 1, 1, 0 })]
        public void Test_CalculateChange_Equal(double inputCost, double inputPayment, int[] expected)
        {
            Assert.Equal(expected, Program.CalculateChange(Program.validChangeTypes, inputCost, inputPayment));
        }

        [Theory]
        [InlineData(20, 5.5, new int[] { 0, 1, 1, 0, 2, 0, 1, 0, 0, 0, 0, 1 })]
        [InlineData(0, 10, new int[] { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(20, 0, new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(5, 3.53, new int[] { 0, 1, 0, 0, 0, 1, 0, 2, 0, 1, 1, 0 })]
        public void Test_CalculateChange_NotEqual(double inputCost, double inputPayment, int[] expected)
        {
            Assert.NotEqual(expected, Program.CalculateChange(Program.validChangeTypes, inputCost, inputPayment));
        }


        [Theory]
        [InlineData(new int[] { 0, 0, 1, 0, 2, 0, 1, 0, 0, 0, 0, 0 }, "Your change is:\n1 x £10\n2 x £2\n1 x 50p\n")]
        [InlineData(new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Your change is:\n1 x £50\n")]
        [InlineData(new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, "Your change is:\n1 x £50\n1 x 1p\n")]
        [InlineData(new int[] { 0, 0, 0, 5, 0, 0, 0, 0, 0, 12, 0, 1 }, "Your change is:\n5 x £5\n12 x 5p\n1 x 1p\n")]
        [InlineData(new int[] { 1 }, "Invalid Array Length")]
        public void Test_OutputChangeDistribution_Equal(int[] input, string expected)
        {
            Assert.Equal(expected, Program.OutputChangeDistribution(Program.validChangeTypes, input));
        }

        [Theory]
        [InlineData(new int[] { 0, 0, 1, 0, 2, 0, 1, 0, 0, 0, 0, 0 }, "you have some change")]
        [InlineData(new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Your change is:\n1 x £20\n")]
        [InlineData(new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, "Your change is:\n1 x 1p\n")]
        [InlineData(new int[] { 0, 0, 0, 5, 0, 0, 0, 0, 0, 12, 0, 1 }, "Your change is:\n5 x £5\n12 x 5p\n0 x 1p\n")]
        [InlineData(new int[] { 1 }, "Your change is:\n1 x £50")]
        public void Test_OutputChangeDistribution_NotEqual(int[] input, string expected)
        {
            Assert.NotEqual(expected, Program.OutputChangeDistribution(Program.validChangeTypes, input));
        }
    }

}
