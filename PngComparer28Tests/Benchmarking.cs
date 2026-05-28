using System.Diagnostics;
using MB28.PngComparer;
using MB28.PngComparer.Drawing;
using MB28.PngComparer.Photography;

namespace MB28.PngComparerTests;

internal static partial class Tests
{
    static Pixel32[] picTestX2000 = new Pixel32[2000 * 2000];
    static Pixel32[,] picTestX128 = new Pixel32[128, 128];

    public static async Task BenchmarkArrayConverting_X2000()
        => await Common(() => picTestX2000.To2dArray(new(2000, 2000)).BackTo1dArray(), "Array Converting");

    public static async Task BenchmarkBilinear_X128_To_x2000()
        => await Common(() => {
            var p = picTestX128;
            Resizers.Bilinear(ref p, new(2000, 2000));
        }, "Bilinear");

    private static Task Common(Action action, string name, int times = 100)
    {
        Stopwatch stopwatch = new();
        double[] av = new double[times];
        for (short i = 0; i < times; i++)
        {
            stopwatch.Restart();
            action.Invoke();
            av[i] = stopwatch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"--) Benchmark [{name}] {i} in {av[i]}ms");
        }
        stopwatch.Stop();
        Console.WriteLine($"--) Benchmark [{name}] Finished. Min: {av.Min()}ms | Max: {av.Max()}ms | Average: {(Half)av.Average()}ms");
        return Task.CompletedTask;
    }

    

}