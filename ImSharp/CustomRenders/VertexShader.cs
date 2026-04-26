#if HAS_TERRAFX
using TerraFX.Interop.DirectX;

namespace ImSharp;

/// <summary> A Direct3D vertex shader. </summary>
/// <param name="blob"> The vertex shader blob. </param>
/// <param name="description"> A description of this shader, for debugging and logging purposes. </param>
public class VertexShader(byte[] blob, string description) : IDisposable
{
    /// <summary> The vertex shader blob. </summary>
    protected byte[] Blob = blob;

    /// <summary> A description of this shader, for debugging and logging purposes. </summary>
    protected string? Description = description;

    private unsafe ID3D11VertexShader* _shader;

    ~VertexShader()
        => Dispose(false);

    /// <summary> Releases the resources used by this object. </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary> Releases the resources used by this object. </summary>
    /// <param name="disposing"> True if called explicitly, false if garbage collected. </param>
    protected virtual unsafe void Dispose(bool disposing)
        => CustomRenderManager.Release(ref _shader);

    /// <inheritdoc/>
    public override string? ToString()
        => Description ?? base.ToString();

    /// <summary> Gets a shader from this assembly's manifest resources. </summary>
    /// <param name="shaderName"> The shader's name. </param>
    /// <returns> The shader. </returns>
    internal static VertexShader FromManifestResource(string shaderName)
        => new(ResourceProvider.GetManifestResourceBytes(shaderName + "_vs.dxbc"), shaderName);

    /// <summary> Gets the Direct3D pixel shader object, creating it if necessary. </summary>
    /// <returns> The pixel shader object. </returns>
    public unsafe ID3D11VertexShader* GetOrCreateShader()
    {
        if (_shader is null)
            _shader = CreateShader();

        return _shader;
    }

    /// <summary> Creates the Direct3D vertex shader object. </summary>
    /// <returns> The vertex shader object. </returns>
    /// <remarks> This can be overridden to use Direct3D 11 class linkage features. </remarks>
    protected virtual unsafe ID3D11VertexShader* CreateShader()
    {
        ID3D11VertexShader* shader;
        fixed (byte* pBlob = Blob)
        {
            Marshal.ThrowExceptionForHR(
                CustomRenderManager.Instance.Device->CreateVertexShader(pBlob, unchecked((uint)Blob.Length), null, &shader));
        }

        return shader;
    }

    /// <summary> Invalidates the Direct3D shader object. </summary>
    /// <remarks> If using the default implementation of <see cref="CreateShader"/>, this should be called only after changing <see cref="Blob" />. </remarks>
    protected unsafe void InvalidateShader()
        => CustomRenderManager.Release(ref _shader);
}
#endif
