#if HAS_TERRAFX
using TerraFX.Interop.DirectX;

namespace ImSharp;

/// <summary> A full-screen quad with a custom pixel shader and a constant buffer with resolution and reciprocal resolution. </summary>
/// <param name="pixelShader"> The pixel shader to use to render this quad. </param>
/// <param name="description"> A description of this object, for debugging and logging purposes. </param>
public class FullScreenQuad(PixelShader pixelShader, string? description) : ICustomRenderable, IDisposable
{
    private static readonly VertexShader VertexShader = VertexShader.FromManifestResource("FsQuad");

    /// <summary> The pixel shader to use to render this quad. </summary>
    protected PixelShader PixelShader = pixelShader;

    /// <summary> A description of this object, for debugging and logging purposes. </summary>
    protected string? Description = description;

    private uint _savedWidth;
    private uint _savedHeight;

    private unsafe ID3D11Buffer* _resolutionBuffer;

    /// <inheritdoc/>
    public virtual int OutputCount
        => 1;

    /// <inheritdoc/>
    public virtual int KeepAliveDuration
        => 1;

    /// <inheritdoc/>
    public virtual long Version
        => 0;

    /// <inheritdoc/>
    public virtual ITargetClearStrategy? ClearStrategy
        => ITargetClearStrategy.Simple;

    /// <inheritdoc/>
    public virtual D3D11_DEPTH_STENCIL_DESC DepthStencilState
        => ICustomRenderable.DefaultDepthStencilState;

    /// <inheritdoc/>
    public virtual D3D11_RASTERIZER_DESC RasterizerState
        => ICustomRenderable.DefaultRasterizerState;

    ~FullScreenQuad()
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
        => CustomRenderManager.Release(ref _resolutionBuffer);

    /// <inheritdoc/>
    public override string? ToString()
        => Description ?? base.ToString();

    /// <inheritdoc/>
    public virtual DXGI_FORMAT GetOutputFormat(int outputIndex)
        => DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM;

    /// <summary> Creates a Direct3D constant buffer object. </summary>
    /// <param name="initialContents"> The initial contents of the buffer. May be null. </param>
    /// <param name="size"> The size of the buffer. </param>
    /// <returns> The constant buffer object. </returns>
    public static unsafe ID3D11Buffer* CreateConstantBuffer(void* initialContents, int size)
    {
        var bufferDesc = new D3D11_BUFFER_DESC
        {
            ByteWidth           = (uint)((size + 15) & ~15),
            Usage               = D3D11_USAGE.D3D11_USAGE_DEFAULT,
            BindFlags           = (uint)D3D11_BIND_FLAG.D3D11_BIND_CONSTANT_BUFFER,
            CPUAccessFlags      = 0,
            MiscFlags           = 0,
            StructureByteStride = 0,
        };

        ID3D11Buffer* uniformsBuffer;
        var subresData = new D3D11_SUBRESOURCE_DATA
        {
            pSysMem          = initialContents,
            SysMemPitch      = 0,
            SysMemSlicePitch = 0,
        };

        Marshal.ThrowExceptionForHR(CustomRenderManager.Instance.Device->CreateBuffer(&bufferDesc,
            initialContents is not null ? &subresData : null, &uniformsBuffer));

        return uniformsBuffer;
    }

    /// <summary> Gets the Direct3D constant buffer object for the output resolution, creating or updating it if necessary. </summary>
    /// <param name="width"> The output width. </param>
    /// <param name="height"> The output height. </param>
    /// <param name="deviceContext"> The device context to use if the buffer needs to be updated. </param>
    /// <returns> The constant buffer object. </returns>
    protected unsafe ID3D11Buffer* GetOrCreateResolutionBuffer(uint width, uint height, ID3D11DeviceContext* deviceContext)
    {
        if (_resolutionBuffer is not null)
        {
            if (width != _savedWidth || height != _savedHeight)
            {
                var resolution = new Vector4(width, height, 1.0f / width, 1.0f / height);
                deviceContext->UpdateSubresource((ID3D11Resource*)_resolutionBuffer, 0, null, &resolution, 0, 0);
                _savedWidth  = width;
                _savedHeight = height;
            }

            return _resolutionBuffer;
        }

        var initialResolution = new Vector4(width, height, 1.0f / width, 1.0f / height);
        _resolutionBuffer = CreateConstantBuffer(&initialResolution, sizeof(Vector4));

        _savedWidth  = width;
        _savedHeight = height;
        return _resolutionBuffer;
    }

    /// <inheritdoc/>
    public virtual unsafe void Render(uint width, uint height, ID3D11DeviceContext* deviceContext)
    {
        // Bind the vertex shader (see FsQuad_vs.hlsl), no geometry shader, and the pixel shader supplied by inheritors or callers.
        // The vertex shader takes no cbuffers, resources or samplers.
        deviceContext->VSSetShader(VertexShader.GetOrCreateShader(), null, 0);
        deviceContext->GSSetShader(null, null, 0);
        BindPixelShader(width, height, deviceContext);

        // The vertex shader takes no inputs except the vertex ID, which is managed by the system.
        // We are drawing a triangle strip of 4 vertices starting at 0:
        // - The vertex shader will get called with SV_VertexID = 0, 1, 2 and 3, and no other input.
        // - One triangle will be assembled with the output of 0, 1 and 2, another with the outputs of 1, 2 and 3.
        // - The two resulting triangles will be shaded as usual, using the inheritor/caller's pixel shader.
        deviceContext->IASetInputLayout(null);
        deviceContext->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY.D3D11_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);
        deviceContext->IASetVertexBuffers(0, 0, null, null, null);
        deviceContext->IASetIndexBuffer(null, DXGI_FORMAT.DXGI_FORMAT_UNKNOWN, 0);
        deviceContext->Draw(4, 0);
    }

    /// <summary> Binds the pixel shader and its inputs to the given device context. </summary>
    /// <param name="width"> The output width. </param>
    /// <param name="height"> The output height. </param>
    /// <param name="deviceContext"> The device context to run commands on. </param>
    protected virtual unsafe void BindPixelShader(uint width, uint height, ID3D11DeviceContext* deviceContext)
    {
        // This default implementation binds the pixel shader, with the resolution cbuffer at slot 0 (see FsQuad.hlsli).
        deviceContext->PSSetShader(PixelShader.GetOrCreateShader(), null, 0);
        var resolutionBuffer = GetOrCreateResolutionBuffer(width, height, deviceContext);
        deviceContext->PSSetConstantBuffers(0, 1, &resolutionBuffer);
    }
}
#endif
