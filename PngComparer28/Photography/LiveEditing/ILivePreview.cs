
using System.Diagnostics.CodeAnalysis;

namespace MB28.PngComparer.Photography.LiveEditing;

#nullable enable

[Experimental("PNGC28001", UrlFormat = "")]
/// <summary> Some high-level methods needed for live previewing. </summary>
public interface ILivePreview<T> : IDisposable
{
    /// <summary> TODO: Add summary </summary>
    public delegate void PreviewChangedCallback([NotNull] T output, object? sender, object? otherInfo);

    /// <summary> Use this to get changes. this is called when <see cref="Output"/> value changes </summary>
    public abstract PreviewChangedCallback OnPreviewChanged
    {
        get;
        set;
    }

    [NotNull]
    /// <summary> TODO: Add summary </summary>
    public abstract int RefreshRate
    {
        get;
        set;
    }

    [NotNull]
    /// <summary> TODO: Add summary </summary>
    public abstract bool IsRunning
    {
        get;
        set;
    }

    [NotNull]
    /// <summary> TODO: Add summary </summary>
    public abstract T Input
    {
        get;
        set;
    }

    /// <summary> TODO: Add summary </summary>
    public abstract T? Output
    {
        get;
        set;
    }

    /// <summary> Run this in a background thread. use <see cref="EndPreview"/> to stop it </summary>
    public virtual void StartPreview()
    {
    }

    /// <summary> TODO: Add summary </summary>
    public virtual void EndPreview()
    {
    }

    /// <summary> Updates <see cref="Output"/> </summary>
    public abstract Task Update();

    /// <inheritdoc/>
    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
    }


}
#nullable disable