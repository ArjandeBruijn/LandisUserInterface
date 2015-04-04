using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    public class Global
    {
        public static List<string> ScenarioFileNames
        {
            get
            {

                List<string> Values = new List<string>(Properties.Settings.Default.LastScenarioFileNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                return new List<string>(Values.Distinct());
            }

        }
        public static void ClearScenarioFileNames()
        {
            Properties.Settings.Default.LastScenarioFileNames = "";
            Properties.Settings.Default.Save();
        }
        public static void AddScenario(string FileName)
        {
            if (Properties.Settings.Default.LastScenarioFileNames.Contains(FileName) == false)
            {
                Properties.Settings.Default.LastScenarioFileNames += FileName + ";";
                Properties.Settings.Default.Save();
            }

        }
        public static void RemoveScenario(string FileName)
        {
            if (Properties.Settings.Default.LastScenarioFileNames.Contains(FileName) == false)
            {
                throw new System.Exception("LastScenarioFileNames " + Properties.Settings.Default.LastScenarioFileNames + " does not contain filename " + FileName);
            }
            Properties.Settings.Default.LastScenarioFileNames.Replace(FileName + ";", "");
            Properties.Settings.Default.Save();
        }
        public static void ClearScenarios()
        {
            Properties.Settings.Default.LastScenarioFileNames = "";
            Properties.Settings.Default.Save();
        }

        public static string LandisConsoleExe
        {
            get
            {
                return Properties.Settings.Default.LandisConsoleExe;
            }
            set
            {
                Properties.Settings.Default.LandisConsoleExe = value;
                Properties.Settings.Default.Save();
            }
        }

    }
}
