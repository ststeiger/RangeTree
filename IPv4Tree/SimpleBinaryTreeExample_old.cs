
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPv4Tree.old
{

    // http://stackoverflow.com/questions/11868837/fastest-way-to-search-a-number-in-a-list-of-ranges
    public class RangeGroup
    {
        public string RangeGroupId { get; set; }
        public uint Low { get; set; }
        public uint High { get; set; }
        // More properties related with the range here


        public uint HitValue { get; set; }
        public RangeGroup Clone()
        {
            var rg = new RangeGroup();
            rg.RangeGroupId = this.RangeGroupId;
            rg.Low = this.Low;
            rg.High = this.High;

            return rg;
        }

    }


    public class RangeGroupFinder
    {
        private readonly RangeGroup[] m_rangeGroups;


        public RangeGroupFinder(List<RangeGroup> rangeGroup)
        {
            
            // There are many more and the groups are not sequential as it can seen on last 2 groups
            this.m_rangeGroups = rangeGroup.ToArray();

            System.Array.Sort(this.m_rangeGroups, delegate(RangeGroup obj1, RangeGroup obj2)
                {
                    return obj1.Low.CompareTo(obj2.Low);
                }
            );


        }



        public RangeGroup Find(uint number)
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
                if (m_rangeGroups[position].High < number)
                {
                    // Search up
                    position += stepSize;

                }
                else if (m_rangeGroups[position].Low > number)
                {
                    // Search down
                    position -= stepSize;

                }
                else if (m_rangeGroups[position].Low <= number && m_rangeGroups[position].High >= number)
                {
                    // Found it!
                    RangeGroup match = m_rangeGroups[position].Clone();
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
            System.Collections.Generic.List<IPv4Item> ls = GeoLiteImporter.DirectImport();

            List<RangeGroup> lsRangeGroups = new List<RangeGroup>();
            // Populating the list items here
            foreach (IPv4Item x in ls)
            {
                lsRangeGroups.Add(new RangeGroup { RangeGroupId = x.Text, Low = x.Range.From, High = x.Range.To });
            }

            RangeGroupFinder finder = new RangeGroupFinder(lsRangeGroups);

            
            //uint ip = IPv4Helper.IP2num("46.14.227.129");
            //uint ip = IPv4Helper.IP2num("107.129.245.112");
            //uint ip = IPv4Helper.IP2num("244.1.168.2");
            uint ip = IPv4Helper.IP2num("47.238.70.238");
            
            

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            RangeGroup rg1 = finder.Find(ip);
            sw.Stop();

            System.Console.WriteLine(sw.Elapsed);
            System.Console.WriteLine(rg1);

        }

        private static List<RangeGroup> GenerateRangeGroup()
        {
            List<RangeGroup> m_RangeGroups = new List<RangeGroup>();
            // Populating the list items here
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "0", Low = 1, High = 5 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "1", Low = 6, High = 10 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "2", Low = 11, High = 15 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "3", Low = 16, High = 30 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "4", Low = 31, High = 100 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "5", Low = 101, High = 1000 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "6", Low = 1001, High = 5000 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "7", Low = 5001, High = 100000 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = "8", Low = 100001, High = 1000000 });

            return m_RangeGroups;
        }


        public static void Test()
        {
            List<RangeGroup> ls = GenerateRangeGroup();
            RangeGroupFinder finder = new RangeGroupFinder(ls);

            RangeGroup rg1 = finder.Find(123);
            RangeGroup rg2 = finder.Find(1000000);
            RangeGroup rg3 = finder.Find(1000);
            RangeGroup rg4 = finder.Find(0);
            RangeGroup rg5 = finder.Find(1);
            RangeGroup rg6 = finder.Find(1000001);
            
            System.Console.WriteLine(rg1);
            System.Console.WriteLine(rg2);
            System.Console.WriteLine(rg3);
            System.Console.WriteLine(rg4);
            System.Console.WriteLine(rg5);
            System.Console.WriteLine(rg6);
        } // End Sub Test 


    } // End Class SimpleBinaryTreeExample 


} // End Namespace 
