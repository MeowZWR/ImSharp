using Microsoft.Extensions.Logging;

namespace ImSharp;

public static unsafe class ImSharpPerFrame
{
    internal static Action? Update;
    private static  int     _frameRan = -1;

    public static void OnUpdate()
    {
        var context = Im.ImGuiContext.Get().Pointer;
        if (context is null || _frameRan == context->FrameCount)
            return;

        _frameRan         = context->FrameCount;
        Im.ContextPointer = context;
        Im.StylePointer   = Im.ImGuiStyle.Get().Pointer;
        Im.IoPointer      = Im.InputOutput.Get().Pointer;

        // Reset the temporary frame storage every frame.
        InputStringHandlerBuffer.FrameStorageString = StringU8.Null;
#if IMNODES
        ImNodes.ImNodes.StylePointer = ImNodes.ImNodes.ImNodesStyle.Get().Pointer;
        ImNodes.ImNodes.ContextPointer = ImNodes.ImNodes.ImNodesContext.Get().Pointer;
        ImNodes.ImNodes.IoPointer = ImNodes.ImNodes.InputOutput.Get().Pointer;
#endif
        if (Update is not null)
            foreach (var action in Delegate.EnumerateInvocationList(Update))
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    ImSharpConfiguration.Logger?.LogError(ex, "Failure invoking a delegate registered to frame updates.");
                }
            }
    }
}
