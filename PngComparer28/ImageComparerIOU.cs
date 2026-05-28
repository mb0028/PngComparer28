using MB28.PngComparer.Image;
using MB28.PngComparer.Drawing;
using MB28.PngComparer.Photography;
using System.Diagnostics;

namespace MB28.PngComparer;

public static partial class ImageComparer
{
	internal static readonly ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 16 };
	internal const string consoleName = "[MBImageCompare Log]";
	internal static sizeP SizeX256 => new(256, 256);

#pragma warning disable CS0618 // Type or member is obsolete (.Bilinear())

	/// <summary>
	/// Compare images using Intersection Over Union. <br/>
	/// IOU Methods are fast but inaccurate result. <br/> 
	/// Images are will be resized to 256x256 then turns into Black and White before comparing.
	/// </summary>
	/// <param name="matchPercentage"> TODO </param>
	/// <returns> True if comparingResult > matchPercentage </returns>
	public static bool CompareIOUFast(string imagePath, string maskImagePath, out float comparingResult, float matchPercentage = 0.75f)
	{
		Png p = Png.Open(imagePath);
		Png m = Png.Open(maskImagePath);
		Pixel32[] imagePx = p.GetPixels().To2dArray(p.SizeP).Bilinear(SizeX256).BackTo1dArray();
		Pixel32[] maskPx = m.GetPixels().To2dArray(m.SizeP).Bilinear(SizeX256).BackTo1dArray();
		imagePx.BW();
		maskPx.BW();

		int intersection = 0;
		int union = 0;
		for (int i = 0; i < imagePx.Length; i++)
		{
			bool one = imagePx[i].R > 128;
			bool two = maskPx[i].R > 128;
			if (one && two) intersection++;
			if (one || two) union++;
		}

		float interOverUni = union == 0 ? 0f : intersection / (float)union;
		if (interOverUni >= matchPercentage)
		{
			Console.WriteLine($"{consoleName} Image {imagePath} Matchs With Image {maskImagePath} with {(Half)(interOverUni * 100)}%");
			comparingResult = interOverUni;
			return true;
		}

		comparingResult = interOverUni;
		return false;
	}


	/// <summary>
	/// Compare images using Intersection Over Union. <br/>
	/// IOU Methods are fast but inaccurate result. <br/> 
	/// Images are will be resized to 256x256 then turns into Black and White and lastly will blurred before comparing.
	/// </summary>
    /// <param name="png"></param>
    /// <param name="masks"></param>
    /// <param name="bestMatchPercentage"></param>
    /// <param name="bestMatchMask"></param>
    /// <param name="matchPercentage"></param>
    /// <returns> True if at least on of <paramref name="masks"/> compare result is > <paramref name="matchPercentage"/>  </returns>
	public static bool CompareIOU(Png png, IList<BWMask> masks, out float bestMatchPercentage, out BWMask bestMatchMask, float matchPercentage = 0.75f)
	{
		Pixel32[] imagePx = png.GetPixels();
		imagePx = imagePx.To2dArray(png.SizeP).Bilinear(SizeX256).BackTo1dArray();
		imagePx.BW();

		var px2d = imagePx.To2dArray(SizeX256);
		px2d.GaussianBlur(16);
		imagePx = px2d.BackTo1dArray();

		float bestMatchPerc = 0;
		int bestMatchIndex = -1;
		for (int i = 0; i < masks.Count; i++)
		{
			Pixel32[] maskPx = masks[i].BWPicture.GetPixels();

			int intersection = 0;
			int union = 0;
			Parallel.For(0, imagePx.Length, parallelOptions, i =>
			{
				bool one = imagePx[i].R > 128;
				bool two = maskPx[i].R > 128;
				if (one && two) intersection++;
				if (one || two) union++;
			});

			float interOverUni = union == 0 ? 0f : intersection / (float)union;
			if (interOverUni >= matchPercentage)
			{
				bestMatchPerc = interOverUni;
				Console.WriteLine($"{consoleName} Compared with {masks[i].MaskName}. Matchs! Current best match: {Math.Round(bestMatchPerc * 100)}");
				if (bestMatchPerc == 1)
				{
					Console.WriteLine($"{consoleName} Comparing stopped because image has 100% similarity with {masks[i].MaskName}.");
					bestMatchPercentage = bestMatchPerc;
					bestMatchMask = masks[i];
					return true;
				}
			}
			else
				Console.WriteLine($"{consoleName} Compared with {masks[i].MaskName}. Not Matchs :( This IOU: {interOverUni} | Current best match: {Math.Round(bestMatchPerc * 100f)}");
		}

		if (bestMatchPerc >= matchPercentage)
		{
			bestMatchPercentage = bestMatchPerc;
			bestMatchMask = masks[bestMatchIndex];
			return true;
		}

		Console.WriteLine($"{consoleName} Compared with all masks but matchs with non of them :( Best match is {masks[bestMatchIndex].MaskName} with {Math.Round(bestMatchPerc * 100f)} similarity.");
		bestMatchPercentage = bestMatchPerc;
		bestMatchMask = masks[bestMatchIndex];
		return false;
	}


#pragma warning restore CS0618 // Type or member is obsolete

}
