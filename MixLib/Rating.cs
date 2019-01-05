using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixLib
{
    class Rating
    {
        public int one;
        public int two;
        public int three;
        public int four;
        public int five;
        public int total;
        public Rating(int one, int two, int three, int four, int five, int total)
        {
            this.one = one;
            this.two = two;
            this.three = three;
            this.four = four;
            this.five = five;
            this.total = total;
        }
    }
}
