// ReSharper disable MethodOverloadWithOptionalParameter

namespace ImSharp;

public static partial class ImEx
{
    /// <summary> A wrapper around functions using a specific font for icons. </summary>
    public static class Icon
    {
        /// <inheritdoc cref="Draw{T}(T,Rgba32)"/>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static void Draw<T>(T icon) where T : IIconStandIn
        {
            using var _ = T.Font.Push();
            Im.Text(icon.Span);
        }

        /// <summary> Draw a stand-alone icon as text. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="textColor"> The color of the icon. Uses text color if 0. </param>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static void Draw<T>(T icon, Rgba32 textColor) where T : IIconStandIn
        {
            using var _ = T.Font.Push();
            Im.Text(icon.Span, textColor);
        }

        /// <inheritdoc cref="DrawAligned{T}(T,Rgba32)"/>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static void DrawAligned<T>(T icon) where T : IIconStandIn
        {
            Im.Cursor.FrameAlign();
            using var _ = T.Font.Push();
            Im.Text(icon.Span);
        }

        /// <summary> Draw a stand-alone icon as text aligned to the frame offset. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="textColor"> The color of the icon. Uses text color if 0. </param>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static void DrawAligned<T>(T icon, Rgba32 textColor) where T : IIconStandIn
        {
            Im.Cursor.FrameAlign();
            using var _ = T.Font.Push();
            Im.Text(icon.Span, textColor);
        }

        /// <summary> Calculate the size of a stand-alone icon. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <returns> The size of the icon. </returns>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static Vector2 CalculateSize<T>(T icon) where T : IIconStandIn
        {
            using var _ = T.Font.Push();
            return Im.Font.CalculateSize(icon.Span, false);
        }

        /// <summary> Draw a button with the given icon as label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="tooltip"> A tooltip shown when hovering the button regardless of whether it is disabled or not as text. Does not have to be null-terminated. </param>
        /// <param name="config"> Additional parameters to configure the design and behavior of the button. </param>
        /// <returns> True if the button has been clicked in this frame. </returns>
        /// <remarks> The tooltip is always evaluated. If this is expensive, prefer leaving it empty and using <seealso cref="Im.Tooltip.OnHover(HoveredFlags,ref HoverUtf8StringHandler,bool,Im.Font)"/> manually. </remarks>
        [OverloadResolutionPriority(20)]
        public static bool Button<T>(T icon, Utf8TextHandler tooltip = default, in ButtonConfiguration config = default) where T : IIconStandIn
        {
            var size = new Vector2(config.Size.X is 0 ? Im.Style.FrameHeight : config.Size.X,
                config.Size.Y is 0 ? Im.Style.FrameHeight : config.Size.Y);
            using var _ = Im.Disabled(config.Disabled);
            bool      ret;
            using (T.Font.Push())
            {
                using var style = config.PushColorStyle();

                ret = Im.Button(icon.Span, size, config.Flags);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span);
            return ret;
        }


        /// <summary> Draw a button with the given icon as label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="config"> Additional parameters to configure the design and behavior of the button. </param>
        /// <returns> True if the button has been clicked in this frame. </returns>
        [OverloadResolutionPriority(25)]
        public static bool Button<T>(T icon, in ButtonConfiguration config = default) where T : IIconStandIn
        {
            var size = new Vector2(config.Size.X is 0 ? Im.Style.FrameHeight : config.Size.X,
                config.Size.Y is 0 ? Im.Style.FrameHeight : config.Size.Y);

            using var style = config.PushColorStyle();
            using var _     = Im.Disabled(config.Disabled);
            using var font  = T.Font.Push();
            return Im.Button(icon.Span, size, config.Flags);
        }

        /// <summary> Draw a button with the given icon as label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="size"> The desired size for the button. If (0, 0), it will be frame size. </param>
        /// <param name="tooltip"> A tooltip shown when hovering the button regardless of whether it is disabled or not as text. Does not have to be null-terminated. </param>
        /// <param name="disabled"> Whether the button should be disabled or not. </param>
        /// <param name="buttonColor"> The color of the button's background. </param>
        /// <param name="textColor"> The color of the button's label. </param>
        /// <param name="flags"> Additional flags to control the button's behaviour. </param>
        /// <returns> True if the button has been clicked in this frame. </returns>
        /// <remarks> The tooltip is always evaluated. If this is expensive, prefer leaving it empty and using <seealso cref="Im.Tooltip.OnHover(HoveredFlags,ref HoverUtf8StringHandler,bool,Im.Font)"/> manually. </remarks>
        [OverloadResolutionPriority(50)]
        public static bool Button<T>(T icon, Utf8TextHandler tooltip = default, bool disabled = false,
            ColorParameter buttonColor = default, ColorParameter textColor = default, Vector2 size = default,
            ButtonFlags flags = ButtonFlags.None)
            where T : IIconStandIn
        {
            if (size.X is 0)
                size.X = Im.Style.FrameHeight;
            if (size.Y is 0)
                size.Y = Im.Style.FrameHeight;
            bool ret;
            using (T.Font.Push())
            {
                using var _ = Im.Disabled(disabled);
                using var color = Im.Color.Push(ImGuiColor.Button, buttonColor)
                    .Push(ImGuiColor.Text, textColor);
                ret = Im.Button(icon.Span, size, flags);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }


        /// <inheritdoc cref="Button{T}(T,Utf8TextHandler,bool,ColorParameter,ColorParameter,Vector2,ButtonFlags)"/>
        [OverloadResolutionPriority(100)]
        public static bool Button<T>(T icon, Utf8TextHandler tooltip = default, bool disabled = false,
            Vector2 size = default, ButtonFlags flags = ButtonFlags.None) where T : IIconStandIn
        {
            if (size.X is 0)
                size.X = Im.Style.FrameHeight;
            if (size.Y is 0)
                size.Y = Im.Style.FrameHeight;
            bool ret;
            using (T.Font.Push())
            {
                using var _ = Im.Disabled(disabled);
                ret = Im.Button(icon.Span, size, flags);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <inheritdoc cref="Button{T}(T,Utf8TextHandler,bool,ColorParameter,ColorParameter,Vector2,ButtonFlags)"/>
        [OverloadResolutionPriority(200)]
        public static bool Button<T>(T icon, Utf8TextHandler tooltip = default,
            Vector2 size = default, ButtonFlags flags = ButtonFlags.None) where T : IIconStandIn
        {
            if (size.X is 0)
                size.X = Im.Style.FrameHeight;
            if (size.Y is 0)
                size.Y = Im.Style.FrameHeight;
            bool ret;
            using (T.Font.Push())
            {
                ret = Im.Button(icon.Span, size, flags);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <summary> Draw a button with the given icon as label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="size"> The desired size for the button. If (0, 0), it will be frame size. </param>
        /// <param name="disabled"> Whether the button should be disabled or not. </param>
        /// <param name="flags"> Additional flags to control the button's behaviour. </param>
        /// <returns> True if the button has been clicked in this frame. </returns>
        [OverloadResolutionPriority(300)]
        public static bool Button<T>(T icon, bool disabled = false, Vector2 size = default, ButtonFlags flags = ButtonFlags.None)
            where T : IIconStandIn
        {
            if (size.X is 0)
                size.X = Im.Style.FrameHeight;
            if (size.Y is 0)
                size.Y = Im.Style.FrameHeight;
            using var _    = Im.Disabled(disabled);
            using var font = T.Font.Push();
            return Im.Button(icon.Span, size, flags);
        }

        /// <inheritdoc cref="Button{T}(T,bool,Vector2,ButtonFlags)"/>
        [OverloadResolutionPriority(400)]
        public static bool Button<T>(T icon, Vector2 size = default, ButtonFlags flags = ButtonFlags.None) where T : IIconStandIn
        {
            if (size.X is 0)
                size.X = Im.Style.FrameHeight;
            if (size.Y is 0)
                size.Y = Im.Style.FrameHeight;
            using var font = T.Font.Push();
            return Im.Button(icon.Span, size, flags);
        }

        /// <summary> Draw a button with the given icon and label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="label"> The label. </param>
        /// <param name="tooltip"> A tooltip shown when hovering the button regardless of whether it is disabled or not as text. Does not have to be null-terminated. </param>
        /// <param name="config"> Additional parameters to configure the design and behavior of the button. </param>
        /// <param name="corners"> Flags to control which corners of the button should follow the frame rounding style. </param>
        /// <param name="iconPosition"> Where to display the icon. </param>
        /// <returns> True if the button has been clicked in this frame. </returns>
        /// <remarks> The tooltip is always evaluated. If this is expensive, prefer leaving it empty and using <seealso cref="Im.Tooltip.OnHover(HoveredFlags,ref HoverUtf8StringHandler,bool,Im.Font)"/> manually. </remarks>
        [OverloadResolutionPriority(18)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip = default,
            in ButtonConfiguration config = default, Corners corners = Corners.Default,
            IconPosition iconPosition = IconPosition.BeforeLabel) where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            var size       = config.Size;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);

            bool ret;
            using (Im.Disabled(config.Disabled))
            {
                using var style = config.PushColorStyle();
                using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
                {
                    ret = ButtonCorners(label, size, config.Flags, corners);
                }

                DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,Utf8TextHandler,in ButtonConfiguration,Corners,IconPosition)"/>
        [OverloadResolutionPriority(20)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip = default,
            in ButtonConfiguration config = default, IconPosition iconPosition = IconPosition.BeforeLabel) where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            var size       = config.Size;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);

            bool ret;
            using (Im.Disabled(config.Disabled))
            {
                using var style = config.PushColorStyle();
                using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
                {
                    ret = Im.Button(label, size, config.Flags);
                }

                DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <summary> Draw a button with the given icon and label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="label"> The label. </param>
        /// <param name="config"> Additional parameters to configure the design and behavior of the button. </param>
        /// <param name="corners"> Flags to control which corners of the button should follow the frame rounding style. </param>
        /// <param name="iconPosition"> Where to display the icon. </param>
        /// <returns> True if the button has been clicked in this frame. </returns>
        [OverloadResolutionPriority(23)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, in ButtonConfiguration config = default,
            Corners corners = Corners.Default, IconPosition iconPosition = IconPosition.BeforeLabel) where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            var size       = config.Size;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);

            using var style = config.PushColorStyle();
            using var _     = Im.Disabled(config.Disabled);
            bool      ret;
            using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
            {
                ret = ButtonCorners(label, size, config.Flags, corners);
            }

            DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,in ButtonConfiguration,Corners,IconPosition)"/>
        [OverloadResolutionPriority(25)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, in ButtonConfiguration config = default,
            IconPosition iconPosition = IconPosition.BeforeLabel) where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            var size       = config.Size;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);

            using var style = config.PushColorStyle();
            using var _     = Im.Disabled(config.Disabled);
            bool      ret;
            using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
            {
                ret = Im.Button(label, size, config.Flags);
            }

            DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            return ret;
        }

        /// <summary> Draw a button with the given icon and label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="label"> The label. </param>
        /// <param name="size"> The desired size for the button. If (0, 0), it will be calculated automatically. </param>
        /// <param name="tooltip"> A tooltip shown when hovering the button regardless of whether it is disabled or not as text. Does not have to be null-terminated. </param>
        /// <param name="disabled"> Whether the button should be disabled or not. </param>
        /// <param name="buttonColor"> The color of the button's background. </param>
        /// <param name="textColor"> The color of the button's label. </param>
        /// <param name="flags"> Additional flags to control the button's behaviour. </param>
        /// <param name="corners"> Flags to control which corners of the button should follow the frame rounding style. </param>
        /// <param name="iconPosition"> Where to display the icon. </param>
        /// <returns> True if the button has been clicked in this frame. </returns>
        /// <remarks> The tooltip is always evaluated. If this is expensive, prefer leaving it empty and using <seealso cref="Im.Tooltip.OnHover(HoveredFlags,ref HoverUtf8StringHandler,bool,Im.Font)"/> manually. </remarks>
        [OverloadResolutionPriority(48)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip = default, bool disabled = false,
            ColorParameter buttonColor = default, ColorParameter textColor = default, Vector2 size = default,
            ButtonFlags flags = ButtonFlags.None, Corners corners = Corners.Default, IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            bool ret;
            using (Im.Disabled(disabled))
            {
                using var color = Im.Color.Push(ImGuiColor.Button, buttonColor)
                    .Push(ImGuiColor.Text, textColor);
                using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
                {
                    ret = ButtonCorners(label, size, flags, corners);
                }

                DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,Utf8TextHandler,bool,ColorParameter,ColorParameter,Vector2,ButtonFlags,Corners,IconPosition)"/>
        [OverloadResolutionPriority(50)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip = default, bool disabled = false,
            ColorParameter buttonColor = default, ColorParameter textColor = default, Vector2 size = default,
            ButtonFlags flags = ButtonFlags.None, IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            bool ret;
            using (Im.Disabled(disabled))
            {
                using var color = Im.Color.Push(ImGuiColor.Button, buttonColor)
                    .Push(ImGuiColor.Text, textColor);
                using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
                {
                    ret = Im.Button(label, size, flags);
                }

                DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,Utf8TextHandler,bool,ColorParameter,ColorParameter,Vector2,ButtonFlags,Corners,IconPosition)"/>
        [OverloadResolutionPriority(98)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip = default, bool disabled = false,
            Vector2 size = default, ButtonFlags flags = ButtonFlags.None, Corners corners = Corners.Default,
            IconPosition iconPosition = IconPosition.BeforeLabel) where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            bool ret;
            using (Im.Disabled(disabled))
            {
                using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
                {
                    ret = ButtonCorners(label, size, flags, corners);
                }

                DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,Utf8TextHandler,bool,ColorParameter,ColorParameter,Vector2,ButtonFlags,Corners,IconPosition)"/>
        [OverloadResolutionPriority(100)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip = default, bool disabled = false,
            Vector2 size = default, ButtonFlags flags = ButtonFlags.None, IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            bool ret;
            using (Im.Disabled(disabled))
            {
                using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
                {
                    ret = Im.Button(label, size, flags);
                }

                DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            }

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,Utf8TextHandler,bool,ColorParameter,ColorParameter,Vector2,ButtonFlags,Corners,IconPosition)"/>
        [OverloadResolutionPriority(198)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip = default, Vector2 size = default,
            ButtonFlags flags = ButtonFlags.None, Corners corners = Corners.Default, IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            bool ret;
            using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
            {
                ret = ButtonCorners(label, size, flags, corners);
            }

            DrawLabeledButtonIcon(icon, labelWidth, iconPosition);

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,Utf8TextHandler,bool,ColorParameter,ColorParameter,Vector2,ButtonFlags,Corners,IconPosition)"/>
        [OverloadResolutionPriority(200)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip = default, Vector2 size = default,
            ButtonFlags flags = ButtonFlags.None, IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            bool ret;
            using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
            {
                ret = Im.Button(label, size, flags);
            }

            DrawLabeledButtonIcon(icon, labelWidth, iconPosition);

            if (tooltip.GetSpan(out var span))
                Im.Tooltip.OnHover(HoveredFlags.AllowWhenDisabled, span, true);
            return ret;
        }

        /// <summary> Draw a button with the given icon and label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="label"> The label. </param>
        /// <param name="size"> The desired size for the button. If (0, 0), it will be frame size. </param>
        /// <param name="disabled"> Whether the button should be disabled or not. </param>
        /// <param name="flags"> Additional flags to control the button's behaviour. </param>
        /// <param name="corners"> Flags to control which corners of the button should follow the frame rounding style. </param>
        /// <param name="iconPosition"> Where to display the icon. </param>
        /// <returns> True if the button has been clicked in this frame. </returns>
        [OverloadResolutionPriority(298)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, bool disabled = false, Vector2 size = default,
            ButtonFlags flags = ButtonFlags.None, Corners corners = Corners.Default, IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            using var _ = Im.Disabled(disabled);
            bool      ret;
            using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
            {
                ret = ButtonCorners(label, size, flags, corners);
            }

            DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,bool,Vector2,ButtonFlags,Corners,IconPosition)"/>
        [OverloadResolutionPriority(300)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, bool disabled = false, Vector2 size = default,
            ButtonFlags flags = ButtonFlags.None, IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            using var _ = Im.Disabled(disabled);
            bool      ret;
            using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
            {
                ret = Im.Button(label, size, flags);
            }

            DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,bool,Vector2,ButtonFlags,Corners,IconPosition)"/>
        [OverloadResolutionPriority(398)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Vector2 size = default, ButtonFlags flags = ButtonFlags.None,
            Corners corners = Corners.Default, IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            bool ret;
            using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
            {
                ret = ButtonCorners(label, size, flags, corners);
            }

            DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            return ret;
        }

        /// <inheritdoc cref="LabeledButton{T}(T,Utf8LabelHandler,bool,Vector2,ButtonFlags,Corners,IconPosition)"/>
        [OverloadResolutionPriority(400)]
        public static bool LabeledButton<T>(T icon, Utf8LabelHandler label, Vector2 size = default, ButtonFlags flags = ButtonFlags.None,
            IconPosition iconPosition = IconPosition.BeforeLabel)
            where T : IIconStandIn
        {
            var labelWidth = Im.Font.CalculateSize(ref label).X;
            HandleLabeledButtonSizeDefaults(ref size, icon, labelWidth);
            bool ret;
            using (PushButtonLabelAlign(icon, size.X, labelWidth, iconPosition))
            {
                ret = Im.Button(label, size, flags);
            }

            DrawLabeledButtonIcon(icon, labelWidth, iconPosition);
            return ret;
        }

        /// <summary> Calculate the default size of a button with the given icon and label. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="label"> The label. </param>
        /// <returns> The size the button would take by default. </returns>
        public static Vector2 CalculateLabeledButtonSize<T>(T icon, Utf8LabelHandler label) where T : IIconStandIn
            => new(CalculateLabeledButtonWidth(icon, Im.Font.CalculateSize(ref label).X), Im.Style.FrameHeight);

        [MethodImpl(ImSharpConfiguration.Inl)]
        private static float CalculateLabeledButtonWidth<T>(T icon, float labelWidth) where T : IIconStandIn
            => Im.Style.FramePadding.X * 2.0f
              + (icon.IsEmpty ? 0.0f : Im.Style.TextHeight)
              + (labelWidth is 0.0f ? 0.0f : Im.Style.ItemInnerSpacing.X + labelWidth);

        [MethodImpl(ImSharpConfiguration.Inl)]
        private static void HandleLabeledButtonSizeDefaults<T>(ref Vector2 size, T icon, float labelWidth) where T : IIconStandIn
        {
            if (size.X is 0)
                size.X = CalculateLabeledButtonWidth(icon, labelWidth);
            if (size.Y is 0)
                size.Y = Im.Style.FrameHeight;
        }

        private static Im.StyleDisposable PushButtonLabelAlign<T>(T icon, float width, float labelWidth, IconPosition iconPosition)
            where T : IIconStandIn
        {
            if (labelWidth is 0.0f || icon.IsEmpty)
                return new Im.StyleDisposable();

            width -= 2.0f * Im.Style.FramePadding.X;
            var leeway = width - labelWidth;
            if (leeway is 0.0f)
                return new Im.StyleDisposable();

            var iconReserve = Im.Style.TextHeight + Im.Style.ItemInnerSpacing.X;
            var position    = (leeway - iconReserve) * Im.Style.ButtonTextAlignment.X;
            if (iconPosition is IconPosition.BeforeLabel or IconPosition.Start)
                position += iconReserve;

            return ImStyleDouble.ButtonTextAlign.PushX(position / leeway);
        }

        private static void DrawLabeledButtonIcon<T>(T icon, float labelWidth, IconPosition iconPosition) where T : IIconStandIn
        {
            if (icon.IsEmpty)
                return;

            using var font       = T.Font.Push();
            var       upperLeft  = Im.Item.UpperLeftCorner + Im.Style.FramePadding;
            var       lowerRight = Im.Item.LowerRightCorner - Im.Style.FramePadding;
            if (labelWidth is not 0.0f)
            {
                if (iconPosition is IconPosition.AfterLabel or IconPosition.End)
                    upperLeft.X += labelWidth + Im.Style.ItemInnerSpacing.X;
                else
                    lowerRight.X -= labelWidth + Im.Style.ItemInnerSpacing.X;
            }

            var alignment = iconPosition switch
            {
                IconPosition.Start => 0.0f,
                IconPosition.End   => 1.0f,
                _                  => Im.Style.ButtonTextAlignment.X,
            };
            Im.DrawList.Window.Text(
                Vector2.Lerp(upperLeft, lowerRight - Im.Font.CalculateSize(icon.Span), Im.Style.ButtonTextAlignment with { X = alignment }),
                ImGuiColor.Text.Get(), icon.Span);
        }

        /// <summary> Draw an icon with a label and a tooltip when hovering either of them. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="label"> The label as text. Does not have to be null-terminated. </param>
        /// <param name="tooltip"> The tooltip as text. Does not have to be null-terminated. </param>
        /// <param name="iconColor"> The color for the icon. </param>
        /// <param name="hovered"> Force the tooltip on when this is true. </param>
        public static void Labeled<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip, Rgba32 iconColor, bool hovered = false)
            where T : IIconStandIn
        {
            Draw(icon, iconColor);
            hovered = hovered || Im.Item.Hovered();
            Im.Line.SameInner();
            Im.Text(ref label);
            if (hovered || Im.Item.Hovered())
                Im.Tooltip.Set(tooltip);
        }

        /// <summary> Draw an icon and a label in the same line as the last item and a tooltip when hovering either of them or the last item. </summary>
        /// <typeparam name="T"> The icon type. </typeparam>
        /// <param name="icon"> The icon. </param>
        /// <param name="label"> The label as text. Does not have to be null-terminated. </param>
        /// <param name="tooltip"> The tooltip as text. Does not have to be null-terminated. </param>
        /// <param name="iconColor"> The color for the icon. </param>
        public static void Label<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip, Rgba32 iconColor) where T : IIconStandIn
        {
            Im.Line.SameInner();
            var hovered = Im.Item.Hovered();
            Draw(icon, iconColor);
            hovered = hovered || Im.Item.Hovered();
            Im.Line.SameInner();
            Im.Text(ref label);
            if (hovered || Im.Item.Hovered())
                Im.Tooltip.Set(tooltip);
        }

        /// <inheritdoc cref="Labeled{T}(T,Utf8LabelHandler,Utf8TextHandler,Rgba32,bool)"/>
        public static void Labeled<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip, bool hovered = false) where T : IIconStandIn
        {
            Draw(icon);
            hovered = hovered || Im.Item.Hovered();
            Im.Line.SameInner();
            Im.Text(ref label);
            if (hovered || Im.Item.Hovered())
                Im.Tooltip.Set(tooltip);
        }

        /// <inheritdoc cref="Label{T}(T,Utf8LabelHandler,Utf8TextHandler,Rgba32)"/>
        public static void Label<T>(T icon, Utf8LabelHandler label, Utf8TextHandler tooltip) where T : IIconStandIn
        {
            Im.Line.SameInner();
            var hovered = Im.Item.Hovered();
            Draw(icon);
            hovered = hovered || Im.Item.Hovered();
            Im.Line.SameInner();
            Im.Text(ref label);
            if (hovered || Im.Item.Hovered())
                Im.Tooltip.Set(tooltip);
        }

        /// <summary> Positioning behaviours for the icon in a labeled icon button. </summary>
        public enum IconPosition
        {
            /// <summary> Position the icon just before the label. </summary>
            BeforeLabel = 0,

            /// <summary> Position the icon just after the label. </summary>
            AfterLabel = 1,

            /// <summary> Position the icon at the start of the button, independently of <see cref="ImStyleDouble.ButtonTextAlign"/>. </summary>
            Start = 2,

            /// <summary> Position the icon at the end of the button, independently of <see cref="ImStyleDouble.ButtonTextAlign"/>. </summary>
            End = 3,
        }
    }
}
