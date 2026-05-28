
namespace MB28.PngComparer.Drawing;

/// <summary> Stores 2 readonly int: Width and Height</summary>
public readonly struct SizeP(int w, int h)
{
    public int Width { get; } = w;
    public int Height { get; } = h;

    /// <summary>
    /// same as Width
    /// </summary>
    public int W => Width;
    /// <summary>
    /// same as Height
    /// </summary>
    public int H => Height;
}

/// <summary> Stores 2 readonly float: Width and Height</summary>
public readonly struct SizePF(float w, float h)
{
    public float Width { get; } = w;
    public float Height { get; } = h;

    /// <summary>
    /// same as Width
    /// </summary>
    public float W => Width;
    /// <summary>
    /// same as Height
    /// </summary>
    public float H => Height;
}
