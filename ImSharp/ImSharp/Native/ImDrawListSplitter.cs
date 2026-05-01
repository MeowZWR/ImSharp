namespace ImSharp;

public static partial class Im
{
    public static partial class Native
    {
        public struct ImDrawListSplitter
        {
            public int                     Current;
            public int                     Count;
            public ImVector<ImDrawChannel> Channels;
        };
    }
}
