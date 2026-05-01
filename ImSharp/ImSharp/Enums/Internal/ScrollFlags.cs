namespace ImSharp.Internal;

/// <summary> TODO </summary>
[Flags]
public enum ScrollFlags : uint
{
    /// <summary> TODO </summary>
    None = 0,

    /// <summary> TODO </summary>
    KeepVisibleEdgeX = 1 << 0,

    /// <summary> TODO </summary>
    KeepVisibleEdgeY = 1 << 1,

    /// <summary> TODO </summary>
    KeepVisibleCenterX = 1 << 2,

    /// <summary> TODO </summary>
    KeepVisibleCenterY = 1 << 3,

    /// <summary> TODO </summary>
    AlwaysCenterX = 1 << 4,

    /// <summary> TODO </summary>
    AlwaysCenterY = 1 << 5,

    /// <summary> TODO </summary>
    NoScrollParent = 1 << 6,

    /// <summary> TODO </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    MaskX = KeepVisibleEdgeX | KeepVisibleCenterX | AlwaysCenterX,

    /// <summary> TODO </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    MaskY = KeepVisibleEdgeY | KeepVisibleCenterY | AlwaysCenterY,
}
