
using MB28.PngComparer.Image;
using MB28.PngComparer.Photography;

namespace MB28.PngComparer.Drawing;

/// <summary>
/// Holds a readonly 256x256 Black and White <see cref="Png"/> with a name for it to help image comparing result
/// </summary>
public readonly struct BWMask
{

    /// <summary> The 256x256 Black and White <see cref="Png"/> </summary>
    public Png BWPicture { get; }

    /// <summary> Mask name. Example uses: for displaing it to user or debugging </summary>
    public string MaskName { get; }

    /// <summary> Creates new instance or <see cref="BWMask"/> and automatically turns Png into 256x256 Black and White. </summary>
    /// <param name="png"> input <see cref="Png"/> that will converts to 256x256 Black and White </param>
    /// <param name="maskName"> Name for this mask </param>
    public BWMask(Png png, string maskName)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Pixel32[] Pixel32s = png.GetPixels().To2dArray(png.SizeP).Bilinear(ImageComparer.SizeX256).BackTo1dArray();
#pragma warning restore CS0618 // Type or member is obsolete
        Pixel32s.BW();
        var builder = PngBuilder.FromPixel32_2d(Pixel32s.To2dArray(ImageComparer.SizeX256), false);
        BWPicture = Png.Open(builder.Save());
        MaskName = maskName;
    }
}
