using RayCast.Core.Primitives;
using RayCast.Models;
using RayCast.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Utils
{
    public class ResourceManager
    {
        private static string RELATIVE_RESOURCE_PATH = "Resources\\data\\";
        private static string FILE_EXTENSION = ".res";
        private static char SEPARATOR_CHAR = ',';

        private static string WORLD_MAP_SECTION = "WORLD_MAP";
        private static string TEXTUTES_SECTION = "TEXTURES";
        private static string SPRITES_SECTION = "SPRITES";
        private static string ANIMATED_SPRITES_SECTION = "ANIMATED_SPRITES";


        public int[,] WorldMap { get; set; }
        public Textures Textures { get; set; }
        public Prop[] Props { get; set; }
        public Dictionary<int, int[]> AnimatedSprites{ get; set; }

        private static ResourceManager _instance;
        public static ResourceManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ResourceManager();

                return _instance;
            }
        }

        private ResourceManager() { }

        public void ReadResourceFile(string fileName)
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

            if (fileSections.ContainsKey(WORLD_MAP_SECTION))
                ParseWorldMap(fileSections[WORLD_MAP_SECTION]);

            if (fileSections.ContainsKey(TEXTUTES_SECTION))
                ParseTextures(fileSections[TEXTUTES_SECTION]);

            if (fileSections.ContainsKey(SPRITES_SECTION))
                ParseSprites(fileSections[SPRITES_SECTION]);

            if (fileSections.ContainsKey(ANIMATED_SPRITES_SECTION))
                ParseAnimatedSprites(fileSections[ANIMATED_SPRITES_SECTION]);

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

        private void ParseWorldMap(List<string> listToParse)
        {
            int y = listToParse.Count;
            int x = listToParse.First().Split(SEPARATOR_CHAR).Length;
            WorldMap = new int[y, x];

            int i = 0;
            int j = 0;

            foreach (string row in listToParse)
            {
                j = 0;
                string[] rowSplitted = row.Split(SEPARATOR_CHAR);
                if (rowSplitted.Length > x)
                    throw new Exception("WorldMap is badly formatted: " + row);

                foreach (string col in rowSplitted)
                {
                    WorldMap[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
        }

        private void ParseTextures(List<string> listToParse)
        {
            Textures = new Textures(64, 64);
            foreach (string line in listToParse)
            {
                string[] splitted = line.Split(SEPARATOR_CHAR);

                if (splitted.Length == 2)
                {
                    string name = splitted[0].Trim();
                    int index = int.Parse(splitted[1].Trim());
                    Textures.Add(name, index);
                }
                else if (splitted.Length == 4)
                {
                    string name = splitted[0].Trim();
                    int index = int.Parse(splitted[1].Trim());
                    int width = int.Parse(splitted[2].Trim());
                    int height = int.Parse(splitted[3].Trim());
                    Textures.Add(name, index, width, height);
                }
                else
                {
                    throw new Exception("A Texture is badly formatted: " + line);
                }
            }
        }

        private void ParseSprites(List<string> listToParse)
        {
            int spriteCount = listToParse.Count;
            Props = new Prop[spriteCount];

            int i = 0;
            foreach (string line in listToParse)
            {
                string[] splitted = line.Split(SEPARATOR_CHAR);

                if (splitted.Length > 3)
                    throw new Exception("A Sprrite id badly formatted: " + line);

                double x = double.Parse(splitted[0].Trim());
                double y = double.Parse(splitted[1].Trim());
                int textureIndex = int.Parse(splitted[2].Trim());

                Prop propToAdd = new Prop();
                propToAdd.Position = new Position(x, y, 0, 0);
                propToAdd.Sprite = new Sprite(textureIndex);
                Props[i] = propToAdd;
                i++;
            }

        }

        private void ParseAnimatedSprites(List<string> listToParse)
        {
            AnimatedSprites = new Dictionary<int, int[]>();
            foreach (string line in listToParse)
            {
                string[] splitted = line.Split(SEPARATOR_CHAR);

                if (splitted.Length <= 1)
                    throw new Exception("A Animated sprite is badly formatted: " + line);

                int spriteIndex = int.Parse(splitted[0].Trim());
                int[] frames = new int[splitted.Length - 1];

                splitted = splitted.Skip(1).ToArray();
                for (int i = 0; i < splitted.Length; i++)
                    frames[i] = int.Parse(splitted[i].Trim());

                AnimatedSprites.Add(spriteIndex, frames);
            }
        }

    }
}
