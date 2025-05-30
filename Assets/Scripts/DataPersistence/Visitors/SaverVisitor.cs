using DataPersistence.Interfaces;

namespace DataPersistence.Visitors
{
    public class SaverVisitor : IPersistor<GameData>
    {
        public GameData GameData { get; set; }

        public void Accept(IPersistence<Player.Data> obj)
        {
            if (!GameData.playersData.TryAdd(obj.ID, obj.Data)) GameData.playersData[obj.ID] = obj.Data;
        }
    }
}