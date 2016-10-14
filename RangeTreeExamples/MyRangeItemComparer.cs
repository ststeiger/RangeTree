namespace RangeTreeExamples
{
    using System.Collections.Generic;

    /// <summary>
    /// Compares two range items by comparing their ranges.
    /// </summary>
    public class MyRangeItemComparer : IComparer<MyRangeItem>
    {
        public int Compare(MyRangeItem x, MyRangeItem y)
        {
            return x.Range.CompareTo(y.Range);
        }
    }
}