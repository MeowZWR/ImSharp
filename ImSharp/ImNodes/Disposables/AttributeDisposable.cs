#if IMNODES
namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    /// <summary> A wrapper around ImNodes attributes or pins. </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref struct AttributeDisposable : IDisposable
    {
        private enum AttributeType : byte
        {
            None,
            Input,
            Output,
            Static,
        }

        /// <summary> The unique ID of the attribute. </summary>
        public readonly AttributeId Id;

        private AttributeType _type;

        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static implicit operator AttributeId(AttributeDisposable attribute)
            => attribute.Id;

        /// <summary> Begin a new input attribute inside the current node. Input pins are rendered on the left side of the node. </summary>
        /// <param name="id"> The desired unique ID of the new input attribute. Can be any integer except for <seealso cref="int.MinValue"/>. </param>
        /// <param name="shape"> The shape of the pin rendered next to the attribute. Links are created between pins. </param>
        /// <returns> A disposable object that ends the input attribute on disposal. Use with using. </returns>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        internal AttributeDisposable(AttributeId id, PinShape shape, bool _)
        {
            Native.Methods.Attribute.BeginInputAttribute(id, shape);
            _type = AttributeType.Input;
            Id    = id;
        }

        /// <summary> Begin a new output attribute inside the current node. Output pins are rendered on the right side of the node. </summary>
        /// <param name="id"> The desired unique ID of the new output attribute. Can be any integer except for <seealso cref="int.MinValue"/>. </param>
        /// <param name="shape"> The shape of the pin rendered next to the attribute. Links are created between pins. </param>
        /// <returns> A disposable object that ends the output attribute on disposal. Use with using. </returns>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        internal AttributeDisposable(AttributeId id, PinShape shape)
        {
            Native.Methods.Attribute.BeginOutputAttribute(id, shape);
            _type = AttributeType.Output;
            Id    = id;
        }

        /// <summary> Begin a new static attribute inside the current node. Static attributes have no pin and can not be linked, but can be checked for activity. </summary>
        /// <param name="id"> The desired unique ID of the new static attribute. Can be any integer except for <seealso cref="int.MinValue"/>. </param>
        /// <returns> A disposable object that ends the static attribute on disposal. Use with using. </returns>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        internal AttributeDisposable(AttributeId id)
        {
            Native.Methods.Attribute.BeginStaticAttribute(id);
            _type = AttributeType.Static;
            Id    = id;
        }

        /// <summary> Create a reference to an existing attribute without beginning it. </summary>
        /// <param name="id"> The unique ID of the existing attribute. </param>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        internal AttributeDisposable(AttributeId id, bool _)
        {
            _type = AttributeType.None;
            Id    = id;
        }

        /// <summary> Get whether this attribute's pin is currently hovered by the mouse cursor. </summary>
        /// <remarks> Use after disposing the <seealso cref="NodeEditorDisposable"/>. </remarks>
        public readonly unsafe bool PinHovered
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                AttributeId id;
                if (!Native.Methods.Attribute.IsPinHovered(&id))
                    return false;

                return id == Id;
            }
        }

        /// <summary> Get whether this attribute is currently active. </summary>
        public readonly unsafe bool Active
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                AttributeId id;
                if (!Native.Methods.Attribute.IsAnyAttributeActive(&id))
                    return false;

                return id == Id;
            }
        }

        /// <summary> End the attribute on leaving scope. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void Dispose()
        {
            switch (_type)
            {
                case AttributeType.Input:  Native.Methods.Attribute.EndInputAttribute(); break;
                case AttributeType.Output: Native.Methods.Attribute.EndOutputAttribute(); break;
                case AttributeType.Static: Native.Methods.Attribute.EndStaticAttribute(); break;
            }

            _type = AttributeType.None;
        }
    }
}

#endif
