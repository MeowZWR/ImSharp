#if HAS_TERRAFX
using TerraFX.Interop.DirectX;

namespace ImSharp;

/// <summary> A full-screen quad with a custom pixel shader. </summary>
/// <param name="pixelShaderBlob"> The pixel shader blob to use to render this quad. </param>
public class FullScreenQuad(byte[] pixelShaderBlob, string? description) : ICustomRenderable, IDisposable
{
    private static readonly byte[] VertexShaderBlob = ResourceProvider.GetManifestResourceBytes("FsQuad_vs.dxbc");

    /// <summary> The pixel shader blob to use to render this quad. </summary>
    protected byte[] PixelShaderBlob = pixelShaderBlob;

    /// <summary> A description of this object, for debugging and logging purposes. </summary>
    protected string? Description = description;

    private uint _savedWidth;
    private uint _savedHeight;

    private unsafe ID3D11VertexShader* _vertexShader;
    private unsafe ID3D11PixelShader*  _pixelShader;
    private unsafe ID3D11Buffer*       _resolutionBuffer;

    /// <inheritdoc/>
    public virtual int OutputCount
        => 1;

    /// <inheritdoc/>
    public virtual int KeepAliveDuration
        => 2;

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
    {
        CustomRenderManager.Release(ref _pixelShader);
        CustomRenderManager.Release(ref _vertexShader);
    }

    /// <inheritdoc/>
    public override string? ToString()
        => Description ?? base.ToString();

    /// <inheritdoc/>
    public virtual DXGI_FORMAT GetOutputFormat(int outputIndex)
        => DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM;

    private unsafe ID3D11VertexShader* GetOrCreateVertexShader()
    {
        if (_vertexShader is not null)
            return _vertexShader;

        ID3D11VertexShader* vertexShader;
        fixed (byte* pVertexShaderBlob = VertexShaderBlob)
        {
            Marshal.ThrowExceptionForHR(CustomRenderManager.Instance.Device->CreateVertexShader(pVertexShaderBlob,
                unchecked((uint)VertexShaderBlob.Length), null, &vertexShader));
        }

        _vertexShader = vertexShader;
        return vertexShader;
    }

    /// <summary> Gets the Direct3D pixel shader object, creating it if necessary. </summary>
    /// <returns> The pixel shader object. </returns>
    protected unsafe ID3D11PixelShader* GetOrCreatePixelShader()
    {
        if (_pixelShader is not null)
            return _pixelShader;

        _pixelShader = CreatePixelShader();
        return _pixelShader;
    }

    /// <summary> Creates the Direct3D pixel shader object. </summary>
    /// <returns> The pixel shader object. </returns>
    /// <remarks> This can be overridden to use Direct3D 11 class linkage features. </remarks>
    protected virtual unsafe ID3D11PixelShader* CreatePixelShader()
    {
        ID3D11PixelShader* pixelShader;
        fixed (byte* pPixelShaderBlob = PixelShaderBlob)
        {
            Marshal.ThrowExceptionForHR(CustomRenderManager.Instance.Device->CreatePixelShader(pPixelShaderBlob,
                unchecked((uint)PixelShaderBlob.Length), null, &pixelShader));
        }

        return pixelShader;
    }

    /// <summary> Invalidates the Direct3D pixel shader object. </summary>
    /// <remarks> If using the default implementation of <see cref="CreatePixelShader"/>, this should be called only after changing <see cref="PixelShaderBlob" />. </remarks>
    protected unsafe void InvalidatePixelShader()
        => CustomRenderManager.Release(ref _pixelShader);

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
        deviceContext->VSSetShader(GetOrCreateVertexShader(), null, 0);
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
        deviceContext->PSSetShader(GetOrCreatePixelShader(), null, 0);
        var resolutionBuffer = GetOrCreateResolutionBuffer(width, height, deviceContext);
        deviceContext->PSSetConstantBuffers(0, 1, &resolutionBuffer);
    }
}
#endif
