using System.IO;
using UnityEngine;

namespace JRPG.Core
{
    // Mengelola proses baca dan tulis file save game ke disk lokal.
    public static class SaveManager
    {
        private static string SavePath => Application.persistentDataPath + "/savefile.json";

        public static void SaveGame()
        {
            if (PersistentPlayerData.Instance == null) return;

            GameSaveData data = new GameSaveData
            {
                Level = PersistentPlayerData.Instance.Level,
                CurrentExp = PersistentPlayerData.Instance.CurrentExp,
                Gold = PersistentPlayerData.Instance.Gold,
                PlayerPosX = PersistentPlayerData.Instance.LastMapPosition.x,
                PlayerPosY = PersistentPlayerData.Instance.LastMapPosition.y,
                PlayerPosZ = PersistentPlayerData.Instance.LastMapPosition.z
            };

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Game saved successfully to: {SavePath}");
        }

        public static bool LoadGame()
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("No save file found at path.");
                return false;
            }

            string json = File.ReadAllText(SavePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            if (PersistentPlayerData.Instance != null)
            {
                PersistentPlayerData.Instance.Level = data.Level;
                PersistentPlayerData.Instance.CurrentExp = data.CurrentExp;
                PersistentPlayerData.Instance.Gold = data.Gold;
                PersistentPlayerData.Instance.LastMapPosition = new Vector3(data.PlayerPosX, data.PlayerPosY, data.PlayerPosZ);
                PersistentPlayerData.Instance.HasSavedPosition = true;
            }

            Debug.Log("Game loaded successfully from disk.");
            return true;
        }
    }
}