using System;
using System.Threading;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using static Vortice.DXGI.DXGI;
using static Vortice.Direct3D11.D3D11;

namespace NightScreenViewerBackend
{
    public sealed class GlowRenderer : IDisposable
    {
        private readonly GlowForm _form;
        private IDXGISwapChain1? _swapChain;
        private ID3D11Device? _device;
        private ID3D11DeviceContext? _context;
        private ID3D11RenderTargetView? _rtv;
        private CancellationTokenSource? _loopCts;

        public GlowRenderer(GlowForm form)
        {
            _form = form;
        }

        public void Initialize()
        {
            var desc = new SwapChainDescription1
            {
                Width = _form.Bounds.Width,
                Height = _form.Bounds.Height,
                Format = Format.R8G8B8A8_UNorm,
                Stereo = false,
                SampleDescription = new SampleDescription(1, 0),
                BufferUsage = Usage.RenderTargetOutput,
                BufferCount = 2,
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipSequential,
                AlphaMode = AlphaMode.Ignore
            };

            // 创建 DXGI 工厂
            using var dxgiFactory = CreateDXGIFactory2<IDXGIFactory2>(false);

            // ✅ 获取第一个显卡适配器（2.4.2 版本正确写法）
            IDXGIAdapter1? adapter;
            dxgiFactory.EnumAdapters1(0, out adapter);

            // 创建设备 + 上下文
            ID3D11Device device;
            ID3D11DeviceContext context;
            D3D11CreateDevice(
                adapter,
                DriverType.Unknown,
                DeviceCreationFlags.BgraSupport,
                null,
                out device,
                out context
            );
            _device = device;
            _context = context;

            // 创建交换链
            using var swap = dxgiFactory.CreateSwapChainForHwnd(
                _device,
                _form.Hwnd,
                desc,
                null,
                null
            );
            _swapChain = swap;

            // 创建渲染目标视图
            using var backBuffer = _swapChain.GetBuffer<ID3D11Texture2D>(0);
            _rtv = _device.CreateRenderTargetView(backBuffer);

            Console.WriteLine("[TheaterMode] GlowRenderer initialized successfully.");
        }

        public void Start()
        {
            _loopCts = new CancellationTokenSource();
            var token = _loopCts.Token;

            Task.Run(
                async () =>
                {
                    float t = 0;
                    while (!token.IsCancellationRequested)
                    {
                        t += 0.05f;
                        var intensity = 0.5f + 0.5f * (float)Math.Abs(Math.Sin(t)); // 呼吸范围 0.5~1
                        var clear = new Vortice.Mathematics.Color4(intensity, 0.0f, 0.0f, 1.0f); // 纯红色

                        _context!.OMSetRenderTargets(new[] { _rtv! }, null);
                        _context.ClearRenderTargetView(_rtv!, clear);
                        _swapChain!.Present(1, PresentFlags.None);

                        await Task.Delay(16, token);
                    }
                },
                token
            );
        }

        public void Resize()
        {
            if (_swapChain == null)
                return;
            _context!.OMSetRenderTargets(Array.Empty<ID3D11RenderTargetView>(), null);
            _rtv?.Dispose();
            _swapChain.ResizeBuffers(
                0,
                _form.Bounds.Width,
                _form.Bounds.Height,
                Format.Unknown,
                SwapChainFlags.None
            );
            using var backBuffer = _swapChain.GetBuffer<ID3D11Texture2D>(0);
            _rtv = _device!.CreateRenderTargetView(backBuffer);
        }

        public void Dispose()
        {
            try
            {
                _loopCts?.Cancel();
            }
            catch { }
            _rtv?.Dispose();
            _swapChain?.Dispose();
            _context?.Dispose();
            _device?.Dispose();
        }
    }
}
