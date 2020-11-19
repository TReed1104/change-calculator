﻿using System;

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
            // Get the user input and sanitise that its a valid numerical value we can use
            Console.WriteLine(messageToPrint);   // Print the message to console to instruct the user
            string inputString = RemovePoundSymbol(Console.ReadLine()); // Get input and remove £ symbols
            double inputConverted = 0;
            // Check the input can be converted to a double
            while (!double.TryParse(inputString, out inputConverted)) {
                // Invalid input, inform the user and try again
                Console.WriteLine("Invalid value input, please make sure you are entering a number");
                inputString = RemovePoundSymbol(Console.ReadLine());
            }
            return inputConverted;
        }

        static void Main(string[] args) {
            bool hasUserProvidedValidInputs = false;
            while (!hasUserProvidedValidInputs) {
                // Get user inputs
                double productCost = GetUserInput("Enter the product cost (E.g. £4.50):");
                if (productCost <= 0) {
                    Console.WriteLine("A purchase must cost more than £0");
                    continue;
                }
                double userPayment = GetUserInput("Enter the value you are paying with (£20):");
                if (userPayment <= 0) {
                    Console.WriteLine("The payment value must be more than £0");
                    continue;
                }
                // Check the user is paying enough for the product
                if (userPayment >= productCost) {
                    hasUserProvidedValidInputs = true;
                }
                else {
                    Console.WriteLine("The payment value is less than the cost, more money is required for the purchase");
                }
            }
        }
    }
}
