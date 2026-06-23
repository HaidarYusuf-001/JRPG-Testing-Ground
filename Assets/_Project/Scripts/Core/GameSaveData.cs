using System;

namespace JRPG.Core
{
    // Struktur data murni untuk keperluan serialisasi JSON ke media penyimpanan.
    [Serializable]
    public class GameSaveData
    {
        public int Level;
        public int CurrentExp;
        public int Gold;
        public float PlayerPosX;
        public float PlayerPosY;
        public float PlayerPosZ;
    }
}