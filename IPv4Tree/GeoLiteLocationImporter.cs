
namespace IPv4Tree
{


    public class GeoLiteLocationImporter
    {



        public class GeoLiteLocation
        {
            public string geoname_id;
            public string locale_code; // Localization language
            public string continent_code;
            public string continent_name;
            public string country_iso_code; // ISO 3166-2,
            public string country_name;
        } // End Class Location 


        private static string MapProjectPath(string path)
        {
            string dir = System.IO.Path.GetDirectoryName(typeof(GeoLiteLocationImporter).Assembly.Location);
            dir = System.IO.Path.Combine(dir, "../..");
            dir = System.IO.Path.Combine(dir, path);
            dir = System.IO.Path.GetFullPath(dir);

            return dir;
        } // End Function MapProjectPath 



        public static System.Collections.Generic.Dictionary<string, GeoLiteLocation> ImportLocations(string fileName)
        {
            System.Collections.Generic.Dictionary<string, GeoLiteLocation> dict = new System.Collections.Generic.Dictionary<string, GeoLiteLocation>();
            string line;
            

            System.Reflection.FieldInfo[] fis = typeof(GeoLiteLocation).GetFields();
            // System.Reflection.PropertyInfo[] pis = typeof(GeoLiteLocation).GetProperties();


            // Read the file and display it line by line.
            using (System.IO.FileStream fs = System.IO.File.OpenRead(fileName))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                {
                    for (int counter = 1; (line = sr.ReadLine()) != null; ++counter)
                    {
                        if (string.IsNullOrEmpty(line) || counter == 1)
                            continue;


                        string[] values = line.Split(',');

                        GeoLiteLocation thisLocation = new GeoLiteLocation();

                        for (int i = 0; i < fis.Length; ++i)
                        {
                            fis[i].SetValue(thisLocation, values[i]);
                        } // Next i 

                        dict.Add(thisLocation.geoname_id, thisLocation);

                        // System.Console.WriteLine("Reading line {0}", counter);
                        // System.Console.WriteLine(line);
                        counter++;
                    } // Whend 

                } // End Using sr 

                fs.Close();
            } // End Using fs 

            return dict;
        } // End Function ImportLocations 


        public static System.Collections.Generic.Dictionary<string, GeoLiteLocation> ImportLocations()
        {
            string sqlFile = MapProjectPath(@"GeoLite/GeoLite2-Country-Locations-de.csv");
            return ImportLocations(sqlFile);
        } // End Function ImportLocations 


    } // End Class GeoLiteImporter


} // End Namespace Captcha
