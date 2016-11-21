
using MB.Algodat;

namespace IPv4Tree
{


    class GeoLiteImporter
    {


        public static string MapProjectPath(string path)
        {
            string dir = System.IO.Path.GetDirectoryName(typeof(GeoLiteImporter).Assembly.Location);
            dir = System.IO.Path.Combine(dir, "../..");
            dir = System.IO.Path.Combine(dir, path);
            dir = System.IO.Path.GetFullPath(dir);

            return dir;
        } // End Function MapProjectPath 


        public static void TestBlocks()
        {
            string fileIPv4 = MapProjectPath(@"GeoLite/GeoLite2-Country-Blocks-IPv4.csv");
            string fileIPv6 = MapProjectPath(@"GeoLite/GeoLite2-Country-Blocks-IPv6.csv");


            DirectImport(fileIPv4);

        }

        public class IpBlock
        {
            public string network;
            public string geoname_id;  // Country where the IP is used
            public string registered_country_geoname_id; // Country the IP is registered in 
            public string represented_country_geoname_id;// Country that IP represents, e.g. US military base in Rammstein, Germany
            public string is_anonymous_proxy;
            public string is_satellite_provider;


            public bool is_IPv4
            {
                get
                {
                    return (this.network.IndexOf('.') != -1);
                }
            }
            public bool is_IPv6
            {
                get
                {
                    return (this.network.IndexOf(':') != -1);
                }
            }


            public UInt128 LowerBound
            {
                get
                {
                    if(this.is_IPv4)
                    {
                        string[] bounds = IPv4Helper.GetRange(this.network);
                        return new UInt128(0, IPv4Helper.IP2num(bounds[0]));
                    }
                    else if (this.is_IPv6)
                    {
                        string[] bounds = IPv6Helper.GetRange(this.network);
                        return IPv6Helper.IP2num(bounds[0]);
                    }

                    throw new System.ArgumentException("Invalid CIDR format.");
                }
            }


            public UInt128 UpperBound
            {
                get
                {
                    if (this.is_IPv4)
                    {
                        string[] bounds = IPv4Helper.GetRange(this.network);
                        return new UInt128(0, IPv4Helper.IP2num(bounds[1]));
                    }
                    else if (this.is_IPv6)
                    {
                        string[] bounds = IPv6Helper.GetRange(this.network);
                        return IPv6Helper.IP2num(bounds[1]);
                    }

                    throw new System.ArgumentException("Invalid CIDR format.");
                }
            }


        } // End Class IpBlock 



        public static System.Collections.Generic.List<IPv4Item> DirectImport()
        {
            string fileIPv4 = MapProjectPath(@"GeoLite/GeoLite2-Country-Blocks-IPv4.csv");
            string fileIPv6 = MapProjectPath(@"GeoLite/GeoLite2-Country-Blocks-IPv6.csv");


            return DirectImport(fileIPv4);
        }



        public static System.Collections.Generic.List<IPv4Item> DirectImport(string fileName)
        {
            System.Collections.Generic.List<IPv4Item> ls = new System.Collections.Generic.List<IPv4Item>();
            string line;
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();


            System.Reflection.FieldInfo[] fis = typeof(IpBlock).GetFields();
            // System.Reflection.PropertyInfo[] pis = typeof(IpBlock).GetProperties();

            System.Collections.Generic.Dictionary<string, GeoLiteLocationImporter.GeoLiteLocation> dict = GeoLiteLocationImporter.ImportLocations();

            // Read the file and display it line by line.
            using (System.IO.FileStream fs = System.IO.File.OpenRead(fileName))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                {

                    for (int lineNumber = 1; (line = sr.ReadLine()) != null; ++lineNumber)
                    {
                        if ((lineNumber == 1) || (string.IsNullOrEmpty(line)))
                            continue;

                        string[] values = line.Split(',');

                        IpBlock thisBlock = new IpBlock();

                        for (int i = 0; i < fis.Length; ++i)
                        {
                            fis[i].SetValue(thisBlock, values[i]);
                        } // Next i 


                        GeoLiteLocationImporter.GeoLiteLocation loc = null;
                        if (dict.ContainsKey(thisBlock.registered_country_geoname_id))
                            loc = dict[thisBlock.registered_country_geoname_id];

                        ls.Add(
                            new IPv4Item((uint)thisBlock.LowerBound.Low, (uint)thisBlock.UpperBound.Low, loc)
                        );
                    } // Next counter 

                } // End Using sr 

            } // End Using fs 

            return ls;
        } // End Sub 


        public static void SimpleTest()
        {
            string fileIPv4 = MapProjectPath(@"GeoLite/GeoLite2-Country-Blocks-IPv4.csv");
            string fileIPv6 = MapProjectPath(@"GeoLite/GeoLite2-Country-Blocks-IPv6.csv");

            fileIPv4 = System.IO.File.ReadAllText(fileIPv4, System.Text.Encoding.UTF8);
            fileIPv6 = System.IO.File.ReadAllText(fileIPv6, System.Text.Encoding.UTF8);

            fileIPv4 = fileIPv4.Replace("\r\n", "\n").Replace("\r", "\n");
            fileIPv6 = fileIPv6.Replace("\r\n", "\n").Replace("\r", "\n");

            string[] recordsIPv4 = fileIPv4.Split('\n');
            string[] recordsIPv6 = fileIPv6.Split('\n');

            for (long i = 0; i < recordsIPv4.LongLength; ++i)
            {
                string line = recordsIPv4[i];
                string[] values = line.Split(',');
                System.Console.WriteLine(values);
                System.Console.WriteLine(line);
            } // Next i 

            System.Console.WriteLine(fileIPv4);
            System.Console.WriteLine(fileIPv6);
        } // End Sub Test 


    } // End Class GeoLiteImporter


} // End Namespace Captcha
