
using MB28.PngComparer.Drawing;

namespace MB28.PngComparer;

/// <summary> Extensions for <see cref="Pixel32"/> </summary>
public static class Pixel32Extensions
{
    /// <summary>
    /// Average this pixel with others and clamps the result.
    /// </summary>
    /// <remarks> Note: it doesn't check if others count is 0 or not. </remarks>
    public static Pixel32 Average(this Pixel32 pixel, Pixel32[] others)
    {
        Pixel32 r = pixel;
        foreach (Pixel32 o in others)
            r += o;
        return (r / (others.Length + 1)).Clamp();
    }

    /// <summary> Clamps Pixel32's RGBA between 0 and 255 </summary>
    public static Pixel32 Clamp(this Pixel32 p) =>
        new(Math.Clamp(p.R, 0, 255), Math.Clamp(p.G, 0, 255), Math.Clamp(p.B, 0, 255), Math.Clamp(p.A, 0, 255));

    /// <summary> Gets accent color by averaging center pixel with all other pixels </summary>
    public static Pixel32 GetAccentColor(this Pixel32[] pixels)
        => pixels[pixels.Length / 2].Average(pixels);

    /// <summary> TODO: Add summary </summary>
    public static int W(this Pixel32[,] pixel32s_2d) => pixel32s_2d.GetLength(0);
    /// <summary> TODO: Add summary </summary>
    public static int H(this Pixel32[,] pixel32s_2d) => pixel32s_2d.GetLength(1);

    public delegate Pixel32 ForEachPixel_I(int i);
    public delegate Pixel32 ForEachPixel_XY(int x, int y);
    public delegate int ForRgb(int currentChannel);

    /// <summary>
    /// Changes the pixel of all pixels, in place without creating new array.
    /// </summary>
    public static void ForEach(this Pixel32[] pixel32s, ForEachPixel_I forEachPixel)
    {
        for (int i = 0; i < pixel32s.Length; i++)
            pixel32s[i] = forEachPixel(i);
    }

    /// <summary>
    /// Changes the pixel of all pixels, in place without creating new array.
    /// </summary>
    public static void ForEach(this Pixel32[,] pixel32s_2d, ForEachPixel_XY forEachPixel2d)
    {
        for (int y = 0; y < pixel32s_2d.H(); y++)
            for (int x = 0; x < pixel32s_2d.W(); x++)
                pixel32s_2d[x, y] = forEachPixel2d(x, y);
    }


    /// <summary> Runs same function for each channels (R, G & B only) </summary>
    public static Pixel32 ForRGB(this Pixel32 p, ForRgb action)
    {
        return new Pixel32(p.R = action(p.R), p.G = action(p.G), p.B = action(p.B), p.A);
    }

    /// <summary> Turns Pixel32[] to 2d array </summary>
    public static Pixel32[,] To2dArray(this Pixel32[] pixels, sizeP imageSize)
    {
        Pixel32[,] result = new Pixel32[imageSize.Width, imageSize.Height];

        for (int y = 0; y < imageSize.Height; y++)
            for (int x = 0; x < imageSize.Width; x++)
                result[x, y] = pixels[x * imageSize.Height + y];

        return result;
    }

    /// <summary> Turns the 2d array that converted with Pixel32[].To2dArray() back to 1d </summary>
    public static Pixel32[] BackTo1dArray(this Pixel32[,] pixels2D)
    {
        int w = pixels2D.W();
        int h = pixels2D.H();
        Pixel32[] result = new Pixel32[w * h];

        int i = 0;
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                result[i] = pixels2D[x, y];
                i++;
            }

        return result;
    }

}