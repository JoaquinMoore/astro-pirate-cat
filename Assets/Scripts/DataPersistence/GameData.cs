using System;
using System.Collections.Generic;

namespace DataPersistence
{
    [Serializable]
    public class GameData
    {
        public Dictionary<string, Player.Data> playersData = new();
    }
}