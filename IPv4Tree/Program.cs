
using System.Windows.Forms;

using MB.Algodat;


namespace IPv4Tree
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

            GeoLiteSqlServerImporter.ImportBlocks();

            uint ipv4 = IPv4Helper.IP2num("88.84.21.77");
            LookupIP(ipv4);
        }


        static void LookupIP(uint ipv4)
        {
            System.Console.WriteLine("Example 2");

            RangeTree<uint, IPv4Item> tree = new RangeTree<uint, IPv4Item>(new IPv4ItemComparer());
            
            // Range<uint> rangeToSearch = new Range<uint>(50, 60);

            System.Collections.Generic.List<IPv4Item> ls = GeoLiteImporter.DirectImport();
            tree.Add(ls);
            // tree.Add(new IPv4Item(lower, upper));
            

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 100; i++)
            {
                System.Collections.Generic.List<IPv4Item> results = tree.Query(ipv4);
                // PrintQueryResult("test", results);
                // System.Console.WriteLine("query: {0} results (tree count: {1})", results.Count, tree.Count);
                break;
            } // Next i 

            stopwatch.Stop();
            System.Console.WriteLine("elapsed time: {0}", stopwatch.Elapsed);
            System.Console.ReadKey();
        }


        static void PrintQueryResult(string queryTitle, System.Collections.Generic.IEnumerable<IPv4Item> result)
        {
            System.Console.WriteLine(queryTitle);
            foreach (IPv4Item item in result)
            {
                System.Console.Write("Query result \"");
                System.Console.Write(item.Text); // SELECT * FROM geoip.geoip_locations_temp WHERE geoname_id = item.Text
                System.Console.Write("\" for ");
                System.Console.WriteLine(item);
            }
               
        }


    }
}
