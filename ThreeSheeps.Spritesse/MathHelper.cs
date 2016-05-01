using System;

namespace ThreeSheeps.Spritesse
{
    public static class MathHelper
    {
        public static uint RountToPowerOfTwo(uint x)
        {
            if (x == 0)
                return 1;

            x--;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            return x + 1;
        }
    }
}
