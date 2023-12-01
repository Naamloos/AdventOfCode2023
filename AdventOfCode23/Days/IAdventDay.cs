using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23.Days
{
    public interface IAdventDay
    {
        /// <summary>
        /// Initialize this day's code.
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Run this day's code.
        /// </summary>
        /// <returns>Solution</returns>
        public string Run();
    }
}
