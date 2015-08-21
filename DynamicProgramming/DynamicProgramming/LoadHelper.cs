using System;

namespace DynamicProgramming
{
    public static class LoadHelper
    {
        public static int Parse(string s)
        {
            int value = 0;
            for (var i = 0; i < s.Length; i++)
            {
                value = value * 10 + (s[i] - '0');
            }
            return value;
        }

        public static decimal DecimalParse(string s)
        {
            decimal value = 0;
            for (var i = 2; i < s.Length; i++)
            {
                value = value * 10 + (s[i] - '0');
            }
            return value / 100000000;
        }

        public static int ParseBits(string s)
        {
            var bits = s.Split(' ', '\t');
            var result = 0;
            for (var i = 0; i < bits.Length; i++)
            {
                if (bits[i] == "1")
                    result += (int)Math.Pow(2, i);
            }
            return result;
        }
    }
}