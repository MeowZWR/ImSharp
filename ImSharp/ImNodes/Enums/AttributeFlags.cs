#if IMNODES
namespace ImSharp.ImNodes;

/// <summary> Flags controlling the behavior of attribute pins. </summary>
[Flags]
public enum AttributeFlags : uint
{
    /// <summary> No specific behavior. </summary>
    None = 0,

    /// <summary> Allow detaching a link by left-clicking and dragging the link at a pin it is connected to. </summary>
    /// <remarks>
    /// The user has to actually delete the link for this to work. A deleted link can be
    /// detected by calling <seealso cref="ImNodes.Link.LinkDestroyed"/> after disposing <seealso cref="NodeEditorDisposable"/>.
    /// </remarks>
    EnableLinkDetachWithDragClick = 1 << 0,

    /// <summary>
    /// Visual snapping of an in progress link will trigger IsLink Created/Destroyed events.
    /// Allows previewing the creation of a link while dragging it across attributes.
    /// </summary>
    /// <remarks>
    /// The user has to actually delete the link for this to work. A deleted link can be
    /// detected by calling <seealso cref="ImNodes.Link.LinkDestroyed"/> after disposing <seealso cref="NodeEditorDisposable"/>.
    /// </remarks>
    EnableLinkCreationOnSnap = 1 << 1,
}
#endif
