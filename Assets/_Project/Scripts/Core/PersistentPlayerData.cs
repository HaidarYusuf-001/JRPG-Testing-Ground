using UnityEngine;

namespace JRPG.Core
{
    // Menyimpan data pemain secara permanen selama game berjalan melintasi berbagai scene.
    public class PersistentPlayerData : MonoBehaviour
    {
        public static PersistentPlayerData Instance { get; private set; }

        public Vector3 LastMapPosition;
        public bool HasSavedPosition = false;

        public int Level = 1;
        public int CurrentExp = 0;
        public int Gold = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}