using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    static class Directory 
    {
        public static string[] GetFiles(string dir)
        {
            List<string> FileNames = new List<string>();
            if (System.IO.Directory.Exists(dir))
            {
                foreach (string File in System.IO.Directory.GetFiles(dir))
                {
                    FileNames.Add(File);
                }
                foreach (string subdir in System.IO.Directory.GetDirectories(dir))
                {
                    foreach (string File in GetFiles(subdir))
                    {
                        FileNames.Add(File);
                    }
                }
            }
            return FileNames.ToArray();
        }

        public static void DeleteDirectory(string dir)
        {
            try
            {
                if (System.IO.Directory.Exists(dir))
                {
                    foreach (string File in System.IO.Directory.GetFiles(dir))
                    {
                        System.IO.File.Delete(File);
                    }
                    foreach (string subdir in System.IO.Directory.GetDirectories(dir))
                    {
                        DeleteDirectory(subdir);
                    }
                }
                if (System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.Delete(dir);
                }
            }
            catch
            { 
            }

        }
        public static string[] GetAllFiles(string path, string ThatContain = null)
        {
            List<string> files = new List<string>();
            foreach (string folder in System.IO.Directory.GetDirectories(path))
            {
                foreach (string file in GetAllFiles(folder, ThatContain))
                {
                    if (ThatContain == null || file.Contains(ThatContain)) files.Add(file);
                }
            }

            foreach (string file in System.IO.Directory.GetFiles(path))
            {

                if (ThatContain == null || file.Contains(ThatContain)) files.Add(file);

            }
            return files.ToArray();
        }
    }
}
