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

        public static void CalculateChange(double[] validChangeValues, double inputCost, double inputPayment) {
            // Calculate the distribution of valid change types
            if (inputCost == inputPayment) { Console.WriteLine("You paid with exact change!"); return; }    // Check if the user enterred exact change

            double changeToReturn = inputPayment - inputCost;       // Calculate the change delta
            int remainingChange = (int)(changeToReturn * 100);      // Our running total of change in pence -> gets round decimal issues e.g. when we sometimes get 0.9999999999998 instead of 1 with doubles
            Console.WriteLine("Item cost {0}, User paid {1} and the change to return is {2}", inputCost, inputPayment, changeToReturn);

            int[] changeDistribution = new int[validChangeValues.Length];     // Maps directly to the input change value array - e.g. changeValue[0] which is £50 notes directly matches changeDistribution[0] for its count

            // For each valid change type, check how it goes into the remaining value
            for (int i = 0; i < validChangeValues.Length; i++) {
                if (remainingChange == 0) { break; }                                    // We've run out of change to split
                int changeValueInPence = (int)(validChangeValues[i] * 100);             // Convert the change type to its value in pence
                if (changeValueInPence > (int)(changeToReturn * 100)) { continue; }     // Check we aren't using a change type higher than the change we are distributing
            }
        }

        static void Main(string[] args) {
            bool hasUserProvidedValidInputs = false;
            double[] validChangeTypes = new double[] { 50, 20, 10, 5, 2, 1, 0.50, 0.20, 0.10, 0.05, 0.02, 0.01 };

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
                    CalculateChange(validChangeTypes, productCost, userPayment);
                }
                else {
                    Console.WriteLine("The payment value is less than the cost, more money is required for the purchase");
                }
            }
        }
    }
}
