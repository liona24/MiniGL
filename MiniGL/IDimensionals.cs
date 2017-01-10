namespace MiniGL
{
    public interface I2Dimensional
    {
        double X { get; }
        double Y { get; }

        double GetX();
        double GetY();
    }

    public interface I3Dimensional : I2Dimensional
    {
        double Z { get; }
        double GetZ();
    }

    public interface I4Dimensional : I3Dimensional
    {
        double W { get; }
        double GetW();
    }
}
