
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MB28.PngComparer;
using MB28.PngComparer.Image;
using MB28.PngComparer.Drawing;
using MB28.PngComparer.Photography.LiveEditing;
using MB28.PngComparer.Tags;

namespace MB28.PngComparerTests;

internal static partial class Tests
{
    public static void Pixel32MathAndOperations()
    {
        Console.WriteLine($"Average (200,0,30,25): {new Pixel32(200, 0, 0).Average([new Pixel32(0, 0, 0), new Pixel32(30, 0, 0), new Pixel32(25, 0, 0)])}");
        Console.WriteLine($"Add (50+53): {new Pixel32(50, 0, 0) + new Pixel32(53, 0, 0)}");
        Console.WriteLine($"Sub (50-53): {new Pixel32(50, 0, 0) - new Pixel32(53, 0, 0)}");
        Console.WriteLine($"Div (200/20): {new Pixel32(200, 0, 0) / new Pixel32(20, 0, 0)}");
        Console.WriteLine($"Mul (50*53): {new Pixel32(50, 0, 0) * new Pixel32(53, 0, 0)}");
        Console.WriteLine($"Mul & Clamp (50*53).Clamp(): {(new Pixel32(50, 0, 0) * new Pixel32(53, 0, 0)).Clamp()}\n");

        Console.WriteLine($"== (50==50): {new Pixel32(50, 0, 0) == new Pixel32(50, 0, 0)}");
        Console.WriteLine($"== (50==53): {new Pixel32(50, 0, 0) == new Pixel32(53, 0, 0)}");
        Console.WriteLine($"!= (50!=50): {new Pixel32(50, 0, 0) != new Pixel32(50, 0, 0)}");
        Console.WriteLine($"!= (50!=53): {new Pixel32(50, 0, 0) != new Pixel32(53, 0, 0)}");
    }

    public static void TestTagGenerator()
    {
        Console.WriteLine("Loading tags...");
        foreach (var tag in Tag.GenerateTags(Png.Open($"{imagesDir}/{fileName}.png")))
            Console.WriteLine(tag);
    }
    
    #pragma warning disable CS0618 // Type or member is obsolete

    private static Int128 int128 = 0;
    private static int s = 0;
    public static async Task LiveBlurTest(int refrate)
    {
        var png = Png.Open($"{imagesDir}/{fileName}.png");

        Stopwatch stopwatch = new();
        Stopwatch second = new();

        LiveBlur liveBlur = new(png.GetPixels().To2dArray(png.SizeP), refrate, 8);

        liveBlur.OnPreviewChanged += ([NotNull] a, b, c) =>
        {
            if (second.Elapsed.TotalSeconds > 1)
            {
                Console.WriteLine($"----------------------------| this second fps: {s}fps");
                s = 0;
                second.Restart();
            }
            // Console.WriteLine($"--------- {int128} --| {stopwatch.Elapsed.TotalSeconds}ms");
            // stopwatch.Restart();
            int128++;
            s++;
        };

        stopwatch.Start();
        second.Start();

        liveBlur.StartPreview();
        await Task.Delay(10000);
        liveBlur.EndPreview();
    }
    
    #pragma warning restore CS0618 // Type or member is obsolete
}