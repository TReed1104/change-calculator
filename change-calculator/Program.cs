using System;

namespace change_calculator
{
    public class Program
    {
        // Valid change types to use
        public readonly static double[] validChangeTypes = new double[] { 50, 20, 10, 5, 2, 1, 0.50, 0.20, 0.10, 0.05, 0.02, 0.01 };

        // Convert our decimal £ value to an int of its pence value
        public static int ConvertToPence(double input)
        {
            return (int)(input * 100);
        }

        public static string RemovePoundSymbol(string inputString)
        {
            // Check if the string contains a £ - catchs inputs with pre-post £ symbols, inputs such as '3£4' will be marked invalid as theyre unclear
            if (inputString.Contains('£'))
            {
                inputString = inputString.Replace('£', ' ');    // Replace the £ with a space
                inputString = inputString.Trim();               // Trim the pre-post spaces
            }
            return inputString;
        }

        public static double ParseUserInput(string input)
        {
            // Get the user input and sanitise that its a valid numerical value we can use
            string inputString = RemovePoundSymbol(input); // Get input and remove £ symbols
            double inputConverted = 0;
            // Check the input can be converted to a double
            if (!double.TryParse(inputString, out inputConverted))
            {
                return -1;
            }
            return inputConverted;
        }

        public static string OutputChangeDistribution(double[] validChangeValues, int[] changeDistribution)
        {
            // Check the change type and distribution arrays are the same length
            if (validChangeValues.Length != changeDistribution.Length)
            {
                return "Invalid Array Length";
            }

            // Output the change distribution to the console
            string changeOutputText = "Your change is:";
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
                    changeOutputText += string.Format("\n{0} x £{1}", changeDistribution[i], validChangeValues[i]);
                }
                else
                {
                    changeOutputText += string.Format("\n{0} x {1}p", changeDistribution[i], ConvertToPence(validChangeValues[i]));
                }
            }
            return changeOutputText + "\n";
        }

        public static int[] CalculateChange(double[] validChangeValues, double inputPayment, double inputCost)
        {
            // Calculate the distribution of valid change types

            // Check if the user enterred exact change
            if (inputCost == inputPayment)
            {
                Console.WriteLine("\nThe customer paid with exact change.");
                return null;
            }

            int changeToReturn = ConvertToPence(inputPayment) - ConvertToPence(inputCost);       // Calculate the change delta
            int remainingChange = changeToReturn;      // Our running total of change in pence -> gets round decimal issues e.g. when we sometimes get 0.9999999999998 instead of 1 with doubles
            Console.WriteLine("\nGiven £{0} and a product price £{1} - the change due is £{2}", inputPayment, inputCost, ((double)(remainingChange) / 100));

            int[] changeDistribution = new int[validChangeValues.Length];     // Maps directly to the input change value array - e.g. changeValue[0] which is £50 notes directly matches changeDistribution[0] for its count

            // For each valid change type, check how it goes into the remaining value
            for (int i = 0; i < validChangeValues.Length; i++)
            {
                // We've run out of change to split
                if (remainingChange == 0)
                {
                    break;
                }
                int changeValueInPence = ConvertToPence(validChangeValues[i]);             // Convert the change type to its value in pence

                // Check we aren't using a change type higher than the change we are distributing
                if (changeValueInPence > changeToReturn)
                {
                    continue;
                }

                // Calculate how the change is to be distributed
                int currentChangeToSplit = remainingChange - (remainingChange % changeValueInPence);    // Calculate the value to split with the current change type
                changeDistribution[i] = (currentChangeToSplit / changeValueInPence);                    // Record how many of the current change type we should give
                remainingChange -= currentChangeToSplit;                                                // Amend the running total after we've distributed some change
            }

            // Print the distribution to the console
            Console.WriteLine(OutputChangeDistribution(validChangeValues, changeDistribution));
            // Return the change distribution
            return changeDistribution;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Programming Task - Change Calculator\n---------------------------------------------");
            bool hasUserProvidedValidInputs = false;
            while (!hasUserProvidedValidInputs)
            {
                // Get user inputs
                Console.WriteLine("Enter the value you are paying with (£20):");
                double payment = ParseUserInput(Console.ReadLine());
                if (payment <= 0)
                {
                    Console.WriteLine("An invalid payment value was entered, the payment value must be a number and more than £0...\n");
                    continue;
                }

                Console.WriteLine("Enter the product cost (E.g. £4.50):");
                double productCost = ParseUserInput(Console.ReadLine());
                if (productCost <= 0)
                {
                    Console.WriteLine("An invalid purchase value was entered, the purchase value must be a number and more than £0...\n");
                    continue;
                }

                // Check the user is paying enough for the product
                if (payment >= productCost)
                {
                    hasUserProvidedValidInputs = true;
                    CalculateChange(validChangeTypes, payment, productCost);
                }
                else
                {
                    Console.WriteLine("The payment value entered is less than the product cost, you must enter a higher payment value.");
                }
            }
            Console.ReadLine(); // Hacky end of program break so the user can read the output
        }
    }
}
