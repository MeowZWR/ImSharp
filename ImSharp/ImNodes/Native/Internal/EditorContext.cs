#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public static partial class Internal
        {
            /// <summary> The internal context for an ImNodes editor. </summary>
            /// <remarks> Only obtained and used as a pointer since we do not provide internals for ImNodes. </remarks>
            public struct EditorContext
            { }
        }
    }
}
#endif
