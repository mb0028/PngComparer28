
using MB28.PngComparer.Image;
using MB28.PngComparer.Drawing;

namespace MB28.PngComparer.Photography;

/// <summary>
/// TODO: Add summary
/// </summary>
public static class BlendingUtils
{
    public enum BlendingMode { Darken, Lighten, Multiple, Screen, Overlay, ColorBurn }

    [Funky]
    /// <summary> TODO: Add summary </summary>
    public static Png Blend(string imagePath, string image2Path, BlendingMode blendMode)
        => Blend(Png.Open(imagePath), Png.Open(image2Path), blendMode);

    [Funky]
    /// <summary> TODO: Add summary </summary>
    public static Png Blend(Png png, Png png2, BlendingMode blendMode)
    {
        var pxs = png.GetPixels();

#pragma warning disable CS0618 // Type or member is obsolete

        var pxs2 = png2.GetPixels().To2dArray(png2.SizeP).Bilinear(png.SizeP).BackTo1dArray();
        
#pragma warning restore CS0618 // Type or member is obsolete

        switch (blendMode)
        {
            case BlendingMode.Darken: pxs.ForEach(i => pxs[i].Darken(pxs2[i]).Clamp()); break;
            case BlendingMode.Lighten: pxs.ForEach(i => pxs[i].Lighten(pxs2[i]).Clamp()); break;
            case BlendingMode.Multiple: pxs.ForEach(i => pxs[i].Multiple(pxs2[i]).Clamp()); break;
            case BlendingMode.Screen: pxs.ForEach(i => pxs[i].Screen(pxs2[i]).Clamp()); break;
            case BlendingMode.Overlay: pxs.ForEach(i => pxs[i].Overlay(pxs2[i]).Clamp()); break;
            case BlendingMode.ColorBurn: pxs.ForEach(i => pxs[i].ColorBurn(pxs2[i]).Clamp()); break;
        }

        return Png.Open(PngBuilder.FromPixel32_2d(pxs.To2dArray(png.SizeP), png.HasAlphaChannel).Save());
    }





    internal static Pixel32 Overlay(this Pixel32 pixel, Pixel32 other)
    {
        return new(O(pixel.R, other.R), O(pixel.G, other.G), O(pixel.B, other.B), pixel.A);
        static int O(int RGB, int RGB2) => (int)(RGB > 127.5f ? (1f - ((1f - (RGB / 255.0f)) * (1f - (RGB2 / 255.0f)))) * 255f
            : (RGB * RGB2) / 255);
    }

    internal static Pixel32 Darken(this Pixel32 pixel, Pixel32 other)
    {
        return new(O(pixel.R, other.R), O(pixel.G, other.G), O(pixel.B, other.B), pixel.A);
        static int O(int RGB, int RGB2) => Math.Min(RGB, RGB2);
    }

    internal static Pixel32 Lighten(this Pixel32 pixel, Pixel32 other)
    {
        return new(O(pixel.R, other.R), O(pixel.G, other.G), O(pixel.B, other.B), pixel.A);
        static int O(int RGB, int RGB2) => Math.Max(RGB, RGB2);
    }

    internal static Pixel32 Multiple(this Pixel32 pixel, Pixel32 other)
    {
        return new(O(pixel.R, other.R), O(pixel.G, other.G), O(pixel.B, other.B), pixel.A);
        static int O(int RGB, int RGB2) => (RGB * RGB2) / 255;
    }

    internal static Pixel32 Screen(this Pixel32 pixel, Pixel32 other)
    {
        return new(O(pixel.R, other.R), O(pixel.G, other.G), O(pixel.B, other.B), pixel.A);
        static int O(int RGB, int RGB2) => (int)((1f - ((1f - (RGB / 255.0f)) * (1f - (RGB2 / 255.0f)))) * 255f);
    }

    internal static Pixel32 ColorBurn(this Pixel32 pixel, Pixel32 other) =>
        255 - (255 - pixel) / other;
    
    

}