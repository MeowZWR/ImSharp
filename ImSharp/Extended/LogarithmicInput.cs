namespace ImSharp;

public static partial class ImEx
{
    /// <summary> Draw a numerical input where the buttons half or double the value respectively. </summary>
    /// <typeparam name="T"> The type of number. </typeparam>
    /// <param name="label"> The label of the input as text. Does not have to be null-terminated. </param>
    /// <param name="displayFormat"> The format to display the number in the (read-only) display frame. </param>
    /// <param name="input"> The input value. </param>
    /// <param name="outputValue"> The output value. </param>
    /// <returns> True if the value changed this frame. </returns>
    public static bool LogarithmicInput<T>(Utf8LabelHandler label, Utf8HintHandler displayFormat, T input, out T outputValue)
        where T : unmanaged, INumber<T>
    {
        using var id    = Im.Id.Push(label);
        using var group = Im.Group();
        Im.Input.Scalar("##i"u8, ref input, displayFormat, T.Zero, T.Zero, InputTextFlags.ReadOnly);

        outputValue = input;
        var (isOne, isZero) = T.MultiplicativeIdentity.Equals(input)
            ? (true, false)
            : (false, T.Zero.Equals(input));
        var size = new Vector2(Im.Style.FrameHeight);
        var ret  = false;
        Im.Line.SameInner();
        if (Button("-"u8, size, isZero ? StringU8.Empty : isOne ? "Set the value to zero."u8 : "Half the current value."u8, isZero))
        {
            outputValue = isOne ? T.AdditiveIdentity : input / (T.MultiplicativeIdentity + T.MultiplicativeIdentity);
            ret         = true;
        }

        Im.Line.SameInner();
        if (Button("+"u8, size, isZero ? "Set the value to one."u8 : "Double the current value."u8))
        {
            outputValue = isZero ? T.MultiplicativeIdentity : input + input;
            ret         = true;
        }
        Im.Line.SameInner();
        if (Button("0"u8, size, isZero ? StringU8.Empty : "Set the value to zero."u8, isZero))
        {
            outputValue = T.AdditiveIdentity;
            ret         = true;
        }

        TextLabel(ref label);

        return ret;
    }

    /// <inheritdoc cref="LogarithmicInput{T}(Utf8LabelHandler,Utf8HintHandler,T,out T)"/>
    public static bool LogarithmicInput<T>(Utf8LabelHandler label, T input, out T scalar)
        where T : unmanaged, INumber<T>
        => LogarithmicInput(label, $"{input}", input, out scalar);

    /// <summary> Draw a numerical input where the buttons half or double the value respectively. </summary>
    /// <typeparam name="T"> The type of number. </typeparam>
    /// <param name="label"> The label of the input as text. Does not have to be null-terminated. </param>
    /// <param name="displayFormat"> The format to display the number in the (read-only) display frame. </param>
    /// <param name="value"> The input/output value. </param>
    /// <returns> True if the value changed this frame. </returns>
    public static bool LogarithmicInput<T>(Utf8LabelHandler label, Utf8HintHandler displayFormat, ref T value)
        where T : unmanaged, INumber<T>
    {
        if (!LogarithmicInput(label, displayFormat, value, out var tmp))
            return false;

        value = tmp;
        return true;
    }

    /// <inheritdoc cref="LogarithmicInput{T}(Utf8LabelHandler,Utf8HintHandler,ref T)"/>
    public static bool LogarithmicInput<T>(Utf8LabelHandler label, ref T value)
        where T : unmanaged, INumber<T>
        => LogarithmicInput(label, $"{value}", ref value);
}
