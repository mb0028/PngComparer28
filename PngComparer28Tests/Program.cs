
using MB28.PngComparerTests;


// First 2 tests are update files in /TestImages/  folder
#region Test All

Tests.TestComparers();

Tests.TestEditing();

Tests.TestTagGenerator();

// Tests.Pixel32MathAndOperations();

// await Task.Run(async () => await Tests.LiveBlurTest(10));

#endregion





// All my tests with: Intel Core i5-11400  |  Release Any CPU
#region Benchmarks

// Min: 52.5816ms | Max: 87.6076ms | Average: 58.8ms
// await Tests.BenchmarkArrayConverting_X2000();

// Old, obsoleted overload:   Min: 77.4499ms | Max: 98.7546ms | Average: 81.4ms
// New, ref overload:    Min: 69.007ms | Max: 86.1496ms | Average: 74.06ms
// await Tests.BenchmarkBilinear_X128_To_x2000();

#endregion