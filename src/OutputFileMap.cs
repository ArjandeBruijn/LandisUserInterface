using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    public class OutputFileMap 
    {
        public string FileName { get; private set; }
        public int Year { get; private set; }
        public string DirectoryName { get; private set; }
        public string Extension { get; private set; }
        string FileNameWithoutExtension;
        public string GetBatchRename(string addition)
        {
            string BatchRename = DirectoryName + "\\" + System.IO.Path.GetFileNameWithoutExtension(FileName) +"_" + addition + System.IO.Path.GetExtension(FileName);
            return BatchRename;
        }
        int get_Year(string FileName)
        {
            int year = -1;
            if (int.TryParse(System.Text.RegularExpressions.Regex.Match(FileName, @"\d+").Value, out year))
            {
                return year;
            }
            return year;
        }
        public OutputFileMap(string FileName) 
        {
            try
            {
                this.FileName = FileName;
                FileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(FileName);

                DirectoryName = System.IO.Path.GetDirectoryName(FileName);

                Year = get_Year(FileName);
                 
               
            }
            catch
            {
                return;
            }
          
        }
         
       
    }
}
