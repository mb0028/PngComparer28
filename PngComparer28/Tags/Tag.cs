using MB28.PngComparer.Image;

namespace MB28.PngComparer.Tags;

public static class Tag
{
    internal const int CM = 45, CX = 175;

    public static IList<string> GenerateTags(Png png)
    {
        List<string> tags = [];

        var a = png.GetPixels().GetAccentColor();
        if (a.R > CX && a.G < CM && a.B < CM)
            tags.Add("Red");
        if (a.R < CM && a.G > CX && a.B < CM)
            tags.Add("Green");
        if (a.R < CM && a.G < CM && a.B > CX)
            tags.Add("Blue");
        if (a.R > CX && a.G < CM && a.B > CX)
            tags.Add("Purple");
        if (a.R > CX && a.G > CX && a.B < CM)
            tags.Add("Yellow");
        if (a.R < CM && a.G > CX && a.B > CX)
            tags.AddRange("Light Blue", "Aqua", "Sky");
        if (a.R > CX && a.G > CX && a.B > CX)
            tags.AddRange("Bright", "White");
        if (a.R < CM && a.G < CM && a.B < CM)
            tags.AddRange("Dark", "Black", "Night");
        if (a.R > CX && a.G > CX && a.B > CX && a.G < 245)
            tags.Add("Pink");

        string[] tagsShuffle = tags.ToArray();
        Random.Shared.Shuffle(tagsShuffle);

        return tagsShuffle;
    }
}