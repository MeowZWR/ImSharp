#if IMNODES
namespace ImSharp.ImNodes;

public static unsafe partial class ImNodes
{
    internal static Native.Internal.Context* ContextPointer;
    internal static Native.Style*            StylePointer;
    internal static Native.Io*               IoPointer;

    /// <summary> The current style data used by ImNodes. </summary>
    /// <remarks> Make sure to update this by calling <seealso cref="ImSharpPerFrame.OnUpdate"/> at the start of your frames. </remarks>
    public static ImNodesStyle Style
    {
        [MethodImpl(ImSharpConfiguration.Inl)]
        get => StylePointer;
    }

    /// <summary> The current Input/Output data used by ImNodes. </summary>
    /// <remarks> Make sure to update this by calling <seealso cref="ImSharpPerFrame.OnUpdate"/> at the start of your frames. </remarks>
    public static InputOutput Io
    {
        [MethodImpl(ImSharpConfiguration.Inl)]
        get => IoPointer;
    }

    /// <summary> The current internal context used by ImNodes. </summary>
    /// <remarks> Make sure to update this by calling <seealso cref="ImSharpPerFrame.OnUpdate"/> at the start of your frames. </remarks>
    public static ImNodesContext Context
    {
        [MethodImpl(ImSharpConfiguration.Inl)]
        get => ContextPointer;
    }
}
#endif
