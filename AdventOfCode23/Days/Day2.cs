using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Days
{
    [AdventDay(2, "Cube Conundrum")]
    public class Day2 : IAdventDay
    {
        private readonly ILogger _logger;

        private string[] _input;

        const int red = 12;
        const int green = 13;
        const int blue = 14;

        public Day2(ILogger<Day2> logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            // Read the input file
            string inputValues = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("AdventOfCode23.Inputs.Day2.txt")).ReadToEnd();
            _input = inputValues.Split("\n");
        }

        public string Run()
        {
            int output_part1 = 0;
            int output_part2 = 0;

            foreach (var line in _input)
            {
                var possible = true;

                var id = int.Parse(line.Split(':')[0].Substring(5));
                var groups = line.Split(":")[1].Split(';');

                var redMin = 0;
                var greenMin = 0;
                var blueMin = 0;

                foreach(var group in groups)
                {
                    var colors = group.Split(',').Select(x => x.Trim().TrimEnd());
                    // format: 3 blue
                    foreach(var color in colors)
                    {
                        var amount = int.Parse(color.Split(' ')[0]);

                        if(color.EndsWith("red"))
                        {
                            if (amount > red)
                                possible = false;
                            if(amount > redMin)
                                redMin = amount;
                        }

                        if (color.EndsWith("green"))
                        {
                            if (amount > green)
                                possible = false;
                            if (amount > greenMin)
                                greenMin = amount;
                        }

                        if (color.EndsWith("blue"))
                        {
                            if (amount > blue)
                                possible = false;
                            if (amount > blueMin)
                                blueMin = amount;
                        }
                    }
                }

                if (possible)
                {
                    output_part1 += id;
                    _logger.LogDebug("ID {0} was possible. Current total part1: {1}", id, output_part1);
                }

                output_part2 += redMin * blueMin * greenMin;
                _logger.LogDebug("Power of ID {0} was {1}. Current total part2: {2}", id, redMin * blueMin * greenMin, output_part2);
            }

            return "Part1: " + output_part1.ToString() + " Part2: " + output_part2.ToString();
        }
    }
}
