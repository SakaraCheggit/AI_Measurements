using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_Measurements
{
    internal static class CupSizeCalculator
    {
        private static readonly SortedDictionary<float, string> s_US = new SortedDictionary<float, string>
        {
            { 0.5f, "AA"},
            { 1f, "A"},
            { 2f, "B"},
            { 3f, "C"},
            { 4f, "D"},
            { 5f, "DD (E)"},
            { 6f, "F (DDD)"},
            { 7f, "G (DDDD)"},
            { 8f, "H"},
            { 9f, "I"},
            { 10f, "J"},
            { 11f, "K"},
            { 12f, "L"},
            { 13f, "M"},
            { 14f, "N"},
            { 15f, "O"},
        };
        private static readonly SortedDictionary<float, string> s_UK = new SortedDictionary<float, string>
        {
            { 0.5f, "AA"},
            { 1f, "A"},
            { 2f, "B"},
            { 3f, "C"},
            { 4f, "D"},
            { 5f, "DD"},
            { 6f, "E"},
            { 7f, "F"},
            { 8f, "FF"},
            { 9f, "G"},
            { 10f, "GG"},
            { 11f, "H"},
            { 12f, "HH"},
            { 13f, "J"},
            { 14f, "JJ"},
            { 15f, "K"},
        };
        private static readonly SortedDictionary<float, string> s_Europe = new SortedDictionary<float, string>
        {
            { 0.5f, "AA"},
            { 1f, "A"},
            { 2f, "B"},
            { 3f, "C"},
            { 4f, "D"},
            { 5f, "E"},
            { 6f, "F"},
            { 7f, "G"},
            { 8f, "H"},
        };

        public static string GetCupSize(float difference, Region region)
        {
            var cupSizes = GetCupSizesByRegion(region);
            if (difference > cupSizes.Keys.Max())
                return cupSizes[cupSizes.Keys.Max()];
            return cupSizes.First(cupSize => cupSize.Key > difference).Value;
        }

        private static SortedDictionary<float, string> GetCupSizesByRegion(Region region)
        {
            switch (region)
            {
                case Region.US:
                    return s_US;
                case Region.UK:
                    return s_UK;
                case Region.Europe:
                    return s_Europe;
                default:
                    throw new ArgumentException($"Unknown region: {region}");
            }
        }
    }
}
