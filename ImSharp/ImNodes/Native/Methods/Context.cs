#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public static partial class Methods
        {
            public static unsafe partial class Context
            {
                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SetImGuiContext")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SetImGuiContext(Im.Native.Internal.Context* context);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_CreateContext")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial Internal.Context* CreateContext();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_DestroyContext")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void DestroyContext(Internal.Context* context);

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_GetCurrentContext")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial Internal.Context* GetCurrentContext();

                [LibraryImport(Im.Version.CImNodesLibrary, EntryPoint = "imnodes_SetCurrentContext")]
                [MethodImpl(ImSharpConfiguration.Inl)]
                public static partial void SetCurrentContext(Internal.Context* context);
            }
        }
    }
}
#endif
