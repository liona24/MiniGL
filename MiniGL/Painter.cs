namespace MiniGL
{
    public abstract class BasePainter<T>
    {
        protected ZBuffer<T> zBuffer;
        protected Rasterizer raster;

        public int ScreenWidth { get { return zBuffer.Width; } }
        public int ScreenHeight { get { return zBuffer.Height; } }
        public int OffsetX { get { return raster.OffsetX; } set { raster.OffsetX = value; } }
        public int OffsetY { get { return raster.OffsetY; } set { raster.OffsetY = value; } }


        public BasePainter(int screenWidth, int screenHeight, int offsetX, int offsetY)
        {
            zBuffer = new ZBuffer<T>(screenWidth, screenHeight);
            raster = new Rasterizer(screenWidth, screenHeight, offsetX, offsetY);
        }
        public BasePainter(int screenWidth, int screenHeight)
            : this(screenWidth, screenHeight, 0, 0)
        { }

        public abstract void Paint(T obj);
        public virtual void Clear(T background)
        {
            zBuffer.Clear(background);
        }
    }

    public class Painter2D : BasePainter<GObject2D>
    {

        public Painter2D(int screenWidth, int screenHeight)
            : base(screenWidth, screenHeight)
        { }
        public Painter2D(int screenWidth, int screenHeight, int offsetX, int offsetY)
            : base(screenWidth, screenHeight, offsetX, offsetY)
        { }

        public override void Paint(GObject2D obj)
        {
            raster.Rasterize(obj, zBuffer, obj.TransformToWindow());
        }
    }

    public class Painter : BasePainter<GObject>
    {
        public Painter(int screenWidth, int screenHeight, int offsetX, int offsetY)
            : base(screenWidth, screenHeight, offsetX, offsetY)
        { }
        public Painter(int screenWidth, int screenHeight)
            : base(screenWidth, screenHeight, 0, 0)
        { }


        public override void Paint(GObject obj)
        {
            raster.Rasterize(obj, zBuffer, obj.TransformToWindow());
        }
    }
}
