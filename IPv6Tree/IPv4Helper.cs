
namespace IPv6Tree
{



    // https://dev.maxmind.com/geoip/
    // https://github.com/maxmind/MaxMind-DB-Reader-dotnet
    // https://github.com/maxmind/GeoIP2-dotnet 
    // https://dev.maxmind.com/geoip/geoip2/geolite2/
    class IPv4Helper
    {


        public static void Test()
        {
            string IP = "5.39.40.96/27";
            // IP = "88.84.128.0/19";
            CIDR2IP(IP);

            // IPrange2CIDR("88.84.128.0", "88.84.159.255");
            string cidr = IPrange2CIDR("5.39.40.96", "5.39.40.127");
            cidr = IPrange2CIDR("192.168.1.1", "192.168.255.255");
            System.Console.WriteLine(cidr);
        } // End Sub Test 


        // https://www.digitalocean.com/community/tutorials/understanding-ip-addresses-subnets-and-cidr-notation-for-networking
        public static void CIDR2IP(string IP)
        {
            string[] parts = IP.Split('.', '/');

            uint ipnum = (System.Convert.ToUInt32(parts[0]) << 24) |
                (System.Convert.ToUInt32(parts[1]) << 16) |
                (System.Convert.ToUInt32(parts[2]) << 8) |
                System.Convert.ToUInt32(parts[3]);

            int maskbits = System.Convert.ToInt32(parts[4]);
            uint mask = 0xffffffff;
            mask <<= (32 - maskbits);

            uint ipstart = ipnum & mask;
            uint ipend = ipnum | (mask ^ 0xffffffff);

            string fromRange = string.Format("{0}.{1}.{2}.{3}", ipstart >> 24, (ipstart >> 16) & 0xff, (ipstart >> 8) & 0xff, ipstart & 0xff);
            string toRange = string.Format("{0}.{1}.{2}.{3}", ipend >> 24, (ipend >> 16) & 0xff, (ipend >> 8) & 0xff, ipend & 0xff);

            System.Console.WriteLine(fromRange + " - " + toRange);
        } // End Function CIDR2IP 


        public static string[] GetRange(string IP)
        {
            string[] parts = IP.Split('.', '/');

            uint ipnum = (System.Convert.ToUInt32(parts[0]) << 24) |
                (System.Convert.ToUInt32(parts[1]) << 16) |
                (System.Convert.ToUInt32(parts[2]) << 8) |
                System.Convert.ToUInt32(parts[3]);

            int maskbits = System.Convert.ToInt32(parts[4]);
            uint mask = 0xffffffff;
            mask <<= (32 - maskbits);

            uint ipstart = ipnum & mask;
            uint ipend = ipnum | (mask ^ 0xffffffff);

            string fromRange = string.Format("{0}.{1}.{2}.{3}", ipstart >> 24, (ipstart >> 16) & 0xff, (ipstart >> 8) & 0xff, ipstart & 0xff);
            string toRange = string.Format("{0}.{1}.{2}.{3}", ipend >> 24, (ipend >> 16) & 0xff, (ipend >> 8) & 0xff, ipend & 0xff);

            return new string[] { fromRange, toRange };
        } // End Function CIDR2IP 



        public static uint IP2num(string ip)
        {
            string[] nums = ip.Split('.');
            uint first = System.UInt32.Parse(nums[0]);
            uint second = System.UInt32.Parse(nums[1]);
            uint third = System.UInt32.Parse(nums[2]);
            uint fourth = System.UInt32.Parse(nums[3]);

            return (first << 24) | (second << 16) | (third << 8) | (fourth);
        } // End Function IP2num 


        // https://dev.maxmind.com/geoip/
        // https://stackoverflow.com/questions/461742/how-to-convert-an-ipv4-address-into-a-integer-in-c
        public static string IPrange2CIDR(string ip1, string ip2)
        {
            uint startAddr = IP2num(ip1);
            uint endAddr = IP2num(ip2);

            // uint startAddr = 0xc0a80001; // 192.168.0.1
            // uint endAddr = 0xc0a800fe;   // 192.168.0.254
            // uint startAddr = System.BitConverter.ToUInt32(System.Net.IPAddress.Parse(ip1).GetAddressBytes(), 0);
            // uint endAddr = System.BitConverter.ToUInt32(System.Net.IPAddress.Parse(ip2).GetAddressBytes(), 0);

            if (startAddr > endAddr)
            {
                uint temp = startAddr;
                startAddr = endAddr;
                endAddr = temp;
            } // End if (startAddr > endAddr) 

            // uint diff = endAddr - startAddr -1;
            // int bits =  32 - (int)System.Math.Ceiling(System.Math.Log10(diff) / System.Math.Log10(2));
            // return ip1 + "/" + bits;

            uint diffs = startAddr ^ endAddr;

            // Now count the number of consecutive zero bits starting at the most significant
            int bits = 32;
            // int mask = 0;

            // We keep shifting diffs right until it's zero (i.e. we've shifted all the non-zero bits off)
            while (diffs != 0)
            {
                diffs >>= 1;
                bits--; // Every time we shift, that's one fewer consecutive zero bits in the prefix
                // Accumulate a mask which will have zeros in the consecutive zeros of the prefix and ones elsewhere
                // mask = (mask << 1) | 1;
            } // Whend 

            return (ip1 + "/" + bits);
        } // End Function IPrange2CIDR


    } // End Class IPv4Helper 


} // End Namespace Captcha 
