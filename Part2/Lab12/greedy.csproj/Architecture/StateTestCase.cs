using System.Collections.Generic;
using System.Drawing;

namespace Greedy.Architecture
{
    public class StateTestCase
    {
        public string Name { get; set; }
        public string InputData { get; set; }
        public List<Point> ExpectedPath { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}