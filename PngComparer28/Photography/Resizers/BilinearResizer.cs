
using MB28.PngComparer.Drawing;

namespace MB28.PngComparer.Photography;


/// <summary> Methods for resizing Pixel32 arrays </summary>
public static partial class Resizers
{
    private static int BilinearInterpolation(int RGB00, int RGB01, int RGB10, int RGB11, float dx, float dy)
        => (int)(((1 - dx) * (1 - dy) * RGB00) + (dx * (1 - dy) * RGB01) + ((1 - dx) * dy * RGB10) + (dx * dy * RGB11));

    [Obsolete("\nUse other overload with ref keyword for up to 12% faster resizing.\n"
    + "This overload:   Min: 77.4499ms | Max: 98.7546ms | Average: 81.4ms\n"
    + "Other overload:    Min: 69.007ms | Max: 86.1496ms | Average: 74.06ms")]
    /// <summary> Resizes image using Bilinear interpolation. </summary>
    /// <remarks> Note that this method doesn't have side effects and only returns changes. use other overload </remarks>
    /// <returns> New Pixel32 2d array that resized to <paramref name="newSize"/> </returns>
    public static Pixel32[,] Bilinear(this Pixel32[,] pixs, sizeP newSize)
    {
        sizeP ogSize = new(pixs.W(), pixs.H());
        sizePf scaleFactor = new(newSize.W / (float)ogSize.W, newSize.H / (float)ogSize.H);

        Pixel32[,] result = new Pixel32[newSize.W, newSize.H];

        result.ForEach((i, j) =>
        {
            float x = j / scaleFactor.H;
            float y = i / scaleFactor.W;

            int x0 = (int)x;
            int y0 = (int)y;
            int x1 = Math.Min(x0 + 1, ogSize.H - 1);
            int y1 = Math.Min(y0 + 1, ogSize.W - 1);

            float dx = x - x0;
            float dy = y - y0;

            return new Pixel32(
                r: BilinearInterpolation(pixs[y0, x0].R, pixs[y0, x1].R, pixs[y1, x0].R, pixs[y1, x1].R, dx, dy),
                g: BilinearInterpolation(pixs[y0, x0].G, pixs[y0, x1].G, pixs[y1, x0].G, pixs[y1, x1].G, dx, dy),
                b: BilinearInterpolation(pixs[y0, x0].B, pixs[y0, x1].B, pixs[y1, x0].B, pixs[y1, x1].B, dx, dy),
                a: 255
            );
        });

        return result;
    }

    /// <summary> Resizes image using Bilinear interpolation, in place without creating new array. </summary>
    /// <returns> New Pixel32 2d array that resized to <paramref name="newSize"/> </returns>
    public static void Bilinear(ref Pixel32[,] pixs, sizeP newSize)
    {
        sizeP ogSize = new(pixs.W(), pixs.H());
        sizePf scaleFactor = new(newSize.W / (float)ogSize.W, newSize.H / (float)ogSize.H);

        Pixel32[,] result = new Pixel32[newSize.W, newSize.H];

        for (int j = 0; j < result.H(); j++)
            for (int i = 0; i < result.W(); i++)
            {
                float x = j / scaleFactor.H;
                float y = i / scaleFactor.W;

                int x0 = (int)x;
                int y0 = (int)y;
                int x1 = Math.Min(x0 + 1, ogSize.H - 1);
                int y1 = Math.Min(y0 + 1, ogSize.W - 1);

                float dx = x - x0;
                float dy = y - y0;

                result[i, j] = new Pixel32(
                    r: BilinearInterpolation(pixs[y0, x0].R, pixs[y0, x1].R, pixs[y1, x0].R, pixs[y1, x1].R, dx, dy),
                    g: BilinearInterpolation(pixs[y0, x0].G, pixs[y0, x1].G, pixs[y1, x0].G, pixs[y1, x1].G, dx, dy),
                    b: BilinearInterpolation(pixs[y0, x0].B, pixs[y0, x1].B, pixs[y1, x0].B, pixs[y1, x1].B, dx, dy),
                    a: 255
                );
            }

        pixs = result;
    }
    
}