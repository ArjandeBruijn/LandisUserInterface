using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    public static class LogFile
    {
        static string FileName = "LogFile.txt";

        public static void Reset()
        {
            if (System.IO.File.Exists(FileName))
            {
                System.IO.File.Delete(FileName);
            }
        }

        public static void Write(string message)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName, true);

            sw.WriteLine(message);

            sw.Close();
        }

    }
}
