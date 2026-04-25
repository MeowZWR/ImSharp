#if HAS_TERRAFX
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;

namespace ImSharp;

/// <summary> A manager to handle caches for custom renderable objects. </summary>
public sealed class CustomRenderingManager : IDisposable
{
    /// <summary> The custom rendering manager. </summary>
    public static readonly CustomRenderingManager Instance = new();

    private readonly ConditionalWeakTable<ICustomRenderable, Dictionary<(uint, uint), RenderCache>> _caches = [];

    private unsafe ID3D11Device* _device;

    /// <summary> The Direct3D 11 device to render on. </summary>
    public unsafe ID3D11Device* Device
        => _device;

    private CustomRenderingManager()
        => ImSharpPerFrame.Update += CheckCachedRenderings;

    ~CustomRenderingManager()
        => Dispose(false);

    /// <summary> Dispose and remove all cached renderings. </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private unsafe void Dispose(bool disposing)
    {
        foreach (var (_, caches) in _caches)
            Clear(caches);
        _caches.Clear();
        ImSharpPerFrame.Update -= CheckCachedRenderings;
        Release(ref _device);
    }

    private static void Clear(Dictionary<(uint, uint), RenderCache> caches)
    {
        foreach (var (_, cache) in caches)
            cache.Dispose();
        caches.Clear();
    }

    /// <summary> Sets the Direct3D 11 device to render on. </summary>
    /// <param name="device"> The device. </param>
    public unsafe void SetDevice(nint device)
    {
        var           deviceUnk = (IUnknown*)device;
        ID3D11Device* newDevice;
        Marshal.ThrowExceptionForHR(deviceUnk->QueryInterface((Guid*)Unsafe.AsPointer(in IID.IID_ID3D11Device), (void**)&newDevice));
        Release(ref _device);
        _device = newDevice;
    }

    /// <summary> Atomically sets the pointer at the given location and returns the old pointer. </summary>
    /// <param name="location"> The location to set. </param>
    /// <param name="newPtr"> The new pointer to write at the given location. </param>
    /// <typeparam name="T"> The type of the pointed value. </typeparam>
    /// <returns> The pointer that was at the given location before. </returns>
    public static unsafe T* Exchange<T>(ref T* location, T* newPtr) where T : unmanaged
    {
        fixed (T** pPtr = &location)
            return (T*)Interlocked.Exchange(ref *(nint*)pPtr, (nint)newPtr);
    }

    /// <summary> Releases and clears the COM object pointer at the given location. If it held a null pointer, this is a no-op. </summary>
    /// <param name="location"> The location to clear. </param>
    /// <typeparam name="T"> The type of the COM object. </typeparam>
    public static unsafe void Release<T>(ref T* location) where T : unmanaged, IUnknown.Interface
    {
        var old = Exchange(ref location, null);
        if (old is not null)
            old->Release();
    }

    /// <summary> Renders an object, and returns the output in a form suitable for use as an ImGui image. </summary>
    /// <param name="renderable"> The object to render. </param>
    /// <param name="width"> The width at which to render the object. </param>
    /// <param name="height"> The height at which to render the object. </param>
    /// <param name="outputIndex"> If this object has multiple render outputs, the index, otherwise 0. </param>
    /// <returns> An ImGui texture ID representing the rendered object. </returns>
    public ImTextureId RenderObject(ICustomRenderable renderable, uint width, uint height, int outputIndex = 0)
    {
        ImTextureId output = default;
        RenderObject(renderable, width, height, outputIndex, new(ref output));
        return output;
    }

    /// <summary> Renders an object, and returns the outputs in a form suitable to use as ImGui images. </summary>
    /// <param name="renderable"> The object to render. </param>
    /// <param name="width"> The width at which to render the object. </param>
    /// <param name="height"> The height at which to render the object. </param>
    /// <param name="outputIndex"> The first render output index. </param>
    /// <param name="outputs"> On return, ImGui texture IDs representing the rendered object. </param>
    public unsafe void RenderObject(ICustomRenderable renderable, uint width, uint height, int outputIndex, Span<ImTextureId> outputs)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(outputIndex, 0);
        if (outputs.Length > D3D11.D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT)
            throw new ArgumentException("Output count exceeds D3D11's maximum simultaneous render target count");

        var version = renderable.Version;
        var caches  = _caches.GetOrCreateValue(renderable);
        if (!caches.TryGetValue((width, height), out var cache))
        {
            // Cause a version mismatch on purpose to simplify the paths below.
            cache              = new RenderCache(unchecked(version - 1));
            cache.DepthStencil = new DepthStencil(_device, width, height);
            caches.Add((width, height), cache);
        }

        if (cache.Version == version)
        {
            cache.ExpiresAtFrame = Im.Context.FrameCount + renderable.KeepAliveDuration;
            cache.ExportOutputs(outputIndex, outputs);
            return;
        }

        cache.SetOutputCount(renderable.OutputCount, _device, width, height, renderable.GetOutputFormat);

        using (new DeviceImmediateContext(_device, out var deviceContext))
        {
            // Save some state to restore it later, so we play nice with ImGui, the game itself, and other DirectX consumers in the process.
            // Despite changing the RS (Rasterizer) Viewports, saving and restoring them seems unnecessary.
            using var savedRsState = new SavedRasterizerState(deviceContext);
            using var savedRtViews = new SavedRenderTargetViews(deviceContext);
            using var savedDsState = new SavedDepthStencilState(deviceContext);

            // First clear the depth/stencil and render targets, if and how the renderable wants it.
            if (renderable.ClearStrategy is { } clearStrategy)
            {
                clearStrategy.ClearDepthStencil(deviceContext, cache.DepthStencil.DepthStencilView);
                for (var i = 0; i < cache.Outputs.Length; ++i)
                    clearStrategy.ClearRenderTarget(deviceContext, i, cache.Outputs[i].RenderTargetView);
            }

            // Install our own output configuration (RS Viewports + our version of the stuff we saved earlier).
            SetSimpleViewport(deviceContext, width, height);
            SetRasterizerState(deviceContext, renderable.RasterizerState);
            SetDepthStencilState(deviceContext, renderable.DepthStencilState, 0);
            cache.SetRenderTargets(deviceContext);

            // Our output configuration and render targets are installed, now the renderable may run its own draw calls.
            renderable.Render(width, height, deviceContext);
        }

        cache.ExpiresAtFrame = Im.Context.FrameCount + renderable.KeepAliveDuration;
        cache.Version        = version;
        cache.ExportOutputs(outputIndex, outputs);
    }

    /// <summary> Check all cached renderings for disposal. </summary>
    /// <remarks> Any rendering that has not been retrieved for at least its <seealso cref="ICustomRenderable.KeepAliveDuration"/> frames will be disposed and removed. </remarks>
    private void CheckCachedRenderings()
    {
        var frame              = Im.Context.FrameCount;
        var discardRenderables = new HashSet<ICustomRenderable>(32);
        var discardSizes       = new HashSet<(uint, uint)>(16);
        foreach (var (renderable, caches) in _caches)
        {
            var version = renderable.Version;
            discardSizes.Clear();
            foreach (var (size, cache) in caches)
            {
                if (cache.Version != version || cache.ExpiresAtFrame <= frame)
                {
                    cache.Dispose();
                    discardSizes.Add(size);
                }
            }

            foreach (var size in discardSizes)
                caches.Remove(size);
            if (caches.Count is 0)
                discardRenderables.Add(renderable);
        }

        foreach (var renderable in discardRenderables)
            _caches.Remove(renderable);
    }

    private static unsafe void SetSimpleViewport(ID3D11DeviceContext* deviceContext, float width, float height)
    {
        var viewport = new D3D11_VIEWPORT
        {
            TopLeftX = 0.0f,
            TopLeftY = 0.0f,
            Width    = width,
            Height   = height,
            MinDepth = 0.0f,
            MaxDepth = 1.0f,
        };
        deviceContext->RSSetViewports(1, &viewport);
    }

    private unsafe void SetRasterizerState(ID3D11DeviceContext* deviceContext, in D3D11_RASTERIZER_DESC desc)
    {
        ID3D11RasterizerState* rsState;
        fixed (D3D11_RASTERIZER_DESC* pDesc = &desc)
            Marshal.ThrowExceptionForHR(_device->CreateRasterizerState(pDesc, &rsState));
        deviceContext->RSSetState(rsState);
        Release(ref rsState);
    }

    private unsafe void SetDepthStencilState(ID3D11DeviceContext* deviceContext, in D3D11_DEPTH_STENCIL_DESC desc, uint stencilRef)
    {
        ID3D11DepthStencilState* dsState;
        fixed (D3D11_DEPTH_STENCIL_DESC* pDesc = &desc)
            Marshal.ThrowExceptionForHR(_device->CreateDepthStencilState(pDesc, &dsState));
        deviceContext->OMSetDepthStencilState(dsState, stencilRef);
        Release(ref dsState);
    }

    private unsafe ref struct DeviceImmediateContext
    {
        private ID3D11DeviceContext* _deviceContext;

        public DeviceImmediateContext(ID3D11Device* device, out ID3D11DeviceContext* deviceContext)
        {
            fixed (DeviceImmediateContext* pThis = &this)
                device->GetImmediateContext(&pThis->_deviceContext);
            deviceContext = _deviceContext;
        }

        public void Dispose()
            => Release(ref _deviceContext);
    }

    private unsafe ref struct SavedRasterizerState
    {
        private ID3D11DeviceContext*   _deviceContext;
        private ID3D11RasterizerState* _rsState;

        public SavedRasterizerState(ID3D11DeviceContext* deviceContext)
        {
            _deviceContext = deviceContext;
            fixed (SavedRasterizerState* pThis = &this)
                deviceContext->RSGetState(&pThis->_rsState);
        }

        public void Dispose()
        {
            _deviceContext->RSSetState(_rsState);
            Release(ref _rsState);
        }
    }

    private unsafe ref struct SavedRenderTargetViews
    {
        // Poor man's static_assert.
        private const uint _0 = D3D11.D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT == 8 ? 0 : -666;

        private ID3D11DeviceContext*    _deviceContext;
        private ID3D11RenderTargetView* _rtView0;
        private ID3D11RenderTargetView* _rtView1;
        private ID3D11RenderTargetView* _rtView2;
        private ID3D11RenderTargetView* _rtView3;
        private ID3D11RenderTargetView* _rtView4;
        private ID3D11RenderTargetView* _rtView5;
        private ID3D11RenderTargetView* _rtView6;
        private ID3D11RenderTargetView* _rtView7;
        private ID3D11DepthStencilView* _dsView;

        public SavedRenderTargetViews(ID3D11DeviceContext* deviceContext)
        {
            _deviceContext = deviceContext;
            fixed (SavedRenderTargetViews* pThis = &this)
                deviceContext->OMGetRenderTargets(8, &pThis->_rtView0, &pThis->_dsView);
        }

        public void Dispose()
        {
            fixed (SavedRenderTargetViews* pThis = &this)
                _deviceContext->OMSetRenderTargets(8, &pThis->_rtView0, pThis->_dsView);
            Release(ref _rtView0);
            Release(ref _rtView1);
            Release(ref _rtView2);
            Release(ref _rtView3);
            Release(ref _rtView4);
            Release(ref _rtView5);
            Release(ref _rtView6);
            Release(ref _rtView7);
            Release(ref _dsView);
        }
    }

    private unsafe ref struct SavedDepthStencilState
    {
        private ID3D11DeviceContext*     _deviceContext;
        private ID3D11DepthStencilState* _dsState;
        private uint                     _stencilRef;

        public SavedDepthStencilState(ID3D11DeviceContext* deviceContext)
        {
            _deviceContext = deviceContext;
            fixed (SavedDepthStencilState* pThis = &this)
                deviceContext->OMGetDepthStencilState(&pThis->_dsState, &pThis->_stencilRef);
        }

        public void Dispose()
        {
            _deviceContext->OMSetDepthStencilState(_dsState, _stencilRef);
            Release(ref _dsState);
        }
    }

    private sealed class RenderCache(long version) : IDisposable
    {
        public long           Version = version;
        public int            ExpiresAtFrame;
        public DepthStencil   DepthStencil;
        public RenderTarget[] Outputs = [];

        ~RenderCache()
            => Dispose();

        public void Dispose()
        {
            if (Outputs.Length is not 0)
            {
                foreach (var output in Outputs)
                    output.Dispose();
                Outputs = [];
            }

            DepthStencil.Dispose();
        }

        public void ExportOutputs(int outputIndex, Span<ImTextureId> outputs)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(outputIndex, Outputs.Length);
            if (outputIndex + outputs.Length > Outputs.Length)
                throw new ArgumentException("Some of the requested outputs are past the renderable's output count");

            for (var i = 0; i < outputs.Length; ++i)
                outputs[i] = Outputs[outputIndex + i];
        }

        public unsafe void SetRenderTargets(ID3D11DeviceContext* deviceContext)
        {
            var outputRtViews = stackalloc ID3D11RenderTargetView*[Outputs.Length];
            for (var i = 0; i < Outputs.Length; ++i)
                outputRtViews[i] = Outputs[i].RenderTargetView;
            deviceContext->OMSetRenderTargets((uint)Outputs.Length, outputRtViews, DepthStencil.DepthStencilView);
        }

        public unsafe void SetOutputCount(int count, ID3D11Device* device, uint width, uint height, Func<int, DXGI_FORMAT> format)
        {
            var previousCount = Outputs.Length;
            if (count == previousCount)
                return;

            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, D3D11.D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT);

            for (var i = count; i < previousCount; ++i)
                Outputs[i].Dispose();

            Array.Resize(ref Outputs, count);
            for (var i = previousCount; i < count; ++i)
                Outputs[i] = new RenderTarget(device, width, height, format(i));
        }
    }

    private unsafe struct Texture2D : IDisposable
    {
        public ID3D11Texture2D*          Texture;
        public ID3D11ShaderResourceView* ShaderResourceView;

        public Texture2D(ID3D11Device* device, uint width, uint height, DXGI_FORMAT format, D3D11_BIND_FLAG bind)
        {
            fixed (Texture2D* pThis = &this)
            {
                var texDesc = new D3D11_TEXTURE2D_DESC
                {
                    Width          = width,
                    Height         = height,
                    MipLevels      = 1,
                    ArraySize      = 1,
                    Format         = format,
                    SampleDesc     = new(1, 0),
                    Usage          = D3D11_USAGE.D3D11_USAGE_DEFAULT,
                    BindFlags      = (uint)(bind | D3D11_BIND_FLAG.D3D11_BIND_SHADER_RESOURCE),
                    CPUAccessFlags = 0,
                    MiscFlags      = 0,
                };
                var srvDesc = new D3D11_SHADER_RESOURCE_VIEW_DESC
                {
                    ViewDimension = D3D_SRV_DIMENSION.D3D11_SRV_DIMENSION_TEXTURE2D,
                    Format        = format,
                };
                Marshal.ThrowExceptionForHR(device->CreateTexture2D(&texDesc, null, &pThis->Texture));
                try
                {
                    Marshal.ThrowExceptionForHR(
                        device->CreateShaderResourceView((ID3D11Resource*)Texture, &srvDesc, &pThis->ShaderResourceView));
                }
                catch
                {
                    Release(ref Texture);
                    throw;
                }
            }
        }

        public void Dispose()
        {
            Release(ref ShaderResourceView);
            Release(ref Texture);
        }

        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static implicit operator ImTextureId(in Texture2D texture)
            => new((nint)texture.ShaderResourceView);
    }

    private unsafe struct DepthStencil : IDisposable
    {
        public Texture2D               Texture;
        public ID3D11DepthStencilView* DepthStencilView;

        public DepthStencil(ID3D11Device* device, uint width, uint height, DXGI_FORMAT format = DXGI_FORMAT.DXGI_FORMAT_D24_UNORM_S8_UINT)
        {
            Texture = new Texture2D(device, width, height, format, D3D11_BIND_FLAG.D3D11_BIND_DEPTH_STENCIL);
            try
            {
                var dsvDesc = new D3D11_DEPTH_STENCIL_VIEW_DESC
                {
                    ViewDimension = D3D11_DSV_DIMENSION.D3D11_DSV_DIMENSION_TEXTURE2D,
                    Format        = format,
                };
                ID3D11DepthStencilView* dsv;
                Marshal.ThrowExceptionForHR(device->CreateDepthStencilView((ID3D11Resource*)Texture.Texture, &dsvDesc, &dsv));
                DepthStencilView = dsv;
            }
            catch
            {
                Texture.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            Release(ref DepthStencilView);
            Texture.Dispose();
        }

        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static implicit operator ImTextureId(in DepthStencil depthStencil)
            => depthStencil.Texture;
    }

    private unsafe struct RenderTarget : IDisposable
    {
        public Texture2D               Texture;
        public ID3D11RenderTargetView* RenderTargetView;

        public RenderTarget(ID3D11Device* device, uint width, uint height, DXGI_FORMAT format)
        {
            Texture = new Texture2D(device, width, height, format, D3D11_BIND_FLAG.D3D11_BIND_RENDER_TARGET);
            try
            {
                var rtvDesc = new D3D11_RENDER_TARGET_VIEW_DESC
                {
                    ViewDimension = D3D11_RTV_DIMENSION.D3D11_RTV_DIMENSION_TEXTURE2D,
                    Format        = format,
                };
                ID3D11RenderTargetView* rtv;
                Marshal.ThrowExceptionForHR(device->CreateRenderTargetView((ID3D11Resource*)Texture.Texture, &rtvDesc, &rtv));
                RenderTargetView = rtv;
            }
            catch
            {
                Texture.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            Release(ref RenderTargetView);
            Texture.Dispose();
        }

        [MethodImpl(ImSharpConfiguration.OptInl)]
        public static implicit operator ImTextureId(in RenderTarget renderTarget)
            => renderTarget.Texture;
    }
}
#endif
