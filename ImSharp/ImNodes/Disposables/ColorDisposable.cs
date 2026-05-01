#if IMNODES

namespace ImSharp.ImNodes;

public static partial class ImNodes
{
    /// <summary> A wrapper around ImNodes color pushing. </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref struct ColorDisposable : IDisposable
    {
        /// <summary> The number of ImNodes colors currently pushed using this disposable. </summary>
        public int Count { get; private set; }

        /// <summary> Push a color to the ImNodes color stack. </summary>
        /// <param name="type"> The type of ImNodes color to change. </param>
        /// <param name="color"> The color to change it to. </param>
        /// <param name="condition"> If this is false, the color is not pushed. </param>
        /// <returns> A disposable object that can be used to push further colors and pops those colors after leaving scope. Use with using. </returns>
        /// <remarks> If you need to keep colors pushed longer than the current scope, use without using and use <seealso cref="PopUnsafe"/>. </remarks>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public ColorDisposable Push(ImNodesColor type, Rgba32 color, bool condition)
            => condition ? Push(type, color) : this;

        /// <inheritdoc cref="Push(ImNodesColor,Rgba32,bool)"/>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public ColorDisposable Push(ImNodesColor type, Rgba32 color)
        {
            Native.Methods.Stacks.PushColorStyle(type, color.Color);
            ++Count;
            return this;
        }

        // TODO references
        /// <summary> Push a color to the ImNodes color stack. </summary>
        /// <param name="type"> The type of ImNodes color to change. </param>
        /// <param name="color"> The color to change it to. If this is null, no color will be set. </param>
        /// <returns> A disposable object that can be used to push further colors and pops those colors after leaving scope. Use with using. </returns>
        /// <remarks> If you need to keep colors pushed longer than the current scope, use without using and use <seealso cref="PopUnsafe"/>. </remarks>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public ColorDisposable Push(ImNodesColor type, Rgba32? color)
            => color.HasValue ? Push(type, color.Value) : this;

        /// <summary> Pop a number of colors. </summary>
        /// <param name="num"> The number of colors to pop. This is clamped to the number of colors pushed by this object. </param>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void Pop(int num = 1)
        {
            num   =  Math.Min(num, Count);
            Count -= num;
            while (num-- > 0)
                Native.Methods.Stacks.PopColorStyle();
        }

        /// <summary> Pop all pushed colors. </summary>
        [MethodImpl(ImSharpConfiguration.OptInl)]
        public void Dispose()
            => Pop(Count);
    }
}
#endif
