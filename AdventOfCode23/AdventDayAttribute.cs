using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23
{
    public class AdventDayAttribute : Attribute
    {
        public int Day { get; private set; }
        public string Name { get; private set; }

        public AdventDayAttribute(int day, string name) 
        {
            this.Day = day;
            this.Name = name;
        }
    }
}
