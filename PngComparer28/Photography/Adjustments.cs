
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
    /// <summary> Change image's brightness </summary>
    /// <param name="addPerc"> from -1.0 to 1.0 </param>
    public static void Brightness(this Pixel32[] p, float addPerc)
    {
        int add = Math.Clamp((int)(addPerc * 255), -255, 255);
        p.ForEach(i =>
        {
            Pixel32 pixel = p[i];
            return new Pixel32(pixel.R + add, pixel.G + add, pixel.B + add, pixel.A).Clamp();
        });
    }

    /// <summary> Change image's contrast </summary>
    /// <param name="addPerc"> from -2.0 to 2.0 </param>
    public static void Contrast(this Pixel32[] p, float addPerc)
    {
        int add = Math.Clamp((int)(addPerc * 255), -512, 512);
        float gray = 0;

        for (int i = 0; i < p.Length; i++)
            gray += p[i].R * 0.2126f + p[i].G * 0.7152f + p[i].B * 0.0722f;
        gray /= p.Length;

        p.ForEach(i =>
        {
            Pixel32 pixel = p[i];
            return new Pixel32((int)(pixel.R + ((pixel.R - gray) * add / 255f)),
                (int)(pixel.G + ((pixel.G - gray) * add / 255f)),
                (int)(pixel.B + ((pixel.B - gray) * add / 255f)),
                pixel.A).Clamp();
        });
    }

    /// <summary> Change image's saturation </summary>
    /// <param name="addPerc"> from -2.0 to 2.0 </param>
    public static void Saturation(this Pixel32[] p, float addPerc)
    {
        int add = Math.Clamp((int)(addPerc * 255), -512, 512);
        p.ForEach(i =>
        {
            Pixel32 pixel = p[i];
            float gray = pixel.R * 0.2126f + pixel.G * 0.7152f + pixel.B * 0.0722f;
            return new Pixel32((int)(pixel.R + ((pixel.R - gray) * add / 255f)),
                (int)(pixel.G + ((pixel.G - gray) * add / 255f)),
                (int)(pixel.B + ((pixel.B - gray) * add / 255f)),
                pixel.A).Clamp();
        });
    }

    /// <summary> Change image's vibrance </summary>
    /// <param name="addPerc"> from 0.01 to 5.0 (1.0 = no change) </param>
    public static void Vibrance(this Pixel32[] p, float addPerc)
    {
        addPerc = Math.Clamp(addPerc, 0.01f, 5.0f);
        p.ForEach(i =>
        {
            Pixel32 pixel = p[i];
            float gray = pixel.R * 0.2126f + pixel.G * 0.7152f + pixel.B * 0.0722f;
            int addVibranceR = (int)((255 - pixel.R) * addPerc);
            int addVibranceG = (int)((255 - pixel.G) * addPerc);
            int addVibranceB = (int)((255 - pixel.B) * addPerc);
            return new Pixel32((int)(pixel.R + ((pixel.R - gray) * addVibranceR / 255f)),
                (int)(pixel.G + ((pixel.G - gray) * addVibranceG / 255f)),
                (int)(pixel.B + ((pixel.B - gray) * addVibranceB / 255f)),
                pixel.A).Clamp();
        });
    }

    /// <summary> Change image's gamma </summary>
    /// <remarks> This is not a real gamma modification. its more like Brightness + Gamma + Saturation </remarks>
    /// <param name="addPerc"> from 0.0 to 5.0 </param>
    public static void Gamma(this Pixel32[] p, float addPerc)
    {
        float gamma = Math.Clamp(addPerc, 0f, 5.0f);
        float level = 1.0f / (gamma / 100.0f);

        double[] pow = new double[256];
        for (int i = 0; i < 256; i++)
        {
            double value = 255 * Math.Pow(i / 255.0, level) + 0.5;
            if (value > 255) value = 255;
            else if (value < 0) value = 0;
            else value = Math.Floor(value);
            pow[i] = value;
        }

        int add = (int)(addPerc * 255);
        p.ForEach(i =>
        {
            Pixel32 pixel = p[i];
            return new Pixel32((int)(pixel.R + ((pixel.R - pow[pixel.R]) * add / 255f)),
                (int)(pixel.G + ((pixel.G - pow[pixel.G]) * add / 255f)),
                (int)(pixel.B + ((pixel.B - pow[pixel.B]) * add / 255f)),
                pixel.A).Clamp();
        });
    }

    /// <summary> Change image's shadows, midtones & highlights </summary>
    public static void ShadowsMidtonesHighlights(this Pixel32[] p, float shadows = 1.0f, float midtones = 1.0f, float highlights = 1.0f)
    {
        p.ForEach(i =>
        {
            Pixel32 pixel = p[i];
            float brightness = Lum.Brightness(pixel);

            if (brightness < 80)
                pixel = new Pixel32((int)(pixel.R * shadows), (int)(pixel.G * shadows), (int)(pixel.B * shadows), pixel.A);
            else if (brightness > 80 && brightness < 175)
                pixel = new Pixel32((int)(pixel.R * midtones), (int)(pixel.G * midtones), (int)(pixel.B * midtones), pixel.A);
            else if (brightness > 175)
                pixel = new Pixel32((int)(pixel.R * highlights), (int)(pixel.G * highlights), (int)(pixel.B * highlights), pixel.A);

            return pixel.Clamp();
        });
    }

    /// <summary> Change image's hue </summary>
    /// <param name="addPerc"> from -1.0 to 2.0 </param>
    public static void Hue(this Pixel32[] p, float offset)
    {
        p.ForEach(i =>
        {
            Pixel32 pixel = p[i];
            return new Pixel32().Clamp();
        });
    }
}