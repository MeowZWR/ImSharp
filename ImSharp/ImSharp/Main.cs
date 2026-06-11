namespace ImSharp;

public static partial class Im
{
    public static unsafe class Main
    {
        /// <summary> Tell ImGui to render the collected frame. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static void Render()
            => Native.Methods.Main.Render();

        /// <summary> Tell ImGui to start a new frame. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static void NewFrame()
            => Native.Methods.Main.NewFrame();

        /// <summary> Tell ImGui to end the current frame. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static void EndFrame()
            => Native.Methods.Main.EndFrame();
    }
}
