using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MB.Algodat;

namespace RangeTreeExamples
{


    public class HugeRangeItem : IRangeProvider<System.Numerics.BigInteger>
    {
        public string Text
        {
            get;
            set;
        }


        public Range<System.Numerics.BigInteger> Range
        {
            get; set;
        }


        public HugeRangeItem(System.Numerics.BigInteger a, System.Numerics.BigInteger b)
        {
            Range = new Range<System.Numerics.BigInteger>(a, b);
            Text = a + " - " + b;
        }

        public HugeRangeItem(System.Numerics.BigInteger a, System.Numerics.BigInteger b, string text)
        {
            Range = new Range<System.Numerics.BigInteger>(a, b);
            Text = text;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} - {2})", Text, Range.From, Range.To);
        }



        public static bool operator ==(HugeRangeItem left, HugeRangeItem right)
        {
            return object.Equals(left, right);
        }


        public static bool operator !=(HugeRangeItem left, HugeRangeItem right)
        {
            return !object.Equals(left, right);
        }


        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != this.GetType())
                return false;

            return this.Equals((HugeRangeItem)obj);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                return ((Text != null ? Text.GetHashCode() : 0) * 397) ^ Range.GetHashCode();
            }
        }


    }


}
