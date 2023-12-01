using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Days
{
    [AdventDay(1, "Trebuchet?!")]
    public class Day1 : IAdventDay
    {
        private ILogger _logger;

        private Dictionary<string, int> _lookupTable;
        private string[] _input;
        private int _output;

        public Day1(ILogger<Day1> logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            _lookupTable = new()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 },
                { "ten", 10 }
            };

            _output = 0;

            // Read the input file
            string inputValues = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("AdventOfCode23.Inputs.Day1.txt")).ReadToEnd();
            _input = inputValues.Split("\n");
        }

        public string Run()
        {
            foreach (var value in _input)
            {
                // Find the first and last number in text.
                int first = FindFirstNumber(value, false);
                int last = FindFirstNumber(value, true);

                // Combine two numbers to one and add to the output value
                _output += int.Parse(first.ToString() + last.ToString());

                // Debug log progress...
                _logger.LogDebug("Combined detected value: {first}{last} Current total: {current}", first, last, _output);
            }

            return _output.ToString();
        }

        private int FindFirstNumber(string input, bool reverse)
        {
            // This working value will be modified as the loop progresses
            string workingValue = input;

            // if empty we exit the loop and just return 0, we found nothing.
            while (!string.IsNullOrEmpty(workingValue))
            {
                // First, check if first or last value is a number. If so, return that.
                if (int.TryParse(reverse ? workingValue.Last().ToString() : workingValue.First().ToString(), out int value))
                    return value;

                // Second, check if the string starts or ends with a value from our dictionary.
                foreach (var textualNumber in _lookupTable)
                    if (reverse && workingValue.EndsWith(textualNumber.Key) || workingValue.StartsWith(textualNumber.Key))
                        return textualNumber.Value; // Found! we return the numeric value associated

                // Nothing was found, we substring the working value and try again.
                workingValue = reverse ? workingValue.Substring(0, workingValue.Length - 1) : workingValue.Substring(1);
            }
            return 0;
        }
    }
}
