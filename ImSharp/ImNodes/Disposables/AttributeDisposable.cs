namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    /// <summary> A wrapper around ImNodes attributes or pins. </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref struct AttributeDisposable : IDisposable
    {
        /// <summary> The unique ID of the attribute. </summary>
        public readonly AttributeId Id;

        /// <summary> The <see cref="Internal.AttributeType"/>> of the attribute. </summary>
        public Internal.AttributeType Type;

        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static implicit operator AttributeId(AttributeDisposable attribute)
            => attribute.Id;

        /// <summary> Begin a new attribute inside the current node. </summary>
        /// <param name="id"> The desired unique ID of the new input attribute. Can be any integer except for <seealso cref="int.MinValue"/>. </param>
        /// <param name="shape"> The shape of the pin rendered next to the attribute. Links are created between pins. </param>
        /// <param name="type"> The type of the attribute. </param>
        /// <returns> A disposable object that ends the input attribute on disposal. Use with using. </returns>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        internal AttributeDisposable(AttributeId id, PinShape shape, Internal.AttributeType type)
        {
            Type = type;
            Id   = id;

            switch (type)
            {
                case Internal.AttributeType.Input:  Api.BeginInputAttribute(id, shape); break;
                case Internal.AttributeType.Output: Api.BeginOutputAttribute(id, shape); break;
                case Internal.AttributeType.Static: Api.BeginStaticAttribute(id); break;
            }
        }

        /// <summary> Get whether this attribute's pin is currently hovered by the mouse cursor. </summary>
        /// <remarks> Use after disposing the <seealso cref="NodeEditorDisposable"/>. </remarks>
        public unsafe bool PinHovered
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                AttributeId id;
                if (!Api.IsPinHovered(&id))
                    return false;

                return id == Id;
            }
        }

        /// <summary> Get whether this attribute is currently active. </summary>
        public unsafe bool Active
        {
            [MethodImpl(ImSharpConfiguration.OptInl)]
            get
            {
                AttributeId id;
                if (!Api.IsAnyAttributeActive(&id))
                    return false;

                return id == Id;
            }
        }

        /// <summary> End the attribute on leaving scope. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void Dispose()
        {
            switch (Type)
            {
                case Internal.AttributeType.Input:  Api.EndInputAttribute(); break;
                case Internal.AttributeType.Output: Api.EndOutputAttribute(); break;
                case Internal.AttributeType.Static: Api.EndStaticAttribute(); break;
            }

            Type = Internal.AttributeType.None;
        }
    }
}
