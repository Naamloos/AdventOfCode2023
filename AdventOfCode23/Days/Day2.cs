using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Days
{
    [AdventDay(2, "Day 2 isn't out yet.")]
    public class Day2 : IAdventDay
    {
        private readonly ILogger _logger;

        public Day2(ILogger<Day2> logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInformation("Initializing Day2...");
        }

        public string Run()
        {
            _logger.LogInformation("Running Day2...");
            return "NOT YET AVAILABLE?";
        }
    }
}
