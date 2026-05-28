
namespace MB28.PngComparer.Drawing;

/// <summary>
/// Holds RGBA Color <br/>
/// Have operators for (+ - * / == !=) without clamping results. Use Pixel32Extensions.Clamp to clamp
/// </summary>
public struct Pixel32 : IEquatable<Pixel32>
{
    public int R;
    public int G;
    public int B;
    public int A;
    public bool IsGrayscale = false;

    public Pixel32(int r, int g, int b, int a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    public Pixel32(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
        A = 255;
    }

    public Pixel32(int gray)
    {
        R = gray;
        G = gray;
        B = gray;
        A = 255;
        IsGrayscale = true;
    }

    public Pixel32(int r, int g, int b, int a, bool isGrayscale)
    {
        R = r;
        G = g;
        B = b;
        A = a;
        IsGrayscale = isGrayscale;
    }

    public Pixel32(long v) : this()
    {
    }

    /// <summary> Pixel32 with (0, 0, 0, 0) values </summary>
    public static Pixel32 Transparent => new(0, 0, 0, 0);

    /// <summary> Pixel32 with (255, 255, 255, 255) values </summary>
    public static Pixel32 White => new(255, 255, 255, 255);

    /// <summary> Pixel32 with (0, 0, 0, 255) values </summary>
    public static Pixel32 Black => new(0, 0, 0, 255);

    /// <summary> Pixel32 with (255, 0, 0, 255) values </summary>
    public static Pixel32 Red => new(255, 0, 0, 255);

    /// <summary> Pixel32 with (0, 255, 0, 255) values </summary>
    public static Pixel32 Green => new(0, 255, 0, 255);

    /// <summary> Pixel32 with (0, 0, 255, 255) values </summary>
    public static Pixel32 Blue => new(0, 0, 255, 255);


    /// <inheritdoc/>
    public override readonly string ToString() => $"({R}R {G}G {B}B {A}A)";
    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(R, G, B, A, IsGrayscale);


    public bool Equals(Pixel32 other) =>
        (R == other.R) && (G == other.G) && (B == other.B) && (A == other.A);

    public override bool Equals(object obj) =>
        obj is Pixel32 && Equals((Pixel32)obj);


    public static bool operator ==(Pixel32 left, Pixel32 right) =>
        left.Equals(right);

    public static bool operator !=(Pixel32 left, Pixel32 right) =>
        !(left == right);

    public static Pixel32 operator +(Pixel32 left, Pixel32 right) =>
        new Pixel32(left.R + right.R, left.G + right.G, left.B + right.B, left.A + right.A);


    public static Pixel32 operator -(Pixel32 left, Pixel32 right) =>
        new Pixel32(left.R - right.R, left.G - right.G, left.B - right.B, left.A - right.A);

    public static Pixel32 operator -(int left, Pixel32 right) =>
        new Pixel32(left - right.R, left - right.G, left - right.B, left - right.A);


    public static Pixel32 operator *(Pixel32 left, Pixel32 right) =>
        new Pixel32(left.R * right.R, left.G * right.G, left.B * right.B, left.A * right.A);

    public static Pixel32 operator *(int left, Pixel32 right) =>
        new Pixel32(left * right.R, left * right.G, left * right.B, left * right.A);

    public static Pixel32 operator *(Pixel32 left, int right) =>
        new Pixel32(right * left.R, right * left.G, right * left.B, right * left.A);


    public static bool operator >(Pixel32 left, Pixel32 right) =>
        (left.R > right.R) && (left.G > right.G) && (left.B > right.B);

    public static bool operator <(Pixel32 left, Pixel32 right) =>
        (left.R < right.R) && (left.G < right.G) && (left.B < right.B);
 
 
    public static Pixel32 operator /(Pixel32 left, Pixel32 right) =>
        new Pixel32(
            (int)Math.Round(left.R / (float)right.R),
            (int)Math.Round(left.G / (float)right.G),
            (int)Math.Round(left.B / (float)right.B),
            (int)Math.Round(left.A / (float)right.A)
        );

    public static Pixel32 operator /(Pixel32 left, int right) =>
        new Pixel32(
            (int)Math.Round(left.R / (float)right),
            (int)Math.Round(left.G / (float)right),
            (int)Math.Round(left.B / (float)right),
            (int)Math.Round(left.A / (float)right)
        );

}
