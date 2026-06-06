namespace MB28.PngComparer;

/// <summary> Common maths that System.Math and System.MathF doesn't have them. </summary>
public struct MathP
{
    public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp01(t);

    public static float InverseLerp(float a, float b, float value)
    {
        if (a != b)
            return Clamp01((value - a) / (b - a));

        return 0f;
    }

    public static float Clamp01(float v) => Math.Clamp(v, 0f, 1f);

}