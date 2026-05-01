using Microsoft.Extensions.Logging;

namespace ImSharp;

public static unsafe class ImSharpPerFrame
{
    internal static Action? Update;

    public static void OnUpdate()
    {
        Im.StylePointer   = Im.ImGuiStyle.Get().Pointer;
        Im.ContextPointer = Im.ImGuiContext.Get().Pointer;
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
