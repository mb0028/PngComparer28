
using MB28.PngComparer.Drawing;

namespace MB28.PngComparer.Photography;

/// <summary>
/// Adjustments that can applied to an image. all methods are extension for Pixel32[]
/// </summary>
/// <remarks>
///  All array changes are down in place without creating new array
/// </remarks> 
public static class Adjustments
{
    /// <summary> TODO: Add summary </summary>
    public static void Brightness(this Pixel32[] p, float addPerc)
    {
        p.ForEach(i => {
            Pixel32 pixel = p[i];
            return pixel.Clamp();
        });
    }

    /// <summary> TODO: Add summary </summary>
    public static void Contrast(this Pixel32[] p, float addPerc)
    {
        p.ForEach(i => {
            Pixel32 pixel = p[i];
            return pixel.Clamp();
        });
    }

    /// <summary> TODO: Add summary </summary>
    public static void Saturation(this Pixel32[] p, float addPerc)
    {
        p.ForEach(i => {
            Pixel32 pixel = p[i];
            return pixel.Clamp();
        });
    }

    /// <summary> TODO: Add summary </summary>
    public static void Vibrance(this Pixel32[] p, float addPerc)
    {
        p.ForEach(i => {
            Pixel32 pixel = p[i];
            return pixel.Clamp();
        });
    }

}