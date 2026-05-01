#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public struct Style
        {
            public float             GridSpacing;
            public float             NodeCornerRounding;
            public ImVec2            NodePadding;
            public float             NodeBorderThickness;
            public float             LinkThickness;
            public float             LinkLineSegmentsPerLength;
            public float             LinkHoverDistance;
            public float             PinCircleRadius;
            public float             PinQuadSideLength;
            public float             PinTriangleSideLength;
            public float             PinLineThickness;
            public float             PinHoverRadius;
            public float             PinOffset;
            public ImVec2            MiniMapPadding;
            public ImVec2            MiniMapOffset;
            public ImNodesStyleFlags Flags;
            public ColorArray        Colors;

            [InlineArray(ImNodesColorExtensions.NumColors)]
            public struct ColorArray
            {
                private Rgba32 _element0;
            }
        }
    }
}
#endif
