
namespace MB28.PngComparer.More;

/// <summary>
/// Throws when resizing goes wrong
/// </summary>
public class ResizingException : InvalidOperationException
{
    public ResizingException(string message) : base(message)
    {
    }
}