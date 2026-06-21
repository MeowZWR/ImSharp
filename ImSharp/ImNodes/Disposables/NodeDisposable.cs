namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    /// <summary> A wrapper around ImNodes nodes. </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe ref struct NodeDisposable : IDisposable
    {
        /// <summary> The unique ID of the node. </summary>
        public readonly NodeId Id;

        /// <summary> Whether the node is already ended. </summary>
        public bool Alive { get; private set; }

        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static implicit operator NodeId(NodeDisposable node)
            => node.Id;

        /// <summary> Begin a new node inside the current editor. </summary>
        /// <param name="id"> The desired unique ID of the new node. Can be any integer except for <seealso cref="int.MinValue"/>. </param>
        /// <returns> A disposable object that ends the node on disposal. Use with using. </returns>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        internal NodeDisposable(NodeId id)
        {
            Api.BeginNode(id);
            Id    = id;
            Alive = true;
        }

        /// <summary> Create a reference to an existing node without beginning it. </summary>
        /// <param name="id"> The unique ID of the existing node. </param>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        internal NodeDisposable(NodeId id, bool _)
        {
            Id    = id;
            Alive = false;
        }

        /// <summary> Set the ability to click and drag this node. </summary>
        /// <param name="draggable"> Whether the node should be draggable or not. </param>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public readonly void SetDraggable(bool draggable)
            => Api.SetNodeDraggable(Id, draggable);

        /// <inheritdoc cref="NodeTitleBarDisposable(bool)"/>
        public readonly NodeTitleBarDisposable TitleBar()
            => Alive ? new NodeTitleBarDisposable(true) : default;

        /// <summary> Get the dimensions of this node. </summary>
        public readonly Vector2 Dimensions
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                ImVec2 ret;
                Api.GetNodeDimensions(&ret, Id);
                return ret;
            }
        }

        /// <summary> Get or set the position of this node in the screen space coordinate system, i.e. relative to the upper left corner of the containing window. </summary>
        public readonly Vector2 ScreenSpacePosition
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                ImVec2 ret;
                Api.GetNodeScreenSpacePos(&ret, Id);
                return ret;
            }
            [MethodImpl(ImSharpConfiguration.OptInl)]
            set => Api.SetNodeScreenSpacePos(Id, value);
        }

        /// <summary> Get or set the position of this node in the editor coordinate system, i.e. relative to the upper left corner of the containing node editor. </summary>
        public readonly Vector2 EditorSpacePosition
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                ImVec2 ret;
                Api.GetNodeEditorSpacePos(&ret, Id);
                return ret;
            }
            [MethodImpl(ImSharpConfiguration.OptInl)]
            set => Api.SetNodeEditorSpacePos(Id, value);
        }

        /// <summary> Get or set the position of this node in the grid coordinate system, i.e. relative to the upper left corner of the containing node editor translated by the current panning (see <seealso cref="ImNodes.EditorContext.Panning"/>). </summary>
        public readonly Vector2 GridSpacePosition
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                ImVec2 ret;
                Api.GetNodeGridSpacePos(&ret, Id);
                return ret;
            }
            [MethodImpl(ImSharpConfiguration.OptInl)]
            set => Api.SetNodeGridSpacePos(Id, value);
        }

        /// <summary> Snap this node's origin to the grid if <seealso cref="ImNodesStyleFlags.GridSnapping"/> is enabled. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public readonly void SnapToGrid()
            => Api.SnapNodeToGrid(Id);

        /// <summary> Get whether this node is currently hovered by the mouse cursor. </summary>
        /// <remarks> Use after disposing the <seealso cref="NodeEditorDisposable"/>. </remarks>
        public readonly unsafe bool Hovered
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                NodeId id;
                if (!Api.IsNodeHovered(&id))
                    return false;

                return id == Id;
            }
        }

        /// <summary> Get or set the selection state of this node. </summary>
        /// <remarks> Selecting an already selected node, or unselecting an unselected node, is an error. </remarks>
        public readonly bool Selected
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get => Api.IsNodeSelected(Id);
            [MethodImpl(ImSharpConfiguration.OptInl)]
            set
            {
                if (value)
                    Api.SelectNode(Id);
                else
                    Api.ClearNodeSelection(Id);
            }
        }

        /// <summary> End the node on leaving scope. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void Dispose()
        {
            if (!Alive)
                return;

            Api.EndNode();
            Alive = false;
        }
    }
}
