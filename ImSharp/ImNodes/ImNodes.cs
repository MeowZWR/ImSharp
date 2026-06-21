namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    /// <inheritdoc cref="NodeDisposable(NodeId)"/>
    public static NodeDisposable Node(NodeId id)
        => new(id);

    /// <inheritdoc cref="NodeDisposable(NodeId,bool)"/>
    public static NodeDisposable NodeReference(NodeId id)
        => new(id, true);

    /// <inheritdoc cref="NodeEditorDisposable(bool)"/>
    public static NodeEditorDisposable NodeEditor()
        => new(true);
}
