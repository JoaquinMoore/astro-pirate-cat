using System.Collections.Generic;

namespace DataPersistance
{
    [System.Serializable]
    public class GameData
    {
        public Dictionary<string, Player.Data> playersData = new();
    }
}
