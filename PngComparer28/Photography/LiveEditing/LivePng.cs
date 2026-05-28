
using MB28.PngComparer.Image;
using MB28.PngComparer.Drawing;
using static MB28.PngComparer.Photography.LiveEditing.LivePng;

namespace MB28.PngComparer.Photography.LiveEditing;

#pragma warning disable PNGC28001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

[Funky]
/// <summary>
/// TODO: Add summary
/// </summary>
/// <param name="input">TODO: Add summary</param>
/// <param name="updatePngPixels">TODO: Add summary</param>
/// <param name="refreshRate">TODO: Add summary</param>
public class LivePng(Png input, UpdatePngPixels onUpdatePngPixels, int refreshRate = 100) : ILivePreview<Png>
{
    public ILivePreview<Png>.PreviewChangedCallback OnPreviewChanged { get; set; }
    public int RefreshRate { get; set; } = refreshRate;
    public bool IsRunning { get; set; } = false;
    public Png Input { get; set; } = input;
    public Png Output
    {
        get => outpo;
        set
        {
            outpo = value;
            OnPreviewChanged(value, this, null);
        }
    }
    private Png outpo;
    private readonly UpdatePngPixels updatePixels = onUpdatePngPixels;
    public delegate Pixel32[] UpdatePngPixels();

    public async void StartPreview()
    {
        IsRunning = true;
        while (IsRunning)
        {
            await Update();
            await Task.Delay(RefreshRate);
        }
    }

    public void EndPreview()
    {
        IsRunning = false;
        Output = null;
    }

    public Task Update()
    {
        Output = Png.Open(PngBuilder.FromPixel32(updatePixels(), Input.Width, Input.Height, Input.HasAlphaChannel).Save());
        return Task.CompletedTask;
    }
}
#pragma warning restore PNGC28001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.