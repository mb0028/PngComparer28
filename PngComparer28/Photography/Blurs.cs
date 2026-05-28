using MB28.PngComparer.Drawing;

namespace MB28.PngComparer.Photography;

/// <summary>
/// Methods in this class are blurs an image. all methods are extension for Pixel32[,]
/// </summary>
public static class Blurs
{
    /// <summary> Make sure to clean it up after use. </summary>
    internal static volatile Pixel32[,] blurVolatile;

    /// <summary> Blurs an image with Gaussian sampling. </summary>
    public static void GaussianBlur(this Pixel32[,] pixel32s_2d, int repeat = 8)
    {
        int w = pixel32s_2d.W();
        int h = pixel32s_2d.H();
        blurVolatile = pixel32s_2d;

        for (int i = 0; i < repeat; i++)
        {
            Parallel.For(0, h, ImageComparer.parallelOptions, y =>
            {
                Parallel.For(0, w, ImageComparer.parallelOptions, x =>
                {
                    Pixel32 nH = pixel32s_2d[Math.Clamp(x + 1, 0, w - 1), y];
                    Pixel32 nV = pixel32s_2d[x, Math.Clamp(y + 1, 0, h - 1)];
                    Pixel32 pH = pixel32s_2d[Math.Clamp(x - 1, 0, w - 1), y];
                    Pixel32 pV = pixel32s_2d[x, Math.Clamp(y - 1, 0, h - 1)];

                    blurVolatile[x, y] = Pixel32Extensions.Average(pixel32s_2d[x, y], [nH, nV, pH, pV]);
                });
            });
            pixel32s_2d = blurVolatile;
        }
        blurVolatile = null;
    }

    [Funky]
    /// <summary> Blurs an image with Kawase-based blur algorithm. Keeps downscale and upscale image until it blurred out </summary>
    /// <remarks> Note this method doesn't have side effects and only returns changes. </remarks>
    public static Pixel32[,] KawaseBlur(this Pixel32[,] pixel32s_2d, int repeat = 4, int downscaleFactor = 4, int startdownscaleFactor = 2)
    {
        var result = pixel32s_2d;
        sizeP ogSize = new(pixel32s_2d.W(), pixel32s_2d.H());
        sizeP initDownscale = new(ogSize.H / startdownscaleFactor, ogSize.W / startdownscaleFactor);
        sizeP downscaleTarget = new(initDownscale.H / downscaleFactor, initDownscale.W / downscaleFactor);

        Resizers.Bilinear(ref result, initDownscale);

        for (int i = 0; i < repeat; i++)
        {
            Resizers.Bilinear(ref result, downscaleTarget);
            Resizers.Bilinear(ref result, initDownscale);
        }

        Resizers.Bilinear(ref result, ogSize);
        return result;
    }

    /// <summary> Blurs an image with frosted glass blur style</summary>
    public static void FrostedGlassBlur(this Pixel32[,] pixel32s_2d, int maxDistance = 3, int repeat = 2, int gaussianBlurRepeat = 8)
    {
        int w = pixel32s_2d.W();
        int h = pixel32s_2d.H();
        Pixel32[,] original = pixel32s_2d;
        Random random = new(Random.Shared.Next(int.MinValue, int.MaxValue));

        pixel32s_2d.GaussianBlur(gaussianBlurRepeat);
        for (int i = 0; i < repeat; i++)
            pixel32s_2d.ForEach((x, y) =>
                random.Next(0, 4) switch
                {
                    0 => pixel32s_2d[Math.Clamp(x + R(), 0, w - 1), y] = original[x, y],
                    1 => pixel32s_2d[x, Math.Clamp(y + R(), 0, h - 1)] = original[x, y],
                    2 => pixel32s_2d[Math.Clamp(x - R(), 0, w - 1), y] = original[x, y],
                    3 => pixel32s_2d[x, Math.Clamp(y - R(), 0, h - 1)] = original[x, y],
                    _ => new(),
                }
            );

        int R() => random.Next(1, maxDistance + 1);
    }

    /// <summary> Blurs an image with frosted glass blur style. use other overload for more control over blurring</summary>
    public static void FrostedGlassBlur(this Pixel32[,] pixel32s_2d, int amoumt)
    {
        int div2 = Math.Clamp(amoumt / 2, 1, int.MaxValue);
        int div4 = Math.Clamp(amoumt / 4, 1, int.MaxValue);
        pixel32s_2d.FrostedGlassBlur(div2, div4, amoumt);
    }
    
}