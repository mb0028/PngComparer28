
namespace MB28.PngComparer.Drawing;


/// <summary> TODO: Add summary. This is an abstract class. </summary>
public abstract class Lum
{
    /// <summary> TODO: Add summary </summary>
    public const float R = 0.2126f;

    /// <summary> TODO: Add summary </summary>
    public const float G = 0.7152f;

    /// <summary> TODO: Add summary </summary>
    public const float B = 0.0722f;

    public static float Brightness(Pixel32 p) => p.R * R + p.G * G + p.B * B;

    /// <summary> TODO: Add summary </summary>
    public static byte R8 => (byte)MathP.Lerp(0, 255, R);
    /// <summary> TODO: Add summary </summary>
    public static byte G8 => (byte)MathP.Lerp(0, 255, G);
    /// <summary> TODO: Add summary </summary>
    public static byte B8 => (byte)MathP.Lerp(0, 255, B);
}
