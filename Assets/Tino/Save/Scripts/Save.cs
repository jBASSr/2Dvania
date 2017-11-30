using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Tino.Save
{
    public static class SaveLoadGame
    {
        public static Tino.Save.Game SavedGame;
        private static string Path = Application.persistentDataPath + "/gamesave.dat";

        public static void SaveGame()
        {
            if(SavedGame == null) { SavedGame = new Tino.Save.Game(); }

            GetGameState();

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Create(Path))
            {
                bf.Serialize(file, SavedGame);
            }
        }

        public static void LoadGame()
        {
            if (!File.Exists(Path)) { return; }

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Open(Path, FileMode.Open))
            {
                SavedGame = (Tino.Save.Game)bf.Deserialize(file);
            }
        }

        private static void GetGameState()
        {
            SavedGame.CurrentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GameObject player = GameObject.FindWithTag("Player");
            if(player != null)
            {
                UnityEngine.Vector3 pos = player.transform.position;
                SavedGame.PlayerPosition = new Tino.Save.Vector3(pos.x, pos.y, pos.z);
            }
        }
    }
}