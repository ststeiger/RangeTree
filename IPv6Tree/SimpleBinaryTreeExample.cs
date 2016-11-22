
using MB.Algodat;


namespace IPv6Tree
{


    // http://stackoverflow.com/questions/11868837/fastest-way-to-search-a-number-in-a-list-of-ranges
    public class RangeGroup<T> where T : System.IComparable<T> 
    {
        public string RangeGroupId { get; set; }
        public T Low { get; set; }
        public T High { get; set; }

        // More properties related with the range here
        public T HitValue { get; set; }
        
        
        public RangeGroup<T> Clone()
        {
            var rg = new RangeGroup<T>();
            rg.RangeGroupId = this.RangeGroupId;
            rg.Low = this.Low;
            rg.High = this.High;

            return rg;
        }

    }


    public class RangeGroupFinder<T> where T : System.IComparable<T> 
    {
        private readonly RangeGroup<T>[] m_rangeGroups;


        public RangeGroupFinder(System.Collections.Generic.List<RangeGroup<T>> rangeGroup)
        {
            this.m_rangeGroups = rangeGroup.ToArray();

            // Ensure that the groups are sequential - there should be no overlaps
            System.Array.Sort(this.m_rangeGroups, delegate(RangeGroup<T> obj1, RangeGroup<T> obj2)
                {
                    return obj1.Low.CompareTo(obj2.Low);
                }
            );

        }


        public RangeGroup<T> Find(T number)
        {
            int position = m_rangeGroups.Length / 2;
            int stepSize = position / 2;

            /*
            1-10
            11-20
            21-30
            31-40
            41-50

            5 ==> position = 2.5 = 2 ==> stepsize = 1
            */

            int iMaxRange = m_rangeGroups.Length - 1;

            while (true)
            {
                // https://msdn.microsoft.com/en-us/library/system.icomparable.compareto(v=vs.110).aspx
                // a.CompareTo(b)  <0  ==> a < b  Less than zero: This instance precedes b in the sort order.
                // a.CompareTo(b)  = 0 ==> a = b  Zero This instance occurs in the same position in the sort order as b.
                // a.CompareTo(b) > 0 ==> a > b   Greater than zero This instance follows obj in the sort order.


                // if (m_rangeGroups[position].High < number)
                if (m_rangeGroups[position].High.CompareTo(number) < 0) 
                {
                    // Search up
                    position += stepSize;

                }
                //else if (m_rangeGroups[position].Low > number)
                else if (m_rangeGroups[position].Low.CompareTo(number) > 0)
                {
                    // Search down
                    position -= stepSize;

                }
                // else if (m_rangeGroups[position].Low <= number && m_rangeGroups[position].High >= number)
                else if (m_rangeGroups[position].Low.CompareTo(number) <= 0 && m_rangeGroups[position].High.CompareTo(number) >= 0)
                {
                    // Found it!
                    RangeGroup<T> match = m_rangeGroups[position].Clone();
                    match.HitValue = number;

                    return match;
                }
               

                if (position < 0 || position > iMaxRange)
                {
                    // Not found
                    return null;
                }

                stepSize /= 2;

                if (stepSize == 0)
                    stepSize = 1;
            } // Whend 

        } // End Function Find 

    } // End Class RangeGroupFinder 


    class SimpleBinaryTreeExample
    {


        public static void Test2()
        {
            System.Collections.Generic.List<UInt128RangeItem> ls = GeoLiteImporter.DirectImport();

            System.Collections.Generic.List<RangeGroup<UInt128>> lsRangeGroups = new System.Collections.Generic.List<RangeGroup<UInt128>>();
            // Populating the list items here
            foreach (UInt128RangeItem thisItem in ls)
            {
                lsRangeGroups.Add(new RangeGroup<UInt128> { RangeGroupId = thisItem.Text, Low = thisItem.Range.From, High = thisItem.Range.To });
            }

            RangeGroupFinder<UInt128> finder = new RangeGroupFinder<UInt128>(lsRangeGroups);

            // UInt128 ipv6 = IPv6Helper.IP2num("a374:fc53:b27e:32d6:f1b7:e1ff:a829:cdc5");
            UInt128 ipv6 = IPv6Helper.IP2num("2607:f0d0:1002:0051:0000:0000:0000:0004");
            
            

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            RangeGroup<UInt128> rg1 = finder.Find(ipv6);
            sw.Stop();

            System.Console.WriteLine(sw.Elapsed);
            System.Console.WriteLine(rg1);
        } // End Sub Test2 


        private static System.Collections.Generic.List<RangeGroup<uint>> GenerateRangeGroup()
        {
            System.Collections.Generic.List<RangeGroup<uint>> m_RangeGroups = new System.Collections.Generic.List<RangeGroup<uint>>();
            // Populating the list items here
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "0", Low = 1, High = 5 });
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "1", Low = 6, High = 10 });
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "2", Low = 11, High = 15 });
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "3", Low = 16, High = 30 });
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "4", Low = 31, High = 100 });
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "5", Low = 101, High = 1000 });
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "6", Low = 1001, High = 5000 });
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "7", Low = 5001, High = 100000 });
            m_RangeGroups.Add(new RangeGroup<uint> { RangeGroupId = "8", Low = 100001, High = 1000000 });

            return m_RangeGroups;
        } // End Function GenerateRangeGroup 


        public static void Test()
        {
            System.Collections.Generic.List<RangeGroup<uint>> ls = GenerateRangeGroup();
            RangeGroupFinder<uint> finder = new RangeGroupFinder<uint>(ls);

            RangeGroup<uint> rg1 = finder.Find(123);
            RangeGroup<uint> rg2 = finder.Find(1000000);
            RangeGroup<uint> rg3 = finder.Find(1000);
            RangeGroup<uint> rg4 = finder.Find(0);
            RangeGroup<uint> rg5 = finder.Find(1);
            RangeGroup<uint> rg6 = finder.Find(1000001);

            System.Console.WriteLine(rg1);
            System.Console.WriteLine(rg2);
            System.Console.WriteLine(rg3);
            System.Console.WriteLine(rg4);
            System.Console.WriteLine(rg5);
            System.Console.WriteLine(rg6);
        } // End Sub Test 


    } // End Class SimpleBinaryTreeExample 


} // End Namespace 
