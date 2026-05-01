namespace ImSharp;

public static partial class ImEx
{
    /// <inheritdoc cref="ColorPicker(Utf8LabelHandler,Utf8TextHandler,in Vector4,out Vector4,in Vector4)"/>
    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static bool ColorPicker(Utf8LabelHandler label, Utf8TextHandler tooltip, Rgba32 currentColor, out Rgba32 newColor,
        Rgba32 defaultColor)
    {
        if (!ColorPicker(ref label, ref tooltip, currentColor.ToVector(), out var ret, defaultColor.ToVector()))
        {
            newColor = defaultColor;
            return false;
        }

        newColor = ret;
        return true;
    }

    /// <inheritdoc cref="ColorPicker(Utf8LabelHandler,Utf8TextHandler,in Vector4,out Vector4,in Vector4)"/>
    [MethodImpl(ImSharpConfiguration.OptInl)]
    [OverloadResolutionPriority(100)]
    public static bool ColorPicker(Utf8LabelHandler label, Utf8TextHandler tooltip, in Vector4 currentColor, out Vector4 newColor,
        in Vector4 defaultColor)
        => ColorPicker(ref label, ref tooltip, currentColor, out newColor, defaultColor);

    /// <summary> A default color picker with extended functionality. </summary>
    /// <param name="label"> The label and ID for the picker. Does not have to be null-terminated. </param>
    /// <param name="tooltip"> An optional tooltip when the label is hovered. Does not have to be null-terminated. </param>
    /// <param name="currentColor"> The current color to display. </param>
    /// <param name="newColor"> The newly picked color when true is returned. </param>
    /// <param name="defaultColor"> A default color for this picker that a button can reset to. </param>
    /// <returns> True if a new color was picked. </returns>
    public static bool ColorPicker(ref Utf8LabelHandler label, ref Utf8TextHandler tooltip, in Vector4 currentColor, out Vector4 newColor,
        in Vector4 defaultColor)
    {
        var       ret   = false;
        using var _     = Im.Id.Push(ref label);
        using var group = Im.Group();
        newColor = currentColor;
        // Draw the regular color picker with no label.
        ret |= Im.Color.Editor("##Picker"u8, ref newColor, ColorEditorFlags.AlphaPreviewHalf | ColorEditorFlags.NoInputs);

        // Draw a button to return to default.
        Im.Line.SameInner();
        if (Button("Default"u8, Vector2.Zero, StringU8.Empty, currentColor == defaultColor))
        {
            newColor = defaultColor;
            ret      = true;
        }

        var buttonHovered = Im.Item.Hovered();

        // Draw the actual label as well as a potential tooltip.
        Im.Line.SameInner();
        Im.Text(ref label);
        Im.Tooltip.OnHover(ref tooltip);

        // Draw the default tooltip.
        if (buttonHovered)
        {
            using var tt = Im.Tooltip.Begin();
            TextFrameAligned($"Reset this color to {(Rgba32)defaultColor}.");
            Im.Line.SameInner();
            var tmp = defaultColor;
            Im.Color.Editor(StringU8.Empty, ref tmp, ColorEditorFlags.AlphaPreviewHalf | ColorEditorFlags.NoInputs);
        }

        return ret;
    }

    /// <summary> Draw a button-only preview of an RGB color with an optional contrasted letter within the button. </summary>
    /// <param name="label"> The label and ID for the picker as text. If this is a UTF8 string, it HAS to be null-terminated. </param>
    /// <param name="tooltip"> An optional tooltip when the label is hovered. Does not have to be null-terminated. </param>
    /// <param name="input"> The input color. </param>
    /// <param name="output"> The output color. </param>
    /// <param name="letter"> The letter to draw inside the button. If this is whitespace, no letter is drawn. </param>
    /// <returns> True when the output color changed in this frame. </returns>
    public static bool ColorPickerButton(Utf8LabelHandler label, Utf8TextHandler tooltip, Vector3 input, out Vector3 output,
        char letter = ' ')
    {
        output = input;
        var ret = Im.Color.Editor(label, ref input,
            ColorEditorFlags.NoInputs
          | ColorEditorFlags.DisplayRgb
          | ColorEditorFlags.InputRgb
          | ColorEditorFlags.NoTooltip
          | ColorEditorFlags.Hdr);

        if (!char.IsWhiteSpace(letter) && Im.Item.Visible)
        {
            Span<byte> text   = stackalloc byte[8];
            var        length = Encoding.UTF8.GetBytes([letter], text);
            text[length] = 0;
            text         = text[..length];
            var textSize  = Im.Font.CalculateSize(text);
            var center    = Im.Item.UpperLeftCorner + (Im.Item.Size - textSize) / 2;
            var textColor = Rgba32.ContrastColor(new Vector4(output, 0.7f));
            Im.Window.DrawList.Text(center, textColor, text);
        }

        output = input;

        Im.Tooltip.OnHover(tooltip);
        return ret;
    }
}
