#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public static partial class Methods
        {
            public static unsafe partial class Editor
            {
                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EditorContextCreate")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial Internal.EditorContext* Create();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EditorContextFree")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void Free(Internal.EditorContext* context);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EditorContextSet")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void Set(Internal.EditorContext* context);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EditorContextGetPanning")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void GetPanning(ImVec2* ret);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EditorContextResetPanning")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void ResetPanning(ImVec2 position);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EditorContextMoveToNode")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void MoveToNode(NodeId nodeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_BeginNodeEditor")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void BeginNodeEditor();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EndNodeEditor")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void EndNodeEditor();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_MiniMap")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void MiniMap(float sizeFraction, MiniMapLocation location,
                    delegate* unmanaged<NodeId, nint, void> callback, nint userData);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsEditorHovered")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsEditorHovered();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SaveCurrentEditorStateToIniString")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial byte* SaveCurrentEditorStateToIniString(ulong* dataSize);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SaveEditorStateToIniString")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial byte* SaveEditorStateToIniString(Internal.EditorContext* editor, ulong* dataSize);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_LoadCurrentEditorStateFromIniString")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void LoadCurrentEditorStateFromIniString(byte* data, ulong size);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_LoadEditorStateFromIniString")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void LoadEditorStateFromIniString(Internal.EditorContext* editor, byte* data, ulong size);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SaveCurrentEditorStateToIniFile")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SaveCurrentEditorStateToIniFile(byte* fileName);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SaveEditorStateToIniFile")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SaveEditorStateToIniFile(Internal.EditorContext* editor, byte* fileName);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_LoadCurrentEditorStateFromIniFile")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void LoadCurrentEditorStateFromIniFile(byte* fileName);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_LoadEditorStateFromIniFile")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void LoadEditorStateFromIniFile(Internal.EditorContext* editor, byte* fileName);
            }
        }
    }
}
#endif
