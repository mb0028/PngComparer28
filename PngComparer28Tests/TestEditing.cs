
using MB28.PngComparer;
using MB28.PngComparer.Image;
using MB28.PngComparer.Drawing;
using MB28.PngComparer.Photography;
using static MB28.PngComparer.Photography.BlendingUtils;

namespace MB28.PngComparerTests;

internal static partial class Tests
{
    public static void TestEditing()
    {
        var portrait = Png.Open($"{imagesDir}/{fileName}.png");

        // ------------------------------------------------------------
        // Blurs ------------------------------------------------------------
        Pixel32[,] blur = portrait.GetPixels().To2dArray(portrait.SizeP);
        blur.GaussianBlur();
        var blurBuilder = PngBuilder.FromPixel32_2d(blur, portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_GaussianBlur.png", blurBuilder.Save());

        Pixel32[,] blurKawa = portrait.GetPixels().To2dArray(portrait.SizeP).KawaseBlur();
        var kawaBuilder = PngBuilder.FromPixel32_2d(blurKawa, portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_KawaseBlur.png", kawaBuilder.Save());

        Pixel32[,] glassBlur = portrait.GetPixels().To2dArray(portrait.SizeP);
        glassBlur.FrostedGlassBlur();
        var glassBlurBuilder = PngBuilder.FromPixel32_2d(glassBlur, portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_FrostedGlassBlur.png", glassBlurBuilder.Save());

        Pixel32[,] glare = portrait.GetPixels().To2dArray(portrait.SizeP);
        glare.Bloom(out Pixel32[,] bl, 200);
        var glareBuilder = PngBuilder.FromPixel32_2d(glare, portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Bloom.png", glareBuilder.Save());
        var glareLayerBuilder = PngBuilder.FromPixel32_2d(bl, portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_BloomLayer.png", glareLayerBuilder.Save());


        // ------------------------------------------------------------
        // Filters ------------------------------------------------------------
        Pixel32[] bw = portrait.GetPixels();
        bw.BW();
        var bwBuilder = PngBuilder.FromPixel32_2d(bw.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_BW.png", bwBuilder.Save());

        Pixel32[] gray = portrait.GetPixels();
        gray.Grayscale();
        var grayBuilder = PngBuilder.FromPixel32_2d(gray.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Grayscale.png", grayBuilder.Save());

        Pixel32[] invert = portrait.GetPixels();
        invert.InvertColors();
        var invertBuilder = PngBuilder.FromPixel32_2d(invert.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Inverted.png", invertBuilder.Save());

        Pixel32[] colorOverlay = portrait.GetPixels();
        var accentBuilder = PngBuilder.Create(1, 1, false);
        accentBuilder.SetPixel(colorOverlay.GetAccentColor(), 0, 0);
        File.WriteAllBytes($"{imagesDir}/{fileName}_AccentColor.png", accentBuilder.Save());

        colorOverlay.ColorOverlay(Pixel32.Red);
        var colorOverlayBuilder = PngBuilder.FromPixel32_2d(colorOverlay.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_ColorOverlay.png", colorOverlayBuilder.Save());

        Pixel32[] posterize = portrait.GetPixels();
        posterize.Posterize(4);
        var posterizeBuilder = PngBuilder.FromPixel32_2d(posterize.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Posterized.png", posterizeBuilder.Save());


        // ------------------------------------------------------------
        // Resizers ------------------------------------------------------------
        Pixel32[,] bilinear = portrait.GetPixels().To2dArray(portrait.SizeP);
        Resizers.Bilinear(ref bilinear, new(1000, 1000));
        var bilinearBuilder = PngBuilder.FromPixel32_2d(bilinear, portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_BilinearResizeTo1000x1000.png", bilinearBuilder.Save());


        // ------------------------------------------------------------
        // Adjustments ------------------------------------------------------------
        Pixel32[] brightnessAdd = portrait.GetPixels();
        brightnessAdd.Brightness(0.5f);
        var brightBuilder = PngBuilder.FromPixel32_2d(brightnessAdd.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Adjustment_BrightnessAdd.png", brightBuilder.Save());

        Pixel32[] contrastAdd = portrait.GetPixels();
        contrastAdd.Contrast(0.5f);
        var contrastBuilder = PngBuilder.FromPixel32_2d(contrastAdd.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Adjustment_ContrastAdd.png", contrastBuilder.Save());

        Pixel32[] saturationAdd = portrait.GetPixels();
        saturationAdd.Saturation(0.5f);
        var satuBuilder = PngBuilder.FromPixel32_2d(saturationAdd.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Adjustment_SaturationAdd.png", satuBuilder.Save());

        Pixel32[] vibranceAdd = portrait.GetPixels();
        vibranceAdd.Vibrance(1.5f);
        var vibranceBuilder = PngBuilder.FromPixel32_2d(vibranceAdd.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Adjustment_VibranceAdd.png", vibranceBuilder.Save());

        Pixel32[] gammaAdd = portrait.GetPixels();
        gammaAdd.Gamma(0.5f);
        var gammaBuilder = PngBuilder.FromPixel32_2d(gammaAdd.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Adjustment_GammaAdd.png", gammaBuilder.Save());

        Pixel32[] smh = portrait.GetPixels();
        smh.ShadowsMidtonesHighlights(0.2f, 0.7f, 1.2f);
        var smhBuilder = PngBuilder.FromPixel32_2d(smh.To2dArray(portrait.SizeP), portrait.HasAlphaChannel);
        File.WriteAllBytes($"{imagesDir}/{fileName}_Adjustment_ShaMidHigh.png", smhBuilder.Save());

        for (int i = 0; i < 6; i++) {
            var blendingBuilder = PngBuilder.FromPng(Blend($"{imagesDir}/{fileName}.png", $"{imagesDir}/{fileName}3.png", (BlendingMode)i));
            File.WriteAllBytes($"{imagesDir}/{fileName}_Blending_{Enum.GetName((BlendingMode)i)}.png", blendingBuilder.Save());
        }
        
    }
}