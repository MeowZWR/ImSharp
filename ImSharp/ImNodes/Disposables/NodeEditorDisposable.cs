#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    /// <summary> A wrapper around an ImNodes node editor. </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref struct NodeEditorDisposable : IDisposable
    {
        /// <summary> Whether the node editor is already ended. </summary>
        public bool Alive { get; private set; }

        /// <summary> Begin a new node editor. </summary>
        /// <returns> A disposable object that ends the node editor on disposal. Use with using. </returns>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        internal NodeEditorDisposable(bool _)
        {
            Alive = true;
            Native.Methods.Editor.BeginNodeEditor();
        }

        /// <summary> End the node editor on leaving scope. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void Dispose()
        {
            if (!Alive)
                return;

            Alive = false;
            Native.Methods.Editor.EndNodeEditor();
        }

        /// <summary> Get whether this node editor is currently hovered by the mouse cursor. </summary>
        public readonly bool Hovered
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get => Native.Methods.Editor.IsEditorHovered();
        }


        /// <inheritdoc cref="MiniMap(float,Action{NodeId,nint},nint,MiniMapLocation)"/>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public readonly unsafe void MiniMap(float sizeFraction = 0.2f, MiniMapLocation location = MiniMapLocation.TopRight)
            => Native.Methods.Editor.MiniMap(sizeFraction, location, null, nint.Zero);

        /// <inheritdoc cref="MiniMap(float,Action{NodeId,nint},nint,MiniMapLocation)"/>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        [OverloadResolutionPriority(50)]
        public readonly unsafe void MiniMap(float sizeFraction, Action<NodeId> action, MiniMapLocation location = MiniMapLocation.TopRight)
        {
            var ptr = (delegate* unmanaged<NodeId, nint, void>)Marshal.GetFunctionPointerForDelegate(NewAction);
            Native.Methods.Editor.MiniMap(sizeFraction, location, ptr, nint.Zero);
            return;

            void NewAction(NodeId nodeId, nint data)
                => action(nodeId);
        }

        /// <summary> Add a navigable mini map to the editor. </summary>
        /// <param name="sizeFraction"> The fraction of the editors size the mini map should take up. </param>
        /// <param name="location"> The anchor position for the minimap. </param>
        /// <param name="action"> Behavior when hovering over a node in the mini map. </param>
        /// <param name="data"> Additional user data for the behavior action beyond the hovered node ID. </param>
        /// <remarks> Call this before disposing this <seealso cref="NodeEditorDisposable"/> but after all nodes and links have been established. </remarks>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        [OverloadResolutionPriority(50)]
        public readonly unsafe void MiniMap(float sizeFraction, Action<NodeId, nint> action, nint data = 0,
            MiniMapLocation location = MiniMapLocation.TopRight)
        {
            var ptr = (delegate* unmanaged<NodeId, nint, void>)Marshal.GetFunctionPointerForDelegate(action);
            Native.Methods.Editor.MiniMap(sizeFraction, location, ptr, data);
        }

        /// <inheritdoc cref="MiniMap(float,Action{NodeId,nint},nint,MiniMapLocation)"/>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        [OverloadResolutionPriority(100)]
        public readonly unsafe void MiniMap(float sizeFraction, delegate* unmanaged<NodeId, nint, void> action, nint data = 0,
            MiniMapLocation location = MiniMapLocation.TopRight)
            => Native.Methods.Editor.MiniMap(sizeFraction, location, action, data);

        /// <inheritdoc cref="MiniMap(float,Action{NodeId,nint},nint,MiniMapLocation)"/>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        [OverloadResolutionPriority(100)]
        public readonly unsafe void MiniMap(float sizeFraction, delegate* unmanaged<NodeId, void*, void> action, void* data = null,
            MiniMapLocation location = MiniMapLocation.TopRight)
            => Native.Methods.Editor.MiniMap(sizeFraction, location, (delegate* unmanaged<NodeId, nint, void>)action, (nint)data);
    }
}
#endif
