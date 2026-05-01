#if IMNODES
namespace ImSharp.ImNodes;

/// <summary> The internally used ID type for ImNodes nodes. </summary>
public readonly record struct NodeId(int Id) : IAdditionOperators<NodeId, int, NodeId>, ISubtractionOperators<NodeId, int, NodeId>,
    IIncrementOperators<NodeId>, IDecrementOperators<NodeId>, ISpanFormattable, IUtf8SpanFormattable
{
    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static implicit operator NodeId(uint v)
        => new((int)v);

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static implicit operator NodeId(int v)
        => new(v);

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static explicit operator uint(NodeId v)
        => (uint)v.Id;

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static explicit operator int(NodeId v)
        => v.Id;

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static NodeId operator ++(NodeId id)
        => new(id.Id + 1);

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static NodeId operator --(NodeId id)
        => new(id.Id - 1);

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static NodeId operator +(NodeId id, int offset)
        => new(id.Id + offset);

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static NodeId operator -(NodeId id, int offset)
        => new(id.Id - offset);

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static NodeId operator +(int offset, NodeId id)
        => new(id.Id + offset);

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static NodeId operator -(int offset, NodeId id)
        => new(id.Id - offset);

    /// <inheritdoc/>
    public override string ToString()
        => Id.ToString();

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => Id.ToString(format, formatProvider);

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten,
        [StringSyntax(StringSyntaxAttribute.NumericFormat)]
        ReadOnlySpan<char> format, IFormatProvider? provider)
        => Id.TryFormat(destination, out charsWritten, format, provider);

    /// <inheritdoc/>
    public bool TryFormat(Span<byte> destination, out int bytesWritten,
        [StringSyntax(StringSyntaxAttribute.NumericFormat)]
        ReadOnlySpan<char> format, IFormatProvider? provider)
        => Id.TryFormat(destination, out bytesWritten, format, provider);
}

#endif
