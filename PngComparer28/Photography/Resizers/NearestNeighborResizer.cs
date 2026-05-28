
using MB28.PngComparer.Image;
using MB28.PngComparer.Drawing;
using MB28.PngComparer.More;

namespace MB28.PngComparer.Photography;

public static partial class Resizers
{
    [Obsolete("This resize algorithm is not recommended. also uses lot of memory. use Bilinear instead")]
    [Funky]
    /// <summary>
    /// Resizes image using Nearest Neighbor Algorithm.
    /// </summary>
    /// <param name="pixel32s_2d"> The Pixel32 array2d to resize</param>
    /// <param name="repeats">
    /// How many times Upscales image by power of 2. negative value = downscale. cannot be 0 <br/> <br/>
    /// Examples: 
    /// <br/> .... <br/>  -3 = x8 downscale <br/> -2 = x4 downscale <br/> -1 = x2 downscale 
    /// <br/> 1 = x2 upscale <br/> 2 = x4 upscale <br/> 3 = x8 upscale <br/>  ....
    /// </param>
    /// <remarks> Note that this method doesn't have side effects and only returns changes. </remarks>
    /// <returns> resized pixels2D </returns>
    /// <exception cref="ResizingException"> repeats cannot be 0 </exception>
    public static Png NearestNeighbor(Png png, int repeats = 1)
    {
        Pixel32[,] pixel32s_2d = png.GetPixels().To2dArray(png.SizeP);
        if (repeats == 0) throw new ResizingException("repeats cannot be 0");

        Pixel32[,] result = pixel32s_2d;

        if (repeats >= 1)
            for (int i = 0; i < repeats; i++)
                result = NearestNeighborUpscale(result);

        if (repeats <= -1)
            for (int i = 0; i < Math.Abs(repeats); i++)
                result = NearestNeighborDownscale(result);

        var builder = PngBuilder.Create(result.GetLength(0), result.GetLength(1), png.HasAlphaChannel);
        for (int y = 0; y < result.GetLength(1); y++)
            for (int x = 0; x < result.GetLength(0); x++)
                builder.SetPixel(result[x, y], x, y);

        return Png.Open(builder.Save());
    }



    [Obsolete("This resize algorithm is not recommended. also uses lot of memory")]
    internal static Pixel32[,] NearestNeighborUpscale(Pixel32[,] pixel32s_2d)
    {
        int w = pixel32s_2d.GetLength(0) * 2;
        int h = pixel32s_2d.GetLength(1) * 2;
        Pixel32[,] result = new Pixel32[w, h];

        for (int y = 0; y < h; y += 2)
            for (int x = 0; x < w; x += 2)
            {
                result[x, y] = pixel32s_2d[Math.Clamp(x / 2, 0, int.MaxValue), Math.Clamp(y / 2, 0, int.MaxValue)];
                result[x + 1, y] = result[x, y];
                result[x, y + 1] = result[x, y];
                result[x + 1, y + 1] = result[x, y];
            }

        return result;
    }

    [Obsolete("This resize algorithm is not recommended. also uses lot of memory")]
    internal static Pixel32[,] NearestNeighborDownscale(Pixel32[,] pixel32s_2d)
    {
        int w = pixel32s_2d.GetLength(0);
        int h = pixel32s_2d.GetLength(1);
        Pixel32[,] result = new Pixel32[w / 2, h / 2];
        w = result.GetLength(0);
        h = result.GetLength(1);

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                result[x, y] = pixel32s_2d[x * 2, y * 2].Average([
                    pixel32s_2d[x * 2, y * 2 + 1],
                    pixel32s_2d[x * 2 + 1, y * 2],
                    pixel32s_2d[x * 2 + 1, y * 2 + 1],
                ]);

        return result;
    }

}