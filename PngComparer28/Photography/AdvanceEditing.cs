using MB28.PngComparer.Drawing;

namespace MB28.PngComparer.Photography;

/// <summary>
/// TODO: Add summary
/// </summary>
public static class AdvanceEditing
{
    [Funky]
    /// <summary>
    /// TODO: Add summary
    /// </summary>
    /// <param name="pixel32s_2d"> TODO: Add summary </param>
    /// <param name="threshold"> TODO: Add summary </param>
    public static void Glare(this Pixel32[,] pixel32s_2d, int threshold = 225, int amount = 16)
        => pixel32s_2d.Bloom(out var o, threshold, amount);

    [Funky]
    public static void Bloom(this Pixel32[,] pixel32s_2d, out Pixel32[,] bleemLayer, int threshold = 225, int amount = 16)
    {
        Pixel32[,] bloomLayer = new Pixel32[pixel32s_2d.W(), pixel32s_2d.H()];
        Pixel32 t = new(threshold);

        pixel32s_2d.ForEach((x, y) =>
        {
            if (pixel32s_2d[x, y] < t)
                bloomLayer[x, y] = Pixel32.Black;
            else
                bloomLayer[x, y] = pixel32s_2d[x, y];
            return pixel32s_2d[x, y];
        });

        bloomLayer.ForEach((x, y) => (bloomLayer[x, y] != Pixel32.Black ? bloomLayer[x, y] += new Pixel32(30) : bloomLayer[x, y]).Clamp());
        bloomLayer.GaussianBlur(amount);

        bleemLayer = bloomLayer;

        pixel32s_2d.ForEach((x, y) => pixel32s_2d[x, y].Screen(bloomLayer[x, y]).Clamp());
    }
    
}