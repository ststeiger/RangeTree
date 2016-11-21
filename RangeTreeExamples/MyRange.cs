using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MB.Algodat;

namespace RangeTreeExamples
{


    public class MyRangeItem : IRangeProvider<long>
    {
        public string Text
        {
            get;
            set;
        }


        public Range<long> Range
        {
            get; set;
        }


        public MyRangeItem(long a, long b)
        {
            Range = new Range<long>(a, b);
            Text = a + " - " + b;
        }

        public MyRangeItem(long a, long b, string text)
        {
            Range = new Range<long>(a, b);
            Text = text;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} - {2})", Text, Range.From, Range.To);
        }



        public static bool operator ==(MyRangeItem left, MyRangeItem right)
        {
            return object.Equals(left, right);
        }


        public static bool operator !=(MyRangeItem left, MyRangeItem right)
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

            return this.Equals((MyRangeItem)obj);
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
