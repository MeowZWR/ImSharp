namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    public static class Tester
    {
        private static readonly TestData Data = new();

        /// <summary> Show a simple test node editor. </summary>
        /// <param name="editorId"> The ID for the editor. </param>
        public static void Show(Utf8LabelHandler editorId)
        {
            using var idDisposable = Im.Id.Push(ref editorId);
            Im.Text("A -- Add node"u8);
            using (var editor = NodeEditor())
            {
                foreach (var node in Data.Nodes)
                    DrawNode(editor, node);

                foreach (var link in Data.Links)
                    editor.Link(link.Id, link.Start, link.End);

                AddNodeHandling(editor);
            }


            if (ImNodes.Link.LinkCreated(out var start, out var end, out _))
                Data.Links.Add(new Link(++Data.CurrentId, start, end));

            if (ImNodes.Link.LinkDestroyed(out var id))
                Data.Links.RemoveAll(l => l.Id == id);
        }

        private static void DrawNode(in NodeEditor editor, in Node nodeData)
        {
            using var node = editor.Node(nodeData.Id);
            using (node.TitleBar())
            {
                Im.Text("node"u8);
            }

            using (node.InputPin(nodeData.Id.Id << 8))
            {
                Im.Text("input"u8);
            }

            using (node.StaticAttribute(nodeData.Id.Id << 16))
            {
                Im.Item.SetNextWidthScaled(120);
                Im.Drag("value"u8, ref nodeData.Value, speed: 0.01f);
            }

            using (node.OutputPin(nodeData.Id.Id << 24))
            {
                var width = Im.Font.CalculateSize("output"u8).X;
                Im.Cursor.X += 120 * Im.Style.GlobalScale + Im.Font.CalculateSize("value"u8).X - width;
                Im.Text("output"u8);
            }
        }

        private static void AddNodeHandling(in NodeEditor editor)
        {
            if (!Im.Window.Focused(FocusedFlags.RootAndChildWindows) || !editor.Hovered)
                return;

            Im.GetIo().CaptureTextInput = true;
            if (!Im.Keyboard.IsReleased(Key.A))
                return;

            var       nodeId = ++Data.CurrentId;
            using var node   = editor.Node(nodeId);
            node.ScreenSpacePosition = Im.Mouse.Position;
            node.Draggable           = true;
            node.SnapToGrid();
            Data.Nodes.Add(new Node(nodeId, 0));
        }

        private record Node(NodeId Id, float Value)
        {
            public float Value = Value;
        }

        private record Link(LinkId Id, AttributeId Start, AttributeId End);

        private sealed class TestData
        {
            public readonly List<Node> Nodes = [];
            public readonly List<Link> Links = [];
            public          ImGuiId    CurrentId;
        }
    }
}
