using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Utils
{
    public class ConfigurationManager
    {
        private static string RELATIVE_RESOURCE_PATH = "Resources\\configurations\\";
        private static string FILE_EXTENSION = ".ini";
        private static char SEPARATOR_CHAR = ',';

        private static string PLAYER_SECTION = "PLAYER";
        private static string CAMERA_SECTION = "CAMERA";
        private static string GENERAL_SECTION = "GENERAL";
        private static string INPUT_SECTION = "INPUT";

        private static ConfigurationManager _instance;
        public static ConfigurationManager Instacne
        {
            get
            {
                if (_instance == null)
                    _instance = new ConfigurationManager();

                return _instance;
            }
        }

        private ConfigurationManager() { }

        public void ReadConfigurationFile(string fileName)
        {
            string[] fileContent = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + RELATIVE_RESOURCE_PATH + fileName + FILE_EXTENSION);
            Dictionary<string, List<string>> fileSections = new Dictionary<string, List<string>>();
            List<string> section = null;

            foreach (string line in fileContent)
            {
                if (IsSectionStart(line))
                {
                    string name = GetSectionName(line);
                    section = new List<string>();
                    fileSections.Add(name, section);
                }
                else if (!string.IsNullOrEmpty(line))
                {
                    section.Add(line);
                }
            }

            if (fileSections.ContainsKey(PLAYER_SECTION))
                ParsePlayerSection(fileSections[PLAYER_SECTION]);

            if (fileSections.ContainsKey(CAMERA_SECTION))
                ParseCameraSection(fileSections[CAMERA_SECTION]);

            if (fileSections.ContainsKey(GENERAL_SECTION))
                ParseGeneralSection(fileSections[GENERAL_SECTION]);

            if (fileSections.ContainsKey(INPUT_SECTION))
                ParseInputSection(fileSections[INPUT_SECTION]);
        }

        private bool IsSectionStart(string line)
        {
            return (line.Contains("[") && line.Contains("]"));
        }

        private string GetSectionName(string line)
        {
            string[] splitted = line.Split('[');
            string name = splitted[1].Split(']')[0];

            if (name.Contains("("))
                name = name.Split('(')[0];

            return name.Trim();
        }

        private void ParsePlayerSection(List<string> listToParse)
        {

        }

        private void ParseCameraSection(List<string> listToParse)
        {

        }

        private void ParseGeneralSection(List<string> listToParse)
        {

        }

        private void ParseInputSection(List<string> listToParse)
        {

        }

    }
}
