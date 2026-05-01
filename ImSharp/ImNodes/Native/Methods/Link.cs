#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public static partial class Methods
        {
            public static unsafe partial class Link
            {
                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_Link")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void CreateLink(ImNodes.Link id, AttributeId startAttributeId, AttributeId endAttributeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsLinkHovered")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsLinkHovered(ImNodes.Link* linkId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_NumSelectedLinks")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial int NumSelectedLinks();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetSelectedLinks")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void GetSelectedLinks(ImNodes.Link* linkIds);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_ClearLinkSelection_Nil")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void ClearLinkSelection();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_ClearLinkSelection_Int")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void ClearLinkSelection(ImNodes.Link link);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SelectLink")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SelectLink(ImNodes.Link link);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsLinkSelected")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsLinkSelected(ImNodes.Link link);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsLinkStarted")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsLinkStarted(AttributeId* startedAtAttributeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsLinkDropped")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsLinkDropped(AttributeId* startedAtAttributeId, ImBool includingDetachedLinks);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsLinkCreated_BoolPtr")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsLinkCreated(AttributeId* startedAtAttributeId, AttributeId* endedAtAttributeId,
                    ImBool* createdFromSnap);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsLinkCreated_IntPtr")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsLinkCreated(NodeId* startedAtNodeId, AttributeId* startedAtAttributeId, NodeId* endedAtNodeId,
                    AttributeId* endedAtAttributeId, ImBool* createdFromSnap);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsLinkDestroyed")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsLinkDestroyed(ImNodes.Link* linkId);
            }
        }
    }
}
#endif
