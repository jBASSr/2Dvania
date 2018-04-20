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
        public static bool SaveExists { get; private set; }
        public static bool PlayerHealthLoaded { get; set; }
        public static bool PlayerMovementLoaded { get; set; }

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
            SaveLoadGame.SaveExists = false;
            if (!File.Exists(Path)) { return; }

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Open(Path, FileMode.Open))
            {
                SavedGame = (Tino.Save.Game)bf.Deserialize(file);
            }

            for(int i = 0; i < SavedGame.WorldItems.GetLength(0); i++)
            {
                WorldState.SceneItemState[SavedGame.WorldItems[i]] = SavedGame.WorldItemStates[i];
            }

            SaveLoadGame.SaveExists = true;
        }

        private static void GetGameState()
        {
            SavedGame.CurrentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GameObject player = GameObject.FindWithTag("Player");
            HealthSystem playerHealth = player.GetComponent<HealthSystem>();
            SimpleMovement playerMovement = player.GetComponent<SimpleMovement>();

            if(player == null) { return; }
            if(playerHealth == null) { return; }
            if(playerMovement == null) { return; }

            UnityEngine.Vector3 pos = player.transform.position;
            SavedGame.PlayerPosition = new Tino.Save.Vector3(pos.x, pos.y, pos.z);
            SavedGame.PlayerCurrentHealth = playerHealth.health;
            SavedGame.PlayerMaxHealth = playerHealth.maxHealth;
            SavedGame.PlayerExtraJumps = playerMovement.extraJumps;
			SavedGame.PlayerMissileAmmo = playerMovement.MissileAmmo;

            SavedGame.WorldItems = new SceneItemTuple[WorldState.SceneItemState.Count];
            SavedGame.WorldItemStates = new bool[WorldState.SceneItemState.Count];
            WorldState.SceneItemState.Keys.CopyTo(SavedGame.WorldItems, 0);
            WorldState.SceneItemState.Values.CopyTo(SavedGame.WorldItemStates, 0);
        }
    }
}