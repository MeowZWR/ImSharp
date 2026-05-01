#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    /// <summary> Wrapper class for methods related to attributes. </summary>
    public static class Attribute
    {
        /// <inheritdoc cref="AttributeDisposable(AttributeId,PinShape,bool)"/>
        public static AttributeDisposable Input(AttributeId id, PinShape shape = PinShape.CircleFilled)
            => new(id, shape, true);

        /// <inheritdoc cref="AttributeDisposable(AttributeId,PinShape)"/>
        public static AttributeDisposable Output(AttributeId id, PinShape shape = PinShape.CircleFilled)
            => new(id, shape);

        /// <inheritdoc cref="AttributeDisposable(AttributeId)"/>
        public static AttributeDisposable Static(AttributeId id)
            => new(id);

        /// <inheritdoc cref="AttributeDisposable(AttributeId,bool)"/>
        public static AttributeDisposable Reference(AttributeId id)
            => new(id, true);

        /// <summary> Get whether any attribute is currently active. </summary>
        public static unsafe bool AnyActive
        {
            [MethodImpl(ImSharpConfiguration.Inl)]
            get => Native.Methods.Attribute.IsAnyAttributeActive(null);
        }

        /// <summary> Get whether the last drawn attribute is currently active. </summary>
        public static bool LastAttributeActive
        {
            [MethodImpl(ImSharpConfiguration.Inl)]
            get => Native.Methods.Attribute.IsAttributeActive();
        }

        /// <summary> Get whether any attribute pin is currently hovered. </summary>
        /// <remarks> Use after disposing the <seealso cref="NodeEditorDisposable"/>. </remarks>
        [MethodImpl(ImSharpConfiguration.Inl)]
        public static unsafe bool AnyPinHovered()
        {
            AttributeId id = 0;
            return Native.Methods.Attribute.IsPinHovered(&id);
        }

        /// <inheritdoc cref="AttributeFlagDisposable.Push(AttributeFlags)"/>
        [MethodImpl(ImSharpConfiguration.Inl)]
        public static AttributeFlagDisposable PushFlag(AttributeFlags flag)
            => new AttributeFlagDisposable().Push(flag);

        /// <inheritdoc cref="AttributeFlagDisposable.Push(AttributeFlags,bool)"/>
        [MethodImpl(ImSharpConfiguration.Inl)]
        public static AttributeFlagDisposable PushFlag(AttributeFlags flag, bool condition)
            => new AttributeFlagDisposable().Push(flag, condition);

        /// <summary> Pop a number of ImNodes attribute flags. </summary>
        /// <param name="num"> The number of attribute flags to pop. The number is not checked against the ImNodes attribute flag stack. </param>
        /// <remarks> Avoid using this function, and attribute flags across scopes, as much as possible. </remarks>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static void PopFlagUnsafe(int num = 1)
        {
            while (num-- > 0)
                Native.Methods.Stacks.PopAttribute();
        }
    }
}
#endif
