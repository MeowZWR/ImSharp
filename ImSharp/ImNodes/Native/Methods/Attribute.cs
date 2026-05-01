#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public static partial class Methods
        {
            public static unsafe partial class Attribute
            {
                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_BeginInputAttribute")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void BeginInputAttribute(AttributeId id, PinShape shape);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EndInputAttribute")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void EndInputAttribute();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_BeginOutputAttribute")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void BeginOutputAttribute(AttributeId id, PinShape shape);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EndOutputAttribute")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void EndOutputAttribute();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_BeginStaticAttribute")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void BeginStaticAttribute(AttributeId id);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_EndStaticAttribute")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void EndStaticAttribute();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsAttributeActive")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsAttributeActive();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsAnyAttributeActive")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsAnyAttributeActive(AttributeId* attributeId);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_IsPinHovered")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial ImBool IsPinHovered(AttributeId* attributeId);
            }
        }
    }
}
#endif
