
namespace MB28.PngComparer;

/// <summary>
/// Items with this attribute are not designed good or have performance issues. or some similare issues. but safe to use them. 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, Inherited = false)]
public sealed class FunkyAttribute : Attribute
{

}