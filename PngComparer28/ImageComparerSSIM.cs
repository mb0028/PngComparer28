
using MB28.PngComparer.Image;
using MB28.PngComparer.Drawing;
using MB28.PngComparer.Photography;

namespace MB28.PngComparer;

public static partial class ImageComparer
{

	/// <summary>
	/// Quickly compares image with mask and returns result.
	/// </summary>
	/// <param name="matchPercentage"> TODO </param>
	/// <returns> SSIM percentage between 0-1 </returns>
	public static double CompareSSIM(string image1, string image2, float downscale = 1)
		=> CompareSSIM(Png.Open(image1), Png.Open(image2), downscale);

    /// <summary>
    /// Quickly compares two Pngs and returns result.
    /// </summary>
    /// <param name="matchPercentage"> TODO </param>
    /// <returns> SSIM percentage between 0-1 </returns>
    public static double CompareSSIM(Png image1, Png image2, float downscale = 1)
	{
		Pixel32[,] onePx = image1.GetPixels().To2dArray(image1.SizeP);
		Pixel32[,] twoPx = image2.GetPixels().To2dArray(image2.SizeP);

		sizeP downscaledSize = new((int)(image1.SizeP.W * downscale), (int)(image1.SizeP.H * downscale));
		Resizers.Bilinear(ref onePx, downscaledSize);
		Resizers.Bilinear(ref twoPx, downscaledSize);

		return ImageMetrics.Ssim(downscaledSize.Width - 1, downscaledSize.Height - 1,
			(i, j) => ImageMetrics.Rgb2Gray(onePx[i, j].R / 255.0, onePx[i, j].G / 255.0, onePx[i, j].B / 255.0),
			(i, j) => ImageMetrics.Rgb2Gray(twoPx[i, j].R / 255.0, twoPx[i, j].G / 255.0, twoPx[i, j].B / 255.0)
		);
	}

}
