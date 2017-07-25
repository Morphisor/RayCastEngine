using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Utils
{
    public static class Extensions
    {
        public static double ToDegrees(this double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static double ToRadians(this double degrees)
        {
            if (degrees == 90)
                return Math.PI / 2;

            if (degrees == 270)
                return 3 * Math.PI / 2;

            return degrees * (Math.PI / 180);
        }

        public static void SwapValues<T>(this T[] source, long firstIndex, long secondIndex)
        {
            T temp = source[firstIndex];
            source[firstIndex] = source[secondIndex];
            source[secondIndex] = temp;
        }
    }
}
