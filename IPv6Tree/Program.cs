
using System.Windows.Forms;

using MB.Algodat;


namespace IPv6Tree
{


    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
#if false
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif 

            uint ipv4 = IPv4Helper.IP2num("88.84.21.77");
            UInt128 ipv6 = new UInt128(0, ipv4);

            // UInt128 ipv6 = IPv6Helper.IP2num("2607:f0d0:1002:0051:0000:0000:0000:0004");
            LookupIP(ipv6);
        }


        static void LookupIP(UInt128 ipv6)
        {
            System.Console.WriteLine("Example 2");

            RangeTree<UInt128, UInt128RangeItem> tree = new RangeTree<UInt128, UInt128RangeItem>(new UInt128ItemComparer());
            
            // Range<uint> rangeToSearch = new Range<uint>(50, 60);

            System.Collections.Generic.List<UInt128RangeItem> ls = GeoLiteImporter.DirectImport();
            tree.Add(ls);
            // tree.Add(new IPv4Item(lower, upper));
            

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 100; i++)
            {
                System.Collections.Generic.List<UInt128RangeItem> results = tree.Query(ipv6);
                PrintQueryResult("test", results);
                System.Console.WriteLine("query: {0} results (tree count: {1})", results.Count, tree.Count);
                break;
            } // Next i 

            stopwatch.Stop();
            System.Console.WriteLine("elapsed time: {0}", stopwatch.Elapsed);
            System.Console.ReadKey();
        }


        static void PrintQueryResult(string queryTitle, System.Collections.Generic.IEnumerable<UInt128RangeItem> result)
        {
            System.Console.WriteLine(queryTitle);
            foreach (UInt128RangeItem item in result)
            {
                System.Console.Write("Query result \"");
                System.Console.Write(item.Text); // SELECT * FROM geoip.geoip_locations_temp WHERE geoname_id = item.Text
                System.Console.Write("\" for ");
                System.Console.WriteLine(item);
            }
               
        }


    }
}
