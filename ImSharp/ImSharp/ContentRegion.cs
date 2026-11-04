namespace ImSharp;

public static partial class Im
{
    /// <summary> Wrapper class for querying content region information. </summary>
    public static unsafe class ContentRegion
    {
        /// <summary> Get the remaining available content region of the window, child, or cell considering the current cursor position. </summary>
        public static Vector2 Available
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                ImVec2 ret;
                Native.Methods.ContentRegion.GetContentRegionAvail(&ret);
                return ret;
            }
        }

        /// <summary> Get the remaining available content width of the window, child, or cell considering the current cursor position, with Y set to a custom value. </summary>
        public static Vector2 Width(float y = 0)
            => Available with { Y = y };

        /// <summary> Get the remaining available content height of the window, child, or cell considering the current cursor position, with X set to a custom value. </summary>
        public static Vector2 Height(float x = 0)
            => Available with { X = x };

        /// <summary> Get the total available content region of the current window, child, or cell in window coordinates. </summary>
        public static Vector2 Maximum
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                ImVec2 ret;
                Native.Methods.ContentRegion.GetContentRegionMax(&ret);
                return ret;
            }
        }
    }
}
