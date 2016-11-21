using System;
using System.Linq;
using System.Text;
using MB.Algodat;

namespace IPv4Tree
{




    /// <summary>
    /// A simple example cass, which contains an integer range
    /// and a text property.
    /// </summary>
    public class IPv4Item : IRangeProvider<uint>
    {
        public IPv4Item(uint a, uint b)
        {
            this.Range = new Range<uint>(a, b);
            this.Text = a + " - " + b;
        }

        public IPv4Item(uint a, uint b, string text)
        {
            this.Range = new Range<uint>(a, b);
            this.Text = text;
        }

        public IPv4Item(uint a, uint b, GeoLiteLocationImporter.GeoLiteLocation location)
        {
            this.Range = new Range<uint>(a, b);
            this.Text = a + " - " + b;
            this.Location = location;
        }

        public string Text
        {
            get;
            set;
        }


        public GeoLiteLocationImporter.GeoLiteLocation Location
        {
            get;
            set;
        }

        public Range<uint> Range
        {
            get;
            set;
        }


        public override string ToString()
        {
            return string.Format("{0} ({1} - {2})", Text, Range.From, Range.To);
        }

        protected bool Equals(IPv4Item other)
        {
            return string.Equals(Text, other.Text) && Range.Equals(other.Range);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((IPv4Item)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Text != null ? Text.GetHashCode() : 0) * 397) ^ Range.GetHashCode();
            }
        }

        public static bool operator ==(IPv4Item left, IPv4Item right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IPv4Item left, IPv4Item right)
        {
            return !Equals(left, right);
        }
    }
}
