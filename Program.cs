using System;

namespace change_calculator
{
    class Program
    {

        private int ConvertToPence(double input)
        {
            return (int)(input * 100);
        }

        private static string RemovePoundSymbol(string inputString)
        {
            // Check if the string contains a £ - catchs inputs with pre-post £ symbols, inputs such as '3£4' will be marked invalid as theyre unclear
            if (inputString.Contains('£'))
            {
                inputString = inputString.Replace('£', ' ');    // Replace the £ with a space
                inputString = inputString.Trim();               // Trim the pre-post spaces
            }
            return inputString;
        }

        public static double GetUserInput(string messageToPrint)
        {
            // Get the user input and sanitise that its a valid numerical value we can use
            Console.WriteLine(messageToPrint);   // Print the message to console to instruct the user
            string inputString = RemovePoundSymbol(Console.ReadLine()); // Get input and remove £ symbols
            double inputConverted = 0;
            // Check the input can be converted to a double
            while (!double.TryParse(inputString, out inputConverted))
            {
                // Invalid input, inform the user and try again
                Console.WriteLine("Invalid value input, please make sure you are entering a number");
                inputString = RemovePoundSymbol(Console.ReadLine());
            }
            return inputConverted;
        }

        private static void OutputChangeDistribution(double[] validChangeValues, int[] changeDistribution)
        {
            // Output the change distribution to the console
            string changeOutputText = "\nThe distribution of the change is:";
            for (int i = 0; i < validChangeValues.Length; i++)
            {
                // Check if we are returning change of this type
                if (changeDistribution[i] == 0)
                {
                    continue;
                }

                // If the change value is above a pound use a £, if not use a pence symbol - E.g. £10 or 50p
                if (validChangeValues[i] >= 1)
                {
                    changeOutputText += string.Format("\n£{0} x {1}", validChangeValues[i], changeDistribution[i]);
                }
                else
                {
                    int readablePence = (int)(validChangeValues[i] * 100);
                    changeOutputText += string.Format("\n{0}p x {1}", readablePence, changeDistribution[i]);
                }
            }
            Console.WriteLine(changeOutputText);
        }

        public static void CalculateChange(double[] validChangeValues, double inputCost, double inputPayment)
        {
            // Calculate the distribution of valid change types

            // Check if the user enterred exact change
            if (inputCost == inputPayment)
            {
                Console.WriteLine("\nThe customer paid with exact change.");
                return;
            }

            double changeToReturn = inputPayment - inputCost;       // Calculate the change delta
            int remainingChange = (int)(changeToReturn * 100);      // Our running total of change in pence -> gets round decimal issues e.g. when we sometimes get 0.9999999999998 instead of 1 with doubles
            Console.WriteLine("\nThe product cost is £{0} and the customer paid £{1}. Therefore the change to be returned is {2}", inputCost, inputPayment, changeToReturn);

            int[] changeDistribution = new int[validChangeValues.Length];     // Maps directly to the input change value array - e.g. changeValue[0] which is £50 notes directly matches changeDistribution[0] for its count

            // For each valid change type, check how it goes into the remaining value
            for (int i = 0; i < validChangeValues.Length; i++)
            {
                // We've run out of change to split
                if (remainingChange == 0)
                {
                    break;
                }
                int changeValueInPence = (int)(validChangeValues[i] * 100);             // Convert the change type to its value in pence

                // Check we aren't using a change type higher than the change we are distributing
                if (changeValueInPence > (int)(changeToReturn * 100))
                {
                    continue;
                }

                // Calculate how the change is to be distributed
                int currentChangeToSplit = remainingChange - (remainingChange % changeValueInPence);    // Calculate the value to split with the current change type
                changeDistribution[i] = (currentChangeToSplit / changeValueInPence);                    // Record how many of the current change type we should give
                remainingChange -= currentChangeToSplit;                                                // Amend the running total after we've distributed some change
            }

            // Print the distribution to the console
            OutputChangeDistribution(validChangeValues, changeDistribution);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Programming Task - Change Calculator\n---------------------------------------------");
            double[] validChangeTypes = new double[] { 50, 20, 10, 5, 2, 1, 0.50, 0.20, 0.10, 0.05, 0.02, 0.01 };

            bool hasUserProvidedValidInputs = false;
            while (!hasUserProvidedValidInputs)
            {
                // Get user inputs
                double productCost = GetUserInput("Enter the product cost (E.g. £4.50):");
                if (productCost <= 0)
                {
                    Console.WriteLine("A purchase must cost more than £0...\n");
                    continue;
                }
                double userPayment = GetUserInput("Enter the value you are paying with (£20):");
                if (userPayment <= 0)
                {
                    Console.WriteLine("The payment value must be more than £0...\n");
                    continue;
                }
                // Check the user is paying enough for the product
                if (userPayment >= productCost)
                {
                    hasUserProvidedValidInputs = true;
                    CalculateChange(validChangeTypes, productCost, userPayment);
                }
                else
                {
                    Console.WriteLine("The payment value entered is less than the product cost, you must enter a higher payment value.");
                }
            }
        }
    }
}
