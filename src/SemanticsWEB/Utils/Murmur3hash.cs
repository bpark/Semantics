using System;
using System.Text;

namespace SemanticsWEB.Utils
{
    /// <summary>
    /// Modified version of https://github.com/sebas77/Murmur3.net
    /// </summary>
    public static class Murmur3
    {
        public static uint Hash(string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var length = (uint) data.Length;
            const uint seed = 74636332;

            var nblocks = length >> 2;

            var h1 = seed;

            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;

            var i = 0;

            for (var j = nblocks; j > 0; --j)
            {
                var k1l = BitConverter.ToUInt32(data, i);

                k1l *= c1;
                k1l = Rotl32(k1l, 15);
                k1l *= c2;

                h1 ^= k1l;
                h1 = Rotl32(h1, 13);
                h1 = h1 * 5 + 0xe6546b64;

                i += 4;
            }

            nblocks <<= 2;

            uint k1 = 0;

            var tailLength = length & 3;

            if (tailLength == 3)
                k1 ^= (uint) data[2 + nblocks] << 16;
            if (tailLength >= 2)
                k1 ^= (uint) data[1 + nblocks] << 8;
            if (tailLength >= 1)
            {
                k1 ^= data[nblocks];
                k1 *= c1;
                k1 = Rotl32(k1, 15);
                k1 *= c2;
                h1 ^= k1;
            }

            h1 ^= length;

            h1 = Fmix32(h1);

            return h1;
        }

        static uint Fmix32(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return h;
        }

        static uint Rotl32(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }
    }
}