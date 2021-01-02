using Assets.Scripts.Repository.Data;

namespace Assets.Scripts.World.Generation.Data
{
    // TILE TYPES: 0 - empty, 1 - floor, 2 - wall, 3 - other
    public class RoomData
    {
        public TileData[,] Tiles;

        public int Left;
        public int Top;
        public int Width;
        public int Height;

        public int Right => Left + Width - 1;
        public int Bottom => Top + Height - 1;

        public int CenterX => Left + Width / 2;
        public int CenterY => Top + Height / 2;

        public bool IsConnected;
        public RoomType Type;

        public RoomData(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new TileData[Width, Height];
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    Tiles[x, y] = new TileData();
        }

        public bool CollidesWith(RoomData room)
        {
            if (Left > room.Right + 1)
                return false;
            if (Top > room.Bottom + 1)
                return false;
            if (Right < room.Left - 1)
                return false;
            if (Bottom < room.Top - 1)
                return false;

            return true;
        }
    }
}
