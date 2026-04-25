#if HAS_TERRAFX
using TerraFX.Interop.DirectX;

namespace ImSharp;

public class FullScreenQuadWithUniforms<TUniforms>(byte[] pixelShaderBlob, TUniforms uniforms)
    : FullScreenQuad(pixelShaderBlob) where TUniforms : unmanaged
{
    private long _version = 0;

    /// <summary> The pixel shader input data. </summary>
    public TUniforms Uniforms = uniforms;

    private unsafe ID3D11Buffer* _uniformsBuffer;
    private        long          _bufferVersion = 0;

    /// <inheritdoc/>
    public override long Version
        => _version;

    /// <summary> Increments this object's version, invalidating all cached renders. Use after modifying <see cref="Uniforms"/>. </summary>
    public void Update()
        => ++_version;

    /// <inheritdoc/>
    protected override unsafe void Dispose(bool disposing)
    {
        CustomRenderManager.Release(ref _uniformsBuffer);
        base.Dispose(disposing);
    }

    /// <summary> Gets the Direct3D constant buffer object for <see cref="Uniforms"/>, creating or updating it if necessary. </summary>
    /// <param name="deviceContext"> The device context to use if the buffer needs to be updated. </param>
    /// <returns> The constant buffer object. </returns>
    protected unsafe ID3D11Buffer* GetOrCreateUniformsBuffer(ID3D11DeviceContext* deviceContext)
    {
        if (_uniformsBuffer is not null)
        {
            if (_bufferVersion != _version)
            {
                fixed (TUniforms* pUniforms = &Uniforms)
                    deviceContext->UpdateSubresource((ID3D11Resource*)_uniformsBuffer, 0, null, pUniforms, 0, 0);
                _bufferVersion = _version;
            }

            return _uniformsBuffer;
        }

        fixed (TUniforms* pUniforms = &Uniforms)
            _uniformsBuffer = CreateConstantBuffer(pUniforms, sizeof(TUniforms));

        _bufferVersion = _version;
        return _uniformsBuffer;
    }

    /// <inheritdoc/>
    protected override unsafe void BindPixelShader(uint width, uint height, ID3D11DeviceContext* deviceContext)
    {
        // Call the default implementation, then bind the uniforms cbuffer at slot 1.
        base.BindPixelShader(width, height, deviceContext);
        var uniformsBuffer = GetOrCreateUniformsBuffer(deviceContext);
        deviceContext->PSSetConstantBuffers(1, 1, &uniformsBuffer);
    }
}
#endif
