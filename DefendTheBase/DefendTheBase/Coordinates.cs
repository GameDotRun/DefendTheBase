using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefendTheBase
{
    public class Coordinates
    {
        public int x, y, counter;
        public Coordinates(int X, int Y, int COUNTER)
        {
            x = X;
            y = Y;
            counter = COUNTER;
        }

        public Coordinates(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }
}
