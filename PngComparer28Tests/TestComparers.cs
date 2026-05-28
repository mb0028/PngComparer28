
using MB28.PngComparer;

namespace MB28.PngComparerTests;

internal static partial class Tests
{
    static readonly string imagesDir = @"C:\Users\mb28\Desktop\PngComparer28\TestImages";
    static readonly string fileName = "Photo";

    public static void TestComparers()
    {
        // ------------------------------------------------------------
        // Comparers ------------------------------------------------------------
        var rIOU = ImageComparer.CompareIOUFast($"{imagesDir}/{fileName}.png", $"{imagesDir}/{fileName}3.png",
            out float match);
        File.WriteAllText($"{imagesDir}/IOU Result (CompareFast).txt",
            $"Compared {imagesDir}/{fileName}.png\nwith {imagesDir}/{fileName}3.png\n" +
            $"Matchs: {rIOU} | MatchPercentage: {(Half)(match * 100f)}%");

        var resultSSIM = ImageComparer.CompareSSIM($"{imagesDir}/{fileName}.png", $"{imagesDir}/{fileName}3.png");
        File.WriteAllText($"{imagesDir}/SSIM Result.txt",
            $"Compared {imagesDir}/{fileName}.png\nwith {imagesDir}/{fileName}3.png\nSSIM: {resultSSIM}");
    }
}