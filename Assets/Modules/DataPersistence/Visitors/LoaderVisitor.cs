namespace DataPersistance
{
    public class LoaderVisitor : IPersistor<GameData>
    {
        public GameData GameData { get; set; }

        public void Accept(IPersistance<Player.Data> obj)
        {
            obj.Data = GameData.playersData[obj.ID];
        }
    }
}
