namespace RangeTreeExamples
{
    using System.Collections.Generic;

    /// <summary>
    /// Compares two range items by comparing their ranges.
    /// </summary>
    public class UInt128ItemComparer : IComparer<UInt128RangeItem>
    {
        public int Compare(UInt128RangeItem x, UInt128RangeItem y)
        {
            return x.Range.CompareTo(y.Range);
        }
    }
}