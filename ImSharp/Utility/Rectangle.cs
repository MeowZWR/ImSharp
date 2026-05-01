namespace ImSharp;

/// <inheritdoc cref="ImRect"/>
/// <remarks> C# version using <seealso cref="Vector2"/> instead of <seealso cref="ImVec2"/>. </remarks>
public readonly record struct Rectangle(Vector2 Minimum, Vector2 Maximum)
{
    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static implicit operator Rectangle(ImRect rect)
        => new(rect.Minimum, rect.Maximum);

    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static implicit operator ImRect(Rectangle rect)
        => new(rect.Minimum, rect.Maximum);

    /// <inheritdoc cref="ImRect.FromSize"/>
    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static Rectangle FromSize(Vector2 minimum, Vector2 size)
        => new(minimum, minimum + size);

    /// <inheritdoc cref="ImRect.FromSize"/>
    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static Rectangle FromSize(Vector2 size)
        => new(Vector2.Zero, size);

    /// <inheritdoc cref="ImRect.FromSize"/>
    [MethodImpl(ImSharpConfiguration.OptInl)]
    public static Rectangle FromSize(float sizeX, float sizeY)
        => new(Vector2.Zero, new Vector2(sizeX, sizeY));

    /// <inheritdoc cref="ImRect.(float,float,float,float)"/>
    [MethodImpl(ImSharpConfiguration.OptInl)]
    public Rectangle(float xMinimum, float yMinimum, float xMaximum, float yMaximum)
        : this(new Vector2(xMinimum, yMinimum), new Vector2(xMaximum, yMaximum))
    { }

    /// <inheritdoc cref="ImRect.Size"/>
    public Vector2 Size
    {
        [MethodImpl(ImSharpConfiguration.OptInl)]
        get => new(Maximum.X - Minimum.X, Maximum.Y - Minimum.Y);
    }
}
