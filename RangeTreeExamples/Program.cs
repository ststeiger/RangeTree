using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MB.Algodat;
using System.Diagnostics;


// https://github.com/abh/geodns
// https://www.aliexpress.com/item/8GB-RAM-64GB-SSD-750G-HDD-Quad-Core-Laptop-Computer-Notebook-14-Inch-1600-900-Screen/32704458269.html?spm=2114.30010208.3.1.2xQuCI&s=p&ws_ab_test=searchweb0_0,searchweb201602_3_10065_10056_10068_10055_10054_10069_10059_10078_10079_10073_10017_10080_10070_10082_10081_421_420_10060_10061_10052_10062_10053_10050_10051,searchweb201603_3&btsid=8f6699ef-1023-41dd-999b-322557695cb5
// https://www.aliexpress.com/item/Hot-sale-X-26Y-C1037U-2G-RAM-16G-SSD-computer-case-linux-mini-pc-home-server/1407552118.html?spm=2114.30010208.3.1.pDOCn2&ws_ab_test=searchweb0_0,searchweb201602_3_10065_10056_10068_10055_10054_10069_10059_10078_10079_10073_10017_10080_10070_10082_10081_421_420_10060_10061_10052_10062_10053_10050_10051,searchweb201603_3&btsid=7938fefd-efcf-4234-95c1-a6cd296c3696


// „Sag den Menschen nie, wie sie Dinge tun sollen. Sag ihnen, was zu tun ist, 
// und sie werden dich mit ihrem Einfallsreichtum überraschen.“ 
// – George S. Patton



// https://en.wikipedia.org/wiki/Interval_tree
// http://www.thekevindolan.com/2010/02/interval-tree/index.html
// http://intervaltree.codeplex.com/
// https://github.com/ststeiger/RangeTree



// http://stackoverflow.com/questions/1193477/fast-algorithm-to-quickly-find-the-range-a-number-belongs-to-in-a-set-of-ranges
// https://en.wikipedia.org/wiki/AA_tree
// https://msdn.microsoft.com/en-us/library/ms379573(v=vs.80).aspx
// http://demakov.com/snippets/aatree.html




// http://stackoverflow.com/questions/8773469/c-sharp-interval-tree-class

namespace RangeTreeExamples
{

    // http://stackoverflow.com/questions/11868837/fastest-way-to-search-a-number-in-a-list-of-ranges
    public class RangeGroup
    {
        public uint RangeGroupId { get; set; }
        public uint Low { get; set; }
        public uint High { get; set; }
        // More properties related with the range here
    }



    public class RangeGroupFinder
    {
        private static readonly List<RangeGroup> RangeGroups = new List<RangeGroup>();

        static RangeGroupFinder()
        {
            // Populating the list items here
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023238144, High = 1023246335 });
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023246336, High = 1023279103 });
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023279104, High = 1023311871 });
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023311872, High = 1023328255 });
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023328256, High = 1023344639 });
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023344640, High = 1023410175 });
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023410176, High = 1023672319 });
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023672320, High = 1023688703 });
            RangeGroups.Add(new RangeGroup { RangeGroupId = 0, Low = 1023692800, High = 1023696895 });
            // There are many more and the groups are not sequential as it can seen on last 2 groups
        }

        public static RangeGroup Find(uint number)
        {
            return RangeGroups.FirstOrDefault(rg => number >= rg.Low && number <= rg.High);
        }
    }

    class Program
    {
        public static RangeGroup Find(uint number)
        {
            //RangeGroup[] RangeGroups = null;
            List<RangeGroup> RangeGroups = null;

            int position = RangeGroups.Count / 2;
            int stepSize = position / 2;

            while (true)
            {
                if (stepSize == 0)
                {
                    // Couldn't find it.
                    return null;
                }

                if (RangeGroups[position].High < number)
                {
                    // Search down.
                    position -= stepSize;

                }
                else if (RangeGroups[position].Low > number)
                {
                    // Search up.
                    position += stepSize;

                }
                else
                {
                    // Found it!
                    return RangeGroups[position];
                }

                stepSize /= 2;
            }
        }


        static void Main(string[] args)
        {
            TreeExample1();
            TreeExample2();
            TreeExample3();
        }

        static void TreeExample1()
        {
            Console.WriteLine("Example 1");



            var tree3 = new RangeTree<System.Numerics.BigInteger, HugeRangeItem>(new HugeRangeItemComparer());

            tree3.Add(new HugeRangeItem(0, 10, "1"));
            tree3.Add(new HugeRangeItem(20, 30, "2"));
            tree3.Add(new HugeRangeItem(15, 17, "3"));
            tree3.Add(new HugeRangeItem(25, 35, "4"));



            PrintQueryResult("query 1 (5):", tree3.Query( new System.Numerics.BigInteger(5) ) );
            PrintQueryResult("query 2 (10):", tree3.Query(new System.Numerics.BigInteger(10)));
            PrintQueryResult("query 3 (29):", tree3.Query(new System.Numerics.BigInteger(29)));
            PrintQueryResult("query 4 (5-15):", tree3.Query(new Range<System.Numerics.BigInteger>(5, 15)));




            var tree2 = new RangeTree<long, MyRangeItem>(new MyRangeItemComparer());

            tree2.Add(new MyRangeItem(0, 10, "1"));
            tree2.Add(new MyRangeItem(20, 30, "2"));
            tree2.Add(new MyRangeItem(15, 17, "3"));
            tree2.Add(new MyRangeItem(25, 35, "4"));


            PrintQueryResult("query 1", tree2.Query(5));
            PrintQueryResult("query 2", tree2.Query(10));
            PrintQueryResult("query 3", tree2.Query(29));
            PrintQueryResult("query 4", tree2.Query(new Range<long>(5, 15)));



            var tree = new RangeTree<int, RangeItem>(new RangeItemComparer());

            tree.Add(new RangeItem(0, 10, "1"));
            tree.Add(new RangeItem(20, 30, "2"));
            tree.Add(new RangeItem(15, 17, "3"));
            tree.Add(new RangeItem(25, 35, "4"));

            PrintQueryResult("query 1", tree.Query(5));
            PrintQueryResult("query 2", tree.Query(10));
            PrintQueryResult("query 3", tree.Query(29));
            PrintQueryResult("query 4", tree.Query(new Range<int>(5, 15)));

            Console.WriteLine();
        }

        static void TreeExample2()
        {
            Console.WriteLine("Example 2");

            var tree = new RangeTree<int, RangeItem>(new RangeItemComparer());
            var range = new Range<int>(50, 60);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                    RandomTreeInsert(tree, 1000);

                var resultCount = tree.Query(range).Count();
                Console.WriteLine("query: {0} results (tree count: {1})", resultCount, tree.Count);
            }

            stopwatch.Stop();
            Console.WriteLine("elapsed time: {0}", stopwatch.Elapsed);
        }

        static void TreeExample3()
        {
            Console.WriteLine("Example 3");

            var tree = new RangeTreeAsync<int, RangeItem>(new RangeItemComparer());
            var range = new Range<int>(50, 60);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                    RandomTreeInsert(tree, 1000);

                var resultCount = tree.Query(range).Count();
                Console.WriteLine("query: {0} results (tree count: {1})", resultCount, tree.Count);
            }

            stopwatch.Stop();
            Console.WriteLine("elapsed time: {0}", stopwatch.Elapsed);
        }

        static Random s_rnd = new Random();
        
        static void RandomTreeInsert(IRangeTree<int, RangeItem> tree, int limit)
        {
            var a = s_rnd.Next(limit);
            var b = s_rnd.Next(limit);

            tree.Add(new RangeItem(Math.Min(a, b), Math.Max(a, b)));
        }

        static void PrintQueryResult(string queryTitle, IEnumerable<RangeItem> result)
        {
            Console.WriteLine(queryTitle);
            foreach (var item in result)
                Console.WriteLine(item);
        }



        static void PrintQueryResult(string queryTitle, IEnumerable<MyRangeItem> result)
        {
            Console.WriteLine(queryTitle);
            foreach (var item in result)
                Console.WriteLine(item);
        }



        static void PrintQueryResult(string queryTitle, IEnumerable<HugeRangeItem> result)
        {
            Console.WriteLine(queryTitle);
            foreach (var item in result)
                Console.WriteLine(item);
        }

    }
}
