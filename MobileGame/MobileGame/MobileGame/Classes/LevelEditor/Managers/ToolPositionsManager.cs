using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

using GUI_System.GUIObjects;


namespace MobileGame.LevelEditor
{
    static class ToolPositionsManager
    {
        private static Dictionary<string, Vector2> ToolPositions = new Dictionary<string, Vector2>();
        private static string SavePath = @"C:\Users\" + Environment.UserName + @"\MobileGame\EditorSettings.xml";
        private static int notFoundValues = 0;

        public static void AddPosition(string Key, Vector2 Position)
        {
            if (ToolPositions.ContainsKey(Key))
            {
                ToolPositions[Key] = Position;
                Console.WriteLine("Updated Position, with Key: " + Key + ", and Value: " + Position);
            }

            else
            {
                ToolPositions.Add(Key, Position);
                Console.WriteLine("Added new Position, with Key: " + Key + ", and Value: " + Position);
            }
        }

        public static void RemovePosition(string Key)
        {
            if (ToolPositions.ContainsKey(Key))
                ToolPositions.Remove(Key);

            Console.WriteLine("Removed Position, with Key: " + Key);
        }

        public static Vector2 GetPosition(string Key)
        {
            if (ToolPositions.ContainsKey(Key))
                return ToolPositions[Key];

            Vector2 newPos = new Vector2(50 * notFoundValues, 0);
            notFoundValues++;

            return newPos;
        }

        public static void SaveData()
        {
            EditorData tempEditorData = new EditorData();
            tempEditorData.KeyList = new List<string>();
            tempEditorData.PositionList = new List<Vector2>();

            foreach (KeyValuePair<string, Vector2> entry in ToolPositions)
            {
                tempEditorData.KeyList.Add(entry.Key);
                tempEditorData.PositionList.Add(entry.Value);
            }


            FileStream SaveStream = File.Open(SavePath, FileMode.Create);

            try
            {
                XmlSerializer SaveSerializer = new XmlSerializer(typeof(EditorData));
                SaveSerializer.Serialize(SaveStream, tempEditorData);
            }
            finally
            {
                SaveStream.Close();
            }

            Console.WriteLine("Saved Data to File");
        }

        public static void LoadData()
        {
            EditorData tempEditorData = new EditorData();
            tempEditorData.KeyList = new List<string>();
            tempEditorData.PositionList = new List<Vector2>();

            FileStream LoadStream = File.Open(SavePath, FileMode.Open);

            try
            {
                XmlSerializer LoadSerializer = new XmlSerializer(typeof(EditorData));
                tempEditorData = (EditorData)LoadSerializer.Deserialize(LoadStream);
            }
            finally
            {
                LoadStream.Close();
            }

            ToolPositions = tempEditorData.KeyList.ToDictionary(x => x, x => tempEditorData.PositionList[tempEditorData.KeyList.IndexOf(x)]);
        }
    }

    public struct EditorData
    {
        public List<string> KeyList;
        public List<Vector2> PositionList;
    }
}
