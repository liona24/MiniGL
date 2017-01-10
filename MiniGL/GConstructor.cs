namespace MiniGL
{
    public static class GConstructor2D
    {
        public static GObject2D[] GetRectangle(Rect rect, double w, TMaker2D tmaker)
        {
            return GetRectangle(rect.L, rect.T, rect.R, rect.B, w, tmaker);
        }
        public static GObject2D[] GetRectangle(Rect rect, TMaker2D tmaker)
        {
            return GetRectangle(rect.L, rect.T, rect.R, rect.B, 1.0, tmaker);
        }
        public static GObject2D[] GetRectangle(double l, double t, double r, double b, double w, TMaker2D tmaker)
        {
            return new GObject2D[] { new GObject2D (new Vec3(l, t, w), new Vec3(r, t, w), new Vec3(l, b, w), tmaker),
                                        new GObject2D (new Vec3(r, t, w), new Vec3(r, b, w), new Vec3(l, b, w), tmaker) };
        }
    }
}
