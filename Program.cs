using System;

namespace change_calculator {
    class Program {
        private static string RemovePoundSymbol(string inputString) {
            // Check if the string contains a £ - catchs inputs with pre-post £ symbols, inputs such as '3£4' will be marked invalid as theyre unclear
            if (inputString.Contains('£')) {
                inputString = inputString.Replace('£', ' ');    // Replace the £ with a space
                inputString = inputString.Trim();               // Trim the pre-post spaces
            }
            return inputString;
        }

        public static double GetUserInput(string messageToPrint) {
            return 0;
        }

        static void Main(string[] args) {
            // Get input for product price
            Console.WriteLine("Enter the product cost");
            string productPrice = RemovePoundSymbol(Console.ReadLine());
            double priceConverted = 0;
            // Check the input can be converted to a double
            while (!double.TryParse(productPrice, out priceConverted)) {
                // Invalid input, inform the user and try again
                Console.WriteLine("Invalid value input");
                productPrice = RemovePoundSymbol(Console.ReadLine());
            }

            // Get input for product price
            Console.WriteLine("Enter the value you are paying");
            string paymentValue = RemovePoundSymbol(Console.ReadLine());
            double paymentConverted = 0;
            // Check the input can be converted to a double
            while (!double.TryParse(paymentValue, out paymentConverted)) {
                // Invalid input, inform the user and try again
                Console.WriteLine("Invalid value input");
                paymentValue = RemovePoundSymbol(Console.ReadLine());
            }
        }
    }
}
