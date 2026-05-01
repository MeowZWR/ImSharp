#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static partial class Native
    {
        public unsafe struct Io
        {
            public ImBool*     EmulateThreeButtonMouse;
            public ImBool*     LinkDetachWithModifierClick;
            public ImBool*     MultipleSelectModifier;
            public MouseButton AltMouseButton;
            public float       AutoPanningSpeed;
        }
    }
}
#endif
