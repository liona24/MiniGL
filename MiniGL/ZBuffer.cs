namespace MiniGL
{
    public class ZBuffer
    {
        readonly float[][] zs;
        readonly int[][] hashCodes;

        readonly int width, height;

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        ///<summary>
        ///Returns hashCode of the object placed at the coordinates (i, j)
        ///</summary>
        public int this[int i, int j] { get { return hashCodes[i][j]; } }

        public ZBuffer(int width, int height, int background)
        {
            zs = new float[width][];
            hashCodes = new int[width][];
            for (int i = 0; i < width; i++)
            {
                zs[i] = new float[height];
                hashCodes[i] = new int[height];
                for (int j = 0; j < height; j++)
                {
                    hashCodes[i][j] = background;
                    zs[i][j] = float.NegativeInfinity;
                }
            }
        }

        public void Clear(int background)
        {
            Clear(background, float.NegativeInfinity);
        }
        public void Clear(int background, float z)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    zs[i][j] = z;
                    hashCodes[i][j] = background;
                }
            }

        }

        public bool TryInsert(int x, int y, float z, int code)
        {
            if (zs[x][y] < z)
            {
                zs[x][y] = z;
                hashCodes[x][y] = code;
                return true;
            }
            return false;
        }
    }

}
