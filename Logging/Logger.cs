using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class Logging
    {
        public static string filename = "UploadDataLog_" + DateTime.Today.DayOfWeek.ToString() + ".txt";
        public static void Logger(String lines)
        {

            // Write the string to a file.append mode is enabled so that the log
            // lines get appended to  test.txt than wiping content and writing the log

            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\CSV\Out" + filename, true);
            file.WriteLine(lines);

            file.Close();

        }
    }
}
