
using MB28.PngComparer.Image;
using MB28.PngComparer.Drawing;

namespace MB28.PngComparer;

/// <summary> Extensions for <see cref="Pixel32"/> </summary>
public static class PngExtensions
{

    public static Pixel32[] GetPixels(this Png png)
    {
        Pixel32[,] Pixel32s = new Pixel32[png.Width, png.Height];

        for (int x = 0; x < png.Width; x++)
            for (int y = 0; y < png.Height; y++)
                Pixel32s[x, y] = png.GetPixel(x, y);

        return Pixel32s.BackTo1dArray();
    }  

}