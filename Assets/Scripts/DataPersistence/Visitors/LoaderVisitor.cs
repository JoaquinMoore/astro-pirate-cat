using DataPersistence.Interfaces;

namespace DataPersistence.Visitors
{
    public class LoaderVisitor : IPersistor<GameData>
    {
        public GameData GameData { get; set; }

        public void Accept(IPersistence<Player.Data> obj)
        {
            obj.Data = GameData.playersData[obj.ID];
        }
    }
}