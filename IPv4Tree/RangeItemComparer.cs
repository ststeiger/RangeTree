namespace IPv4Tree
{
    using System.Collections.Generic;

    /// <summary>
    /// Compares two range items by comparing their ranges.
    /// </summary>
    public class IPv4ItemComparer : IComparer<IPv4Item>
    {
        public int Compare(IPv4Item x, IPv4Item y)
        {
            return x.Range.CompareTo(y.Range);
        }
    }
}