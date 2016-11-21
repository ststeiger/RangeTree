
namespace IPv4Tree
{


    public class GeoLiteSqlServerImporter
    {


        private static string MapProjectPath(string path)
        {
            string dir = System.IO.Path.GetDirectoryName(typeof(GeoLiteSqlServerImporter).Assembly.Location);
            dir = System.IO.Path.Combine(dir, "../..");
            dir = System.IO.Path.Combine(dir, path);
            dir = System.IO.Path.GetFullPath(dir);

            return dir;
        } // End Function MapProjectPath 


        private static void InsertSQL(System.Text.StringBuilder sb)
        {
            InsertSQL(sb.ToString());
        }

        private static void InsertSQL(string strSQL)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();

            csb.DataSource = System.Environment.MachineName;
            csb.InitialCatalog = "Blogz";

            csb.IntegratedSecurity = true;
            csb.PersistSecurityInfo = false;
            csb.MultipleActiveResultSets = true;
            csb.PacketSize = 4096;


            if (!csb.IntegratedSecurity)
            {
                csb.UserID = "";
                csb.Password = "";
            } // End if (!csb.IntegratedSecurity) 

            using (System.Data.Common.DbConnection con = new System.Data.SqlClient.SqlConnection(csb.ConnectionString))
            {
                using (System.Data.Common.DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = strSQL;

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    cmd.ExecuteNonQuery();

                    if (con.State != System.Data.ConnectionState.Closed)
                        con.Close();

                } // End Using cmd 

            } // End Using con 

        } // End Sub InsertSQL 


        private static void ReadLineByLine(string fileName)
        {
            int counter = 1;
            string line;

            int commandCounter = 0;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Read the file and display it line by line.
            using (System.IO.FileStream fs = System.IO.File.OpenRead(fileName))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                {

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            counter++;
                            continue;
                        } // End if (string.IsNullOrEmpty(line)) 


                        sb.AppendLine(line);
                        commandCounter++;

                        if (commandCounter == 100)
                        {
                            InsertSQL(sb);
                            commandCounter = 0;
                            sb.Length = 0;
                        } // End if (commandCounter == 100) 


                        System.Console.WriteLine("Reading line {0}", counter);
                        // System.Console.WriteLine(line);
                        counter++;
                    } // Whend 

                } // End Using sr 

                fs.Close();
            } // End Using fs 

            if (sb.Length != 0)
                InsertSQL(sb);
        } // End Sub ReadLineByLine 


        public static void ImportBlocks()
        {
            InsertSQL("DELETE FROM geoip.geoip_blocks_temp;");
            string sqlFile = MapProjectPath(@"GeoLite/GeoIP_03_blocks_temp_INSERT.sql");
            ReadLineByLine(sqlFile);
        }


        public static void ImportLocations()
        {
            InsertSQL("DELETE FROM geoip.geoip_locations_temp;");
            string sqlFile = MapProjectPath(@"GeoLite/GeoIP_02_locations_temp_INSERT.sql");
            ReadLineByLine(sqlFile);
        }


    } // End Class GeoLiteImporter


} // End Namespace Captcha
