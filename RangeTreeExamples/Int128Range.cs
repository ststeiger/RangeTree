
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MB.Algodat;


namespace RangeTreeExamples
{


    public class UInt128RangeItem : IRangeProvider<MB.Algodat.UInt128>
    {
        public string Text
        {
            get;
            set;
        }


        public Range<MB.Algodat.UInt128> Range
        {
            get; set;
        }


        public UInt128RangeItem(MB.Algodat.UInt128 a, MB.Algodat.UInt128 b)
        {
            Range = new Range<MB.Algodat.UInt128>(a, b);
            Text = a + " - " + b;
        }

        public UInt128RangeItem(MB.Algodat.UInt128 a, MB.Algodat.UInt128 b, string text)
        {
            Range = new Range<MB.Algodat.UInt128>(a, b);
            Text = text;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} - {2})", Text, Range.From, Range.To);
        }



        public static bool operator ==(UInt128RangeItem left, UInt128RangeItem right)
        {
            return object.Equals(left, right);
        }


        public static bool operator !=(UInt128RangeItem left, UInt128RangeItem right)
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
