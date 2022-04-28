#region Usings

using System;

#endregion

namespace UltimatePlaylist.Common.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal ToVariableDecimal(this decimal d)
        {
            try
            {
                return d.ToString("0.######").ToDecimal();
            }
            catch
            {
                return d;
            }
        }

        public static decimal ToVariableDecimal(this decimal d, int round)
        {
            try
            {
                return d.ToRound(round).ToString("0.######").ToDecimal();
            }
            catch
            {
                return d;
            }
        }

        public static decimal ToPercentage(this decimal d)
        {
            try
            {
                return (d * 100).ToRound(2);
            }
            catch
            {
                return d;
            }
        }

        public static decimal ToDecimal(this string str) => string.IsNullOrEmpty(str) ? 0 : Convert.ToDecimal(str);

        public static decimal ToRound(this decimal d, int decimals)
        {
            return Math.Round(d, decimals, MidpointRounding.AwayFromZero);
        }

        public static decimal ToBrixPlato(this decimal d, int round)
        {
            try
            {
                return d.ToRound(round);
            }
            catch
            {
                return d;
            }
        }

        public static int ToPoints(this decimal specificGravity)
        {
            return Convert.ToInt32((specificGravity - 1) * 1000);
        }

        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }
    }
}
