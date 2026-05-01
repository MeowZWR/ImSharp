#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public static partial class Methods
        {
            public static partial class Stacks
            {
                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_PushColorStyle")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void PushColorStyle(ImNodesColor style, Rgba32 color);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_PopColorStyle")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void PopColorStyle();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_PushStyleVar_Float")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void PushStyle(ImNodesStyleSingle imNodesStyle, float value);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_PushStyleVar_Vec2")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void PushStyle(ImNodesStyleDouble imNodesStyle, ImVec2 value);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_PopStyleVar")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void PopStyle(int count);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_PushAttributeFlag")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void PushAttribute(AttributeFlags flag);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_PopAttributeFlag")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void PopAttribute();
            }
        }
    }
}
#endif
