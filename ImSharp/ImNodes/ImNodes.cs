namespace ImSharp.ImNodes;

public static unsafe partial class ImNodes
{
    /// <summary> The current ImNodes context. </summary>
    internal static Internal.NodesContext* Context;

    /// <summary> The current editor context. </summary>
    internal static Internal.EditorContext* Editor
        => Context->EditorContext;

    /// <inheritdoc cref="ImSharp.ImNodes.NodeEditor(bool)"/>
    public static NodeEditor NodeEditor()
        => new(true);

    /// <summary> The current style data used by ImNodes. </summary>
    public static ImNodesStyle Style
    {
        [MethodImpl(ImSharpConfiguration.Inl)]
        get => &Context->Style;
    }

    /// <summary> The current Input/Output data used by ImNodes. </summary>
    public static InputOutput Io
    {
        [MethodImpl(ImSharpConfiguration.Inl)]
        get => &Context->Io;
    }
}
