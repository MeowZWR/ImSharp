namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    /// <summary> A reference to input/output data for ImNodes. </summary>
    /// <param name="pointer"> The native pointer to the input/output data. </param>
    public readonly unsafe ref struct InputOutput(Internal.Io* pointer)
    {
        /// <summary> The address of the native object. </summary>
        public readonly Internal.Io* Pointer = pointer;

        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static implicit operator InputOutput(Internal.Io* pointer)
            => new(pointer);

        /// <summary> Get the reference to the current input/output data for ImNodes. </summary>
        [MethodImpl(ImSharpConfiguration.Inl)]
        public static InputOutput Get()
            => Api.GetIo();

        /// <summary> The panning speed when dragging an element while the mouse is outside the main editor view. </summary>
        public ref float AutoPanningSpeed
        {
            [MethodImpl(ImSharpConfiguration.Inl)]
            get => ref Pointer->AutoPanningSpeed;
        }

        /// <summary> Holding this button pans the node area. By default, the middle mouse button is used. </summary>
        public ref MouseButton AltMouseButton
        {
            [MethodImpl(ImSharpConfiguration.Inl)]
            get => ref Pointer->AltMouseButton;
        }

        /// <summary> Set the modifier such that left-clicking nodes with this modifier active will add the node to the current selection. By default, Control will be used. </summary>
        /// <param name="io"> A reference to ImGuis own Input/Output data. </param>
        /// <param name="modifier"> The modifier key to use. Should be a single flag value. </param>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void SetMultipleSelectModifier(Im.InputOutput io, ModFlags modifier)
            => Pointer->MultipleSelectModifier = GetPointer(io.Pointer, modifier);

        /// <summary> Set the modifier such that left-clicking links with this modifier active will detach them. By default, this is disabled. </summary>
        /// <param name="io"> A reference to ImGuis own Input/Output data. </param>
        /// <param name="modifier"> The modifier key to use. Should be a single flag value. </param>
        /// <remarks> The implementation has to actually delete the link for this to work. </remarks>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void SetLinkDetachWithModifier(Im.InputOutput io, ModFlags modifier)
            => Pointer->LinkDetachWithModifierClick = GetPointer(io.Pointer, modifier);

        /// <summary> Set the modifier such that left-clicking will pan the editor view. Disabled by default. </summary>
        /// <param name="io"> A reference to ImGuis own Input/Output data. </param>
        /// <param name="modifier"> The modifier key to use. Should be a single flag value. </param>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void SetEmulateThreeButtonMouseModifier(Im.InputOutput io, ModFlags modifier)
            => Pointer->EmulateThreeButtonMouse = GetPointer(io.Pointer, modifier);

        [MethodImpl(ImSharpConfiguration.OptInl)]
        private static ImBool* GetPointer(Im.Native.Io* io, ModFlags modifier)
            => modifier switch
            {
                ModFlags.Ctrl  => &io->KeyCtrl,
                ModFlags.Shift => &io->KeyShift,
                ModFlags.Alt   => &io->KeyAlt,
                ModFlags.Super => &io->KeySuper,
                _              => null,
            };
    }
}
