
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



    class Program
    {

        static void Main(string[] args)
        {
            SimpleBinaryTreeExample.Test();
            TreeExample1();
            TreeExample2();
            TreeExample3();
        }



        // For an IPv4 address, the first element will be 0,
        /// and the second will be a UInt32 representation of the four bytes.
        /// For an IPv6 address, the first element will be a UInt64
        /// representation of the first eight bytes, and the second will be the
        /// last eight bytes.
        /// </summary>
        /// <param name="ipAddress">The IP address to convert.</param>
        /// <returns></returns>
        // private static MB.Algodat.UInt128 IpAddressToInt128(string ipAddress)
        private static MB.Algodat.UInt128 IpAddressToInt128(string ipAddress)
        {
            System.Net.IPAddress ipa = System.Net.IPAddress.Parse(ipAddress);
            byte[] addrBytes = ipa.GetAddressBytes();

            if (System.BitConverter.IsLittleEndian)
            {
                //little-endian machines store multi-byte integers with the
                //least significant byte first. this is a problem, as integer
                //values are sent over the network in big-endian mode. reversing
                //the order of the bytes is a quick way to get the BitConverter
                //methods to convert the byte arrays in big-endian mode.
                System.Array.Reverse(addrBytes);
            }

            ulong[] addrWords = new ulong[2];
            if (addrBytes.Length > 8)
            {
                addrWords[0] = System.BitConverter.ToUInt64(addrBytes, 8); // High
                addrWords[1] = System.BitConverter.ToUInt64(addrBytes, 0); // Low
            }
            else
            {
                addrWords[0] = 0; // High
                addrWords[1] = System.BitConverter.ToUInt32(addrBytes, 0); // Low
            }

            // return addrWords;
            return new MB.Algodat.UInt128(addrWords[0], addrWords[1]);
        }



        // For an IPv4 address, the first element will be 0,
        /// and the second will be a UInt32 representation of the four bytes.
        /// For an IPv6 address, the first element will be a UInt64
        /// representation of the first eight bytes, and the second will be the
        /// last eight bytes.
        /// </summary>
        /// <param name="ipAddress">The IP address to convert.</param>
        /// <returns></returns>
        //private static System.UInt32 IpV4AddressToUInt32(string ipAddress)
        private static System.UInt32 IpV4AddressToUInt32(string ipAddress)
        {
            var ipa = System.Net.IPAddress.Parse(ipAddress);
            byte[] addrBytes = ipa.GetAddressBytes();


            if (addrBytes.Length > 8)
            {
                throw new System.ArgumentException("Not an IPv4 address");
            }


            if (System.BitConverter.IsLittleEndian)
            {
                // little-endian machines store multi-byte integers with the
                // least significant byte first. this is a problem, as integer
                // values are sent over the network in big-endian mode. reversing
                // the order of the bytes is a quick way to get the BitConverter
                // methods to convert the byte arrays in big-endian mode.
                System.Array.Reverse(addrBytes);
            }

            return System.BitConverter.ToUInt32(addrBytes, 0); // High
        } // End Function IpV4AddressToUInt32 


        public static System.Net.IPAddress Int128ToIpAddress(MB.Algodat.UInt128 ipNumber)
        {
            byte[] addrBytes = null;

            if (ipNumber.High == 0 && ipNumber.Low <= System.UInt32.MaxValue) // IPv4
            {
                System.UInt32 ui32 = (System.UInt32)ipNumber.Low;
                addrBytes = System.BitConverter.GetBytes(ui32);
            }
            else // IPv6
            {
                byte[] highBytes = System.BitConverter.GetBytes(ipNumber.High);
                byte[] lowBytes = System.BitConverter.GetBytes(ipNumber.Low);
                addrBytes = new byte[highBytes.Length + lowBytes.Length];
                // System.Array.Copy(sourceArray, sourceIndex, destinationArray, destIndex, length);
                System.Array.Copy(lowBytes, 0, addrBytes, 0, 8);
                System.Array.Copy(highBytes, 0, addrBytes, 8, 8);
            }

            if (System.BitConverter.IsLittleEndian)
                System.Array.Reverse(addrBytes);
            
            System.Net.IPAddress address = new System.Net.IPAddress(addrBytes);
            return address;
        } // End Function Int128ToIpAddress 


        // https://github.com/abh/geodns
        // http://mkaczanowski.com/golang-build-dynamic-dns-service-go/
        // https://pcapdotnet.svn.codeplex.com/svn/PcapDotNet/src/PcapDotNet.Base/
        // http://stackoverflow.com/questions/2928327/converting-an-ip-address-to-a-number
        public void SqlSearch()
        {
            string SQL = @"SELECT * FROM T_Country_IP_Range 
WHERE 
(
    block_from_upper < @in_IP_upper 
    OR 
    (block_from_upper = @in_IP_upper AND block_from_lower <= @in_IP_lower)
) 
AND 
(
    block_to_upper > @in_IP_upper 
    OR 
    (block_to_upper = @in_IP_upper AND block_to_lower >= @in_IP_lower)
);
";
        }


        static void TreeExample1()
        {
            var number2 = IpAddressToInt128("127.0.0.1");
            number2 = IpAddressToInt128("2001:db8:a0b:12f0::1");


            System.Console.WriteLine(number2);
            string ss = number2.ToString();
            System.Console.WriteLine(ss);
            System.Net.IPAddress ipAddress = Int128ToIpAddress(number2);
            System.Console.WriteLine(ipAddress);


            System.Console.WriteLine("Example 1");

            var tree4 = new RangeTree<MB.Algodat.UInt128, UInt128RangeItem>(new UInt128ItemComparer());

            tree4.Add(new UInt128RangeItem(new MB.Algodat.UInt128(0), new MB.Algodat.UInt128(10), "1"));
            tree4.Add(new UInt128RangeItem(new MB.Algodat.UInt128(20), new MB.Algodat.UInt128(30), "2"));
            tree4.Add(new UInt128RangeItem(new MB.Algodat.UInt128(15), new MB.Algodat.UInt128(17), "3"));
            tree4.Add(new UInt128RangeItem(new MB.Algodat.UInt128(25), new MB.Algodat.UInt128(35), "4"));


            PrintQueryResult("query 1 (5):", tree4.Query(new MB.Algodat.UInt128(5)));
            PrintQueryResult("query 2 (10):", tree4.Query(new MB.Algodat.UInt128(10)));
            PrintQueryResult("query 3 (29):", tree4.Query(new MB.Algodat.UInt128(29)));
            PrintQueryResult("query 4 (5-15):", tree4.Query(new Range<MB.Algodat.UInt128>(new MB.Algodat.UInt128(5), new MB.Algodat.UInt128(15))));



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


        static void PrintQueryResult(string queryTitle, IEnumerable<UInt128RangeItem> result)
        {
            Console.WriteLine(queryTitle);
            foreach (var item in result)
                Console.WriteLine(item);
        }

    }
}
