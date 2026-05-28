using MB28.PngComparer.Drawing;

namespace MB28.PngComparer.Photography;

/// <summary>
/// Filters that can applied to an image. all methods are extension for Pixel32 array 1d and 2d
/// </summary>
/// <remarks>
///  All array changes are down in place without creating new array
/// </remarks> 
public static class Filters
{
    /// <summary> Applies Black and White Filter </summary>
	public static void BW(this Pixel32[] pixel32s, float threshold = 0.5f)
    {
        pixel32s.ForEach(i => {
            Pixel32 p = pixel32s[i];
            int c = (p.R * Lum.R + p.G * Lum.G + p.B * Lum.B) / 255f > threshold ? 255 : 0;
            return new(c, c, c, 255);
        });
    }

    /// <summary> Applies Grayscale Filter </summary>
    /// <remarks> mimic MATLAB rgb2gray https://www.mathworks.com/help/matlab/ref/rgb2gray.html
    /// note this uses a weird convention of 0.2989 for the coefficient of red instead
    /// of the coefficient 0.299</remarks>
	public static void Grayscale(this Pixel32[] pixel32s)
    {
        pixel32s.ForEach(i => {
            Pixel32 p = pixel32s[i];
            int gs = (int)(0.2989f * p.R + 0.5870f * p.G + 0.1140f * p.B);
            return new Pixel32(gs, gs, gs, p.A).Clamp();
        });
    }

    /// <summary> Invert Pixels. </summary>
    public static void InvertColors(this Pixel32[] pixel32s)
    {
        pixel32s.ForEach(i =>
        {
            Pixel32 p = pixel32s[i];
            return new Pixel32(255 - p.R, 255 - p.G, 255 - p.B, p.A); //.Clamp();
        });
    }

    /// <summary> TODO: Add summary </summary>
    public static void ColorOverlay(this Pixel32[] pixel32s, Pixel32 color)
        => pixel32s.ForEach(i => pixel32s[i].Overlay(color));

    /// <summary> TODO: Add summary </summary>
    public static void ColorOverlay(this Pixel32[] pixel32s, System.Drawing.Color color)
        => pixel32s.ColorOverlay(new Pixel32(color.R, color.G, color.B, color.A));
    
}