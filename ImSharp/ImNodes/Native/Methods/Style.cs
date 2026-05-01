#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public static partial class Methods
        {
            public static unsafe partial class Style
            {
                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetIO")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial Io* GetIo();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetStyle")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial Native.Style* GetStyle();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_StyleColorsDark")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void StyleColorsDark(Native.Style* style);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_StyleColorsClassic")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void StyleColorsClassic(Native.Style* style);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_StyleColorsLight")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void StyleColorsLight(Native.Style* style);
            }
        }
    }
}
#endif
