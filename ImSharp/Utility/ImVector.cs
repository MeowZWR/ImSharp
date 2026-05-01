namespace ImSharp;

/// <summary> A wrapper around the vector type used by ImGui. </summary>
public unsafe struct ImVector<T> : IReadOnlyList<T>
    where T : unmanaged
{
    /// <summary> The number of objects stored. </summary>
    public int Size;

    /// <summary> The number of objects that space was allocated for. </summary>
    public int Capacity;

    /// <summary> The pointer to the allocated space. </summary>
    public T* Data;

    /// <summary> Access the object at the given index. </summary>
    /// <remarks> Does not check for size. </remarks>
    public ref T this[int index]
    {
        [MethodImpl(ImSharpConfiguration.OptInl)]
        get => ref Data[index];
    }

    /// <inheritdoc/>
    T IReadOnlyList<T>.this[int index]
    {
        [MethodImpl(ImSharpConfiguration.OptInl)]
        get => Data[index];
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Size; ++i)
            yield return this[i];
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <inheritdoc/>
    public int Count
    {
        [MethodImpl(ImSharpConfiguration.OptInl)]
        get => Size;
    }
}
