namespace Assets.Scripts.Repository.Data
{
    public class RoomLayoutData
    {
        /// <summary>
        /// Only used in JSON file to differentiate the objects
        /// </summary>
        public string Comment;

        public int Width;
        public int Height;

        public RoomLayoutTileData[] RoomLayout;
        public RoomType RoomType;
    }
}