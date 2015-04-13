using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    public static class LogFile
    {
        static System.IO.StreamWriter sw;
        static string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFile.txt");

        public static void Reset()
        {
            try
            {
                System.IO.File.Delete(path);
            }
            catch { }
        }

        public static void WriteLine(string line)
        {
            sw = new System.IO.StreamWriter(path, true);
            sw.WriteLine(line);
            sw.Close();
        }
    }
}
