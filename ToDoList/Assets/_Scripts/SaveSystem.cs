using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Saving
{
    public static class SaveSystem
    {
        public static readonly string directoryName = "Saves";

        public static readonly string fileNameData = "playerDataSave.json";

        public static void Save(SaveObject saveObject)
        {
            Debug.Log("<color=white>Saving PlayerData</color>");

            string json = JsonUtility.ToJson(saveObject);

            File.WriteAllText(GetFilePath(fileNameData), json);

            //    BinaryFormatter formatter = new BinaryFormatter();

            //    FileStream stream = new FileStream(GetFilePath(fileNameData), FileMode.Create);

            //    formatter.Serialize(stream, json);
            //    stream.Close();
            //}
        }

        public static SaveObject Load()
        {
            if (VerifyFile(fileNameData))
            {
                //BinaryFormatter formatter = new BinaryFormatter();

                //FileStream stream = new FileStream(GetFilePath(fileNameData), FileMode.Open);

                //string saveString = formatter.Deserialize(stream) as string;

                //stream.Close();

                string saveString = File.ReadAllText(GetFilePath(fileNameData));

                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                Debug.Log("<color=white>PlayerData Loaded</color>");

                return saveObject;
            }
            else
            {
                return null;
            }
        }


        #region Operaciones


        private static string GetFilePath(string fileName)
        {
            VerifyDirectory();
            string filePath = GetDirectoryPath() + "/" + fileName;
            return filePath;
        }

        private static bool VerifyFile(string fileName)
        {
            string filePath = GetFilePath(fileName);
            Debug.Log(GetFilePath(fileName));

            if (!File.Exists(filePath))
            {
                Debug.Log("<color=orange> File Not Founded </color>" + fileName);
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void VerifyDirectory()
        {
            string dirPath = GetDirectoryPath();

            if (!Directory.Exists(dirPath))
            {
                Debug.Log("DirectoryNoExists, Creating");
                Directory.CreateDirectory(dirPath);
            }
        }

        private static string GetDirectoryPath()
        {
            return Application.persistentDataPath + "/" + directoryName;
        }

        #endregion
    }

}