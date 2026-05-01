#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public static partial class Methods
        {
            public static unsafe partial class Node
            {
                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_BeginNode")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void BeginNode(NodeId id);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EndNode")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void EndNode();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetNodeDimensions")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void GetNodeDimensions(ImVec2* ret, NodeId id);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_BeginNodeTitleBar")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void BeginNodeTitleBar();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EndNodeTitleBar")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void EndNodeTitleBar();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SetNodeDraggable")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SetNodeDraggable(NodeId nodeId, ImBool draggable);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SetNodeScreenSpacePos")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SetNodeScreenSpacePos(NodeId nodeId, ImVec2 screenSpacePos);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SetNodeEditorSpacePos")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SetNodeEditorSpacePos(NodeId nodeId, ImVec2 editorSpacePos);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SetNodeGridSpacePos")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SetNodeGridSpacePos(NodeId nodeId, ImVec2 gridSpacePos);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetNodeScreenSpacePos")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void GetNodeScreenSpacePos(ImVec2* ret, NodeId nodeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetNodeEditorSpacePos")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void GetNodeEditorSpacePos(ImVec2* ret, NodeId nodeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetNodeGridSpacePos")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void GetNodeGridSpacePos(ImVec2* ret, NodeId nodeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SnapNodeToGrid")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SnapNodeToGrid(NodeId nodeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsNodeHovered")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsNodeHovered(NodeId* nodeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_NumSelectedNodes")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial int NumSelectedNodes();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetSelectedNodes")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void GetSelectedNodes(NodeId* nodeIds);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_ClearNodeSelection_Nil")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void ClearNodeSelection();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_ClearNodeSelection_Int")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void ClearNodeSelection(NodeId nodeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SelectNode")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SelectNode(NodeId nodeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsNodeSelected")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsNodeSelected(NodeId nodeId);
            }
        }
    }
}
#endif
