
using MB28.PngComparer.Drawing;

namespace MB28.PngComparer.Photography.LiveEditing;

#pragma warning disable PNGC28001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

[Obsolete(deprecated)]
/// <summary>
/// Contains methods for live blurring with minimal GC load and ~40mb ram usage. <br/>
/// Tested (with Intel Core i5-11400): ~510fps
/// </summary>
public class LiveBlur(Pixel32[,] input, int refreshRate, int blurAmount = 8) : ILivePreview<Pixel32[,]>
{
    const string deprecated = "There is something wrong with this method...";

    public ILivePreview<Pixel32[,]>.PreviewChangedCallback OnPreviewChanged { get; set; }
    public int RefreshRate { get; set; } = refreshRate;
    public Pixel32[,] Input { get; set; } = input;
    public Pixel32[,] Output
    {
        get => output;
        set
        {
            output = value;
            OnPreviewChanged(value, this, outputSize);
        }
    }
    public bool IsRunning { get; set; } = false;
    
    public void SetBlurAmount(int value) => blurAmount = value;

    /// <summary>
    /// TODO: Add summary
    /// </summary>
    /// <param name="input">TODO: Add summary</param>
    /// <param name="updateRateIn_ms"> 100 -> 10fps | 50 -> 20fps | 25 -> 40fps | 20 -> 50fps | 10 -> 100fps</param>
    /// <param name="blurAmount">TODO: Add summary</param>
    /// <param name="downFac"> TODO: Add summary </param>
    public async void StartPreview(int downFac = 6)
    {
        Input = Input.Bilinear(new(Input.GetLength(0) / downFac, Input.GetLength(1) / downFac));

        outputSize = new(Input.GetLength(0), Input.GetLength(1));
        output = new Pixel32[outputSize.W, outputSize.H];
        temp = new Pixel32[outputSize.W, outputSize.H];
        oSizeX = outputSize.W;
        oSizeY = outputSize.H;

        IsRunning = true;
        while (IsRunning)
        {
            await Update();
            await Task.Delay(RefreshRate);
        }
    }

    public async Task Update()
    {
        await NextLiveGaussianBlurFrame();
    }

    /// <summary> TODO: Add summary </summary>
    /// <param name="mode"> TODO: Add summary </param>
    public void EndPreview() 
    {
        IsRunning = false;
        Output = null;
        Input = null;
        outputSize = new(0, 0);
        oSizeX = 0;
        oSizeY = 0;
        nH = Pixel32.Transparent;
        nV = Pixel32.Transparent;
        pH = Pixel32.Transparent;
        pV = Pixel32.Transparent;
        aved = Pixel32.Transparent;
    }

    private static Pixel32[,] output;
    private static Pixel32[,] temp;
    private static sizeP outputSize;
    private static int oSizeX;
    private static int oSizeY;
    private static Pixel32 nH;
    private static Pixel32 nV;
    private static Pixel32 pH;
    private static Pixel32 pV;
    private static Pixel32 aved;
    private int blurAmount = blurAmount;
    
    private void Average5(int x, int y) => aved = (Input[x, y] + nH + nV + pH + pV) / 5;

    private Task NextLiveGaussianBlurFrame()
    {
        for (int i = 0; i < blurAmount; i++)
            for (int y = 0; y < oSizeY; y++)
                for (int x = 0; x < oSizeX; x++)
                {
                    nH = Input[Math.Clamp(x + 1, 0, oSizeX - 1), y];
                    nV = Input[x, Math.Clamp(y + 1, 0, oSizeY - 1)];
                    pH = Input[Math.Clamp(x - 1, 0, oSizeX - 1), y];
                    pV = Input[x, Math.Clamp(y - 1, 0, oSizeY - 1)];
                    Average5(x, y);
                    temp[x, y] = aved;
                }
        Output = temp;
        return Task.CompletedTask;
    }
}

#pragma warning restore PNGC28001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.