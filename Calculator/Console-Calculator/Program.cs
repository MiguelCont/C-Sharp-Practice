using System.Diagnostics.Contracts;

class Calculator
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Calculator");
        List<string> input = takeInput();
        double total = 0;
        bool error = false;
        try
        {
            total = mathLogic(input);
        }
        catch (Exception ex)
        {
            error = true;
            Console.WriteLine(ex.Message);
        }
        if(!error)
        {
            Console.WriteLine($"Your Answer is: {total}");
        }

    }   
    /*
        Gets the users input and santizes it, returning a list of the numbers and operators if the string is valid

        Example:
           input: "2+3+5"
           output: [2 ,+ ,3 ,+ ,5]
    */
    static List<string> takeInput()
    {
        Console.WriteLine("Enter your equation");
        bool valid = true;
        string input = Console.ReadLine();
        string noWhiteSpaceInput = input.Replace(" ", "");
        List<string> splitInput = new List<string>();
        string currNumber = "";

        foreach (char c in noWhiteSpaceInput)
        {
            if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                if (!double.TryParse(currNumber, out _) || c == noWhiteSpaceInput[noWhiteSpaceInput.Length - 1])
                {
                    valid = false;
                    break;
                }
                splitInput.Add(currNumber);
                splitInput.Add(c.ToString());
                currNumber = "";
            }
            else
            {
                currNumber += c.ToString();
            }
        }

        splitInput.Add(currNumber);

        if (!valid || splitInput.Count == 1)
        {
            Console.WriteLine("invalid");
            return new List<string>();
        }
        return splitInput;
    }

    private static double mathLogic(List<String> input) {

        // if order of operations occurs the results are put in here
        List<double> tempValues = new List<double>();
        int i = 1;
        bool oop = false;
        bool neg = false;
        int negCount = 0;
        if (input[1] == "+" || input[1] == "-")
        {
            tempValues.Add(double.Parse(input[0]));
        }
        while (i < input.Count)
        {
            string curr = input[i];
            if (neg)
            {
                if (negCount == 0)
                {
                    negCount++;
                }
                else
                {
                    negCount = 0;
                }
            }
            switch (curr)
            {
                case "+":
                    tempValues.Add(double.Parse(input[i + 1]));
                    if (oop)
                    {
                        oop = false;
                    }
                    break;
                case "-":
                    tempValues.Add(double.Parse(input[i + 1]) * -1);
                    neg = true;
                    negCount = 0;
                    if (oop)
                    {
                        oop = false;
                    }
                    break;
                case "*":
                    if (oop)
                    {
                        double temp = tempValues[tempValues.Count - 1];
                        tempValues.RemoveAt(tempValues.Count - 1);
                        if (neg)
                        {
                            tempValues.Add(temp * double.Parse(input[i + 1]) * -1);
                        }
                        else
                        {
                            tempValues.Add(temp * double.Parse(input[i + 1]));
                        }
                    }
                    else
                    {
                        tempValues.RemoveAt(tempValues.Count - 1);
                        if (neg)
                        { 
                            tempValues.Add(double.Parse(input[i - 1]) * double.Parse(input[i + 1])*-1);
                        }
                        else
                        {
                            tempValues.Add(double.Parse(input[i - 1]) * double.Parse(input[i + 1]));
                        }
                        

                    }
                    oop = true;
                    break;
                case "/":
                    if (double.Parse(input[i + 1]) == 0)
                    {
                        throw new ArithmeticException("Invalid Input, Cannot divide by zero");
                    }
                    if (oop)
                    {
                        double temp = tempValues[tempValues.Count - 1];
                        tempValues.RemoveAt(tempValues.Count - 1);
                        tempValues.Add(temp / double.Parse(input[i + 1]));
                    }
                    else
                    {
                        tempValues.RemoveAt(tempValues.Count - 1);
                        tempValues.Add(double.Parse(input[i - 1]) / double.Parse(input[i + 1]));
                    }
                    oop = true;
                    break;
                default:
                    break;
            }
            i++;
        }

        double total = 0;
        foreach (double val in tempValues)
        { 
            total += val;
        }
        return total;
    }
}
