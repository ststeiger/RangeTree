namespace RangeTreeExamples
{
    using System.Collections.Generic;

    /// <summary>
    /// Compares two range items by comparing their ranges.
    /// </summary>
    public class HugeRangeItemComparer : IComparer<HugeRangeItem>
    {
        public int Compare(HugeRangeItem x, HugeRangeItem y)
        {
            return x.Range.CompareTo(y.Range);
        }
    }
}