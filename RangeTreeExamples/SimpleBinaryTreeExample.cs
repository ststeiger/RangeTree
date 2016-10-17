using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RangeTreeExamples
{

    // http://stackoverflow.com/questions/11868837/fastest-way-to-search-a-number-in-a-list-of-ranges
    public class RangeGroup
    {
        public uint RangeGroupId { get; set; }
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
        private static readonly List<RangeGroup> m_RangeGroups;
        private static readonly RangeGroup[] RangeGroups;

        static RangeGroupFinder()
        {
            m_RangeGroups = new List<RangeGroup>();
            // Populating the list items here
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1, High = 5 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 1, Low = 6, High = 10 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 2, Low = 11, High = 15 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 3, Low = 16, High = 30 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 4, Low = 31, High = 100 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 5, Low = 101, High = 1000 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 6, Low = 1001, High = 5000 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 7, Low = 5001, High = 100000 });
            m_RangeGroups.Add(new RangeGroup { RangeGroupId = 8, Low = 100001, High = 1000000 });
            // There are many more and the groups are not sequential as it can seen on last 2 groups

            RangeGroups = m_RangeGroups.ToArray();
        }


        public static RangeGroup Find(uint number)
        {
            int position = RangeGroups.Length / 2;
            int stepSize = position / 2;

            /*
            1-10
            11-20
            21-30
            31-40
            41-50

            5 ==> position = 2.5 = 2 ==> stepsize = 1
            */

            int iMaxRange = RangeGroups.Length - 1;

            while (true)
            {   
                if (RangeGroups[position].High < number)
                {
                    // Search up
                    position += stepSize;

                }
                else if (RangeGroups[position].Low > number)
                {
                    // Search down
                    position -= stepSize;

                }
                else if (RangeGroups[position].Low <= number && RangeGroups[position].High >= number)
                {
                    // Found it!
                    RangeGroup match = RangeGroups[position].Clone();
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

        public static void Test()
        {
            RangeGroup rg1 = RangeGroupFinder.Find(123);
            RangeGroup rg2 = RangeGroupFinder.Find(1000000);
            RangeGroup rg3 = RangeGroupFinder.Find(1000);
            RangeGroup rg4 = RangeGroupFinder.Find(0);
            RangeGroup rg5 = RangeGroupFinder.Find(1);
            RangeGroup rg6 = RangeGroupFinder.Find(1000001);
            
            System.Console.WriteLine(rg1);
            System.Console.WriteLine(rg2);
            System.Console.WriteLine(rg3);
            System.Console.WriteLine(rg4);
            System.Console.WriteLine(rg5);
            System.Console.WriteLine(rg6);
        } // End Sub Test 


    } // End Class SimpleBinaryTreeExample 


} // End Namespace 
