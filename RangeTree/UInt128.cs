
using System.Runtime.CompilerServices;


namespace MB.Algodat
{


    public struct UInt128 
        : System.IComparable
        , System.IComparable<UInt128>
        , System.Collections.Generic.IComparer<UInt128>
    {

        public int Compare(UInt128 x, UInt128 y)
        {
            if (x >  y) return -1;
            if (x == y) return 0;
            return 1;
        }


        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1; // https://msdn.microsoft.com/en-us/library/system.icomparable.compareto(v=vs.110).aspx

            System.Type t = obj.GetType();

            if(object.ReferenceEquals(t, typeof(UInt128)))
            {
                UInt128 ui = (UInt128)obj;
                return this.Compare(this, ui);
            }


            if (object.ReferenceEquals(t, typeof(System.DBNull)))
                return 1;

            ulong? lowerPart = obj as ulong?;
            if (!lowerPart.HasValue)
                return 1;

            UInt128 compareTarget = new UInt128(0, lowerPart.Value);
            return this.Compare(this, compareTarget);
        }


        public int CompareTo(UInt128 other)
        {
            if (this >  other) return -1;
            if (this == other) return 0;
            return 1;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt128(System.UInt64 n)
        {
            Low = n;
            High = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt128(System.UInt64 high, System.UInt64 low)
        {
            Low = low;
            High = high;
        }

        public ulong High;
        public ulong Low;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.UInt64(UInt128 value)
        {
            return value.Low;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator UInt128(System.UInt64 value)
        {
            return new UInt128(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMultiplyUInt64(ref System.UInt64 left, ref System.UInt64 right)
        {
            System.UInt64 a32 = left >> 32;
            System.UInt64 a00 = left & 0xffffffffu;

            System.UInt64 b32 = right >> 32;
            System.UInt64 b00 = right & 0xffffffffu;

            ulong high = a32 * b32;
            ulong low = a00 * b00;

            ulong addLow = (a32 * b00 + a00 * b32);
            ulong addHigh = addLow >> 32;
            addLow = addLow << 32;

            ulong c = (((low & addLow) & 1) + (low >> 1) + (addLow >> 1)) >> 63;
            high += addHigh + c;
            low += addLow;

            High = high;
            Low = low;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddMultiplyUInt64(ref System.UInt64 left, ref System.UInt64 right)
        {
            System.UInt64 a32 = left >> 32;
            System.UInt64 a00 = left & 0xffffffffu;

            System.UInt64 b32 = right >> 32;
            System.UInt64 b00 = right & 0xffffffffu;

            ulong high = a32 * b32;
            ulong low = a00 * b00;

            ulong addLow = (a32 * b00 + a00 * b32);
            ulong addHigh = addLow >> 32;
            addLow = addLow << 32;

            ulong c = (((low & addLow) & 1) + (low >> 1) + (addLow >> 1)) >> 63;
            high += addHigh + c;
            low += addLow;

            c = (((Low & low) & 1) + (Low >> 1) + (low >> 1)) >> 63;
            High = High + high + c;
            Low = Low + low;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 MultiplyUInt64(System.UInt64 left, System.UInt64 right)
        {
            System.UInt64 a32 = left >> 32;
            System.UInt64 a00 = left & 0xffffffffu;

            System.UInt64 b32 = right >> 32;
            System.UInt64 b00 = right & 0xffffffffu;

            ulong high = a32 * b32;

            ulong low = a00 * b00;

            ulong addLow = (a32 * b00 + a00 * b32);
            ulong addHigh = addLow >> 32;
            addLow = addLow << 32;

            ulong c = (((low & addLow) & 1) + (low >> 1) + (addLow >> 1)) >> 63;
            high += addHigh + c;
            low += addLow;

            return new UInt128(high, low);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(UInt128 n)
        {
            ulong c = (((Low & n.Low) & 1) + (Low >> 1) + (n.Low >> 1)) >> 63;
            High = High + n.High + c;
            Low = Low + n.Low;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(System.UInt64 n)
        {
            ulong c = (((Low & n) & 1) + (Low >> 1) + (n >> 1)) >> 63;
            High = High + c;
            Low = Low + n;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator == (UInt128 lhs, UInt128 rhs)
        {
            bool status = false;
            if (lhs.High == rhs.High && lhs.Low == rhs.Low)
            {
                status = true;
            }
            return status;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UInt128 lhs, UInt128 rhs)
        {
            bool status = false;
            if (lhs.High != rhs.High || lhs.Low != rhs.Low)
            {
                status = true;
            }
            return status;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(UInt128 lhs, UInt128 rhs)
        {
            if (lhs.High < rhs.High)
                return true;

            if (lhs.High == rhs.High && lhs.Low < rhs.Low)
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(UInt128 lhs, UInt128 rhs)
        {
            if (lhs.High > rhs.High)
                return true;

            if (lhs.High == rhs.High && lhs.Low > rhs.Low)
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(UInt128 lhs, UInt128 rhs)
        {
            if (lhs.High < rhs.High)
                return true;

            if (lhs.High == rhs.High)
            {
                if(lhs.Low <= rhs.Low)
                    return true;
            }

            return false;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(UInt128 lhs, UInt128 rhs)
        {
            if (lhs.High > rhs.High)
                return true;

            if (lhs.High == rhs.High)
            {
                if(lhs.Low >= rhs.Low)
                    return true;
            }

            return false;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator *(UInt128 left, System.UInt64 right)
        {
            unchecked
            {
                System.UInt64 a96 = left.High >> 32;
                System.UInt64 a64 = left.High & 0xffffffffu;
                System.UInt64 a32 = left.Low >> 32;
                System.UInt64 a00 = left.Low & 0xffffffffu;

                System.UInt64 b32 = right >> 32;
                System.UInt64 b00 = right & 0xffffffffu;

                // multiply [a96 .. a00] x [b96 .. b00]
                // terms higher than c96 disappear off the high side
                // terms c96 and c64 are safe to ignore carry bit
                System.UInt64 c96 = a96 * b00 + a64 * b32;
                System.UInt64 c64 = a64 * b00 + a32 * b32;

                ulong high = (c96 << 32) + c64;
                ulong low = a00 * b00;

                ulong addLow = (a32 * b00 + a00 * b32);
                ulong addHigh = addLow >> 32;
                addLow = addLow << 32;

                ulong c = (((low & addLow) & 1) + (low >> 1) + (addLow >> 1)) >> 63;
                high = high + addHigh + c;
                low = low + addLow;

                return new UInt128(high, low);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator +(UInt128 left, System.UInt64 right)
        {
            ulong c = (((left.Low & right) & 1) + (left.Low >> 1) + (right >> 1)) >> 63;
            ulong high = left.High + c;
            ulong low = left.Low + right;

            return new UInt128(high, low);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator +(UInt128 left, UInt128 right)
        {
            ulong c = (((left.Low & right.Low) & 1) + (left.Low >> 1) + (right.Low >> 1)) >> 63;
            ulong high = left.High + right.High + c;
            ulong low = left.Low + right.Low;

            return new UInt128(high, low);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator >>(UInt128 value, int shift)
        {
            UInt128 shifted = new UInt128();

            if (shift > 63)
            {
                shifted.Low = value.High >> (shift - 64);
                shifted.High = 0;
            }
            else
            {
                shifted.High = value.High >> shift;
                shifted.Low = (value.High << (64 - shift)) | (value.Low >> shift);
            }
            return shifted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator <<(UInt128 value, int shift)
        {
            UInt128 shifted = new UInt128();

            if (shift > 63)
            {
                shifted.High = value.Low << (shift - 64);
                shifted.Low = 0;
            }
            else
            {
                ulong ul = value.Low >> (64 - shift);
                shifted.High = ul | (value.High << shift);
                shifted.Low = value.Low << shift;
            }
            return shifted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator &(UInt128 left, System.UInt64 right)
        {
            UInt128 result = left;
            result.High = 0;
            result.Low &= right;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator &(UInt128 left, UInt128 right)
        {
            UInt128 result = left;
            result.High &= right.High;
            result.Low &= right.Low;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator |(UInt128 left, UInt128 right)
        {
            UInt128 result = left;
            result.High |= right.High;
            result.Low |= right.Low;
            return result;
        }
    }
}