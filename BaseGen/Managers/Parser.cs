#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;
#endregion

namespace BaseGen.Managers
{
    public static class Parser
    {
        public static void ParseMusicFiles(StreamReader reader)
        {

            using (reader)
            {

            }
        }
        public static void ParseNewDirectory()
        {
            string[] files;
            System.IO.StreamReader reader;
            System.IO.StreamWriter writer;
            try
            {
                files = Directory.GetFiles(Main.levelPath);
            }
            catch
            {
                Directory.CreateDirectory(Main.levelPath);
                files = Directory.GetFiles(Main.levelPath);
            }
            int index = 0;
            Managers.Executive.levels = new List<List<char[,]>>();
            Managers.Executive.signData = new List<List<string>>();
            foreach (string fileName in files)
            {
                if (fileName.Contains("Level" + index.ToString()))
                    using (reader = new System.IO.StreamReader(fileName))
                        Managers.Executive.levels.Add(Managers.Parser.ParseLevel(reader));
                else if (fileName.Contains("Signs"))
                    using (reader = new System.IO.StreamReader(fileName))
                        Managers.Executive.signData.Add(Managers.Parser.ParseSignData(reader));
                index++;
            }
            using (writer = new System.IO.StreamWriter(Main.contentPath + "Save.txt", false))
            {
                writer.WriteLine(Main.levelPath);
            }
        }
        public static string LoadLastDirectory(StreamReader file)
        {
            return file.ReadLine();
        }
        public static Menus.Menu ParseMenuData(StreamReader file)
        {
            List<Menus.Buttons.Button> buttons = new List<Menus.Buttons.Button>();
            Menus.Menu menu;
            string m_name, line, b_name;
            Vector2 position;
            string[] split;
            int inIndex = 0, deIndex = 0, boIndex = 0;

            m_name = file.ReadLine().Split(':')[1];
            while ((line = file.ReadLine()) != null)
            {
                split = line.Split(':');
                b_name = split[0];
                split = split[1].Split(',');
                position = new Vector2(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]));
                switch (b_name)
                {
                    case "IncreaseButton":
                        buttons.Add(new Menus.Buttons.IncreaseButton(position, b_name, m_name, inIndex));
                        inIndex++;
                        break;
                    case "DecreaseButton":
                        buttons.Add(new Menus.Buttons.DecreaseButton(position, b_name, m_name, deIndex));
                        deIndex++;
                        break;
                    case "EditorButton":
                        buttons.Add(new Menus.Buttons.EditorButton(position, b_name));

                        break;
                    case "StartEditorButton":
                        buttons.Add(new Menus.Buttons.StartEditorButton(position, b_name));
                        break;
                    case "BackButton":
                        buttons.Add(new Menus.Buttons.BackButton(position, b_name));
                        break;
                    case "BoolButton":
                        buttons.Add(new Menus.Buttons.BoolButton(position, b_name, boIndex));
                        boIndex++;
                        break;
                    case "TextField":
                        buttons.Add(new Menus.Buttons.TextField(position, b_name));
                        break;
                    case "SmallButton":
                        buttons.Add(new Menus.Buttons.SmallButton(position, b_name));
                        break;
                    case "SmallRowButton":
                        buttons.Add(new Menus.Buttons.SmallButton(position, b_name));
                        break;
                    case "SmallColButton":
                        buttons.Add(new Menus.Buttons.SmallButton(position, b_name));
                        break;
                    case "SmallLevelButton":
                        buttons.Add(new Menus.Buttons.SmallButton(position, b_name));
                        break;
                    default:
                        buttons.Add(new Menus.Buttons.Button(position, b_name));
                        break;

                }
            }
            file.Close();
            switch (m_name)
            {
                case "EditorMenu":
                    menu = new Menus.EditorMenu(m_name, buttons);
                    break;
                default:
                    menu = new Menus.Menu(m_name, buttons);
                    break;
            }
            return menu;
        }
        public static Dictionary<string, string> ParseAsset(StreamReader file)
        {
            string line;
            Dictionary<string, string> assetDict = new Dictionary<string, string>();
            while ((line = file.ReadLine()) != null)
            {
                assetDict.Add(line.Split(':')[0], line.Split(':')[1]);
            }
            file.Close();
            return assetDict;
        }
        public static List<char[,]> ParseLevel(StreamReader file)
        {
            List<char[,]> level = new List<char[,]>();
            string line;

            int row = 0, col = 0, width, height;

            line = file.ReadLine();
            width = Convert.ToInt32(line.Split(',')[0]);
            height = Convert.ToInt32(line.Split(',')[1]);
            char[,] levelFront = new char[width, height];
            char[,] levelBack = new char[width, height];
            while (row < width)
            {
                line = file.ReadLine();
                col = 0;
                foreach (char c in line)
                {
                    levelFront[row, col] = c;
                    col++;
                }
                row++;
            }
            row = 0;
            col = 0;
            line = string.Empty;

            while(row < width)
            {
                line = file.ReadLine();
                col = 0;
                foreach (char c in line)
                {
                    levelBack[row, col] = c;
                    col++;
                }
                row++;
            }
            level.Add(levelFront);
            level.Add(levelBack);
            return level;
        }
        public static KeyValuePair<string, List<Point>[]> ParseAnimData(StreamReader file)
        {
            KeyValuePair<string, List<Point>[]> pair;
            List<Point>[] startend = new List<Point>[2];
            List<Point> starts = new List<Point>();
            List<Point> ends = new List<Point>();
            string line;
            string[] split;
            string name = file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                split = line.Split('_');
                starts.Add(new Point(Convert.ToInt32(split[0].Split(',')[0]), Convert.ToInt32(split[0].Split(',')[1])));
                ends.Add(new Point(Convert.ToInt32(split[1].Split(',')[0]), Convert.ToInt32(split[1].Split(',')[1])));
            }
            startend[0] = starts;
            startend[1] = ends;
            pair = new KeyValuePair<string, List<Point>[]>(name, startend);
            return pair;
        }
        public static List<string> ParseSignData(StreamReader file)
        {
            List<string> signData = new List<string>();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                signData.Add(line);
            }
            return signData;
        }

        public static Dictionary<char, string> ParseTileDictionary(StreamReader file)
        {
            Dictionary<char, string> tileDict = new Dictionary<char, string>();
            string line, value;
            string[] lines;
            char key;
            while ((line = file.ReadLine()) != null)
            {
                lines = line.Split(';');
                key = lines[0][0];
                value = lines[1];
                tileDict.Add(key, value);
            }
            return tileDict;
        }
    }
}
