namespace DataPersistance
{
    public class SaverVisitor : IPersistor<GameData>
    {
        public GameData GameData { get; set; }

        public void Accept(IPersistance<Player.Data> obj)
        {
            if (!GameData.playersData.TryAdd(obj.ID, obj.Data))
            {
                GameData.playersData[obj.ID] = obj.Data;
            }
        }
    }
}
