using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Galo
{
    public static class DataSaver
    {
        ///<summary>Returns true if the save file exists, false if it doesn't </summary>
        public static bool CheckFirstTimeData()
        {
            string destination = Application.persistentDataPath + "/galoislandsgame.dat";
            // Debug.Log(destination);
            return File.Exists(destination);
        }

        public static bool CheckResetSuccess()
        {
            return Directory.GetFiles(Application.persistentDataPath).Length == 0;
        }

        public static void ClearPlayerData()
        {
            foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
            {
                DirectoryInfo data_dir = new DirectoryInfo(directory);
                data_dir.Delete(true);
            }

            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
            {
                FileInfo file_info = new FileInfo(file);
                file_info.Delete();
            }

        }


        public static void SaveFile()
        {
            string destination = Application.persistentDataPath + "/galoislandsgame.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);


            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, DataManager.instance.allPlayers);
            file.Close();
        }

        public static AllGaloPlayers LoadFile()
        {
            string destination = Application.persistentDataPath + "/galoislandsgame.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                Debug.LogError("File not found");
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            AllGaloPlayers data = (AllGaloPlayers)bf.Deserialize(file);
            file.Close();
            return data;
        }


    }
}