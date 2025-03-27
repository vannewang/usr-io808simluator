// Services/TcpClientService.cs
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TcpClientService : IDisposable
{
    private TcpClient _client;
    private NetworkStream _stream;

    public bool IsConnected => _client?.Connected == true;

    public TcpClientService()
    {
        _client = new TcpClient();
    }

    public async Task ConnectAsync(string ip, int port)
    {
        if (IsConnected) return;

        try
        {
            // 如果之前的client已经关闭，创建新的实例
            if (_client == null || !_client.Connected)
            {
                _client = new TcpClient();
            }

            // 设置连接超时时间为5秒
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await _client.ConnectAsync(IPAddress.Parse(ip), port, cts.Token);
            _stream = _client.GetStream();
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException("连接超时，请检查服务器地址和端口是否正确");
        }
        catch (FormatException)
        {
            throw new ArgumentException("IP地址格式不正确");
        }
        catch (Exception ex)
        {
            throw new Exception($"连接失败: {ex.Message}");
        }
    }

    public void Disconnect()
    {
        if (!IsConnected) return;
        _stream?.Close();
        _client?.Close();
        _stream?.Dispose();
        _client?.Dispose();
        _stream = null;
        _client = null;  // 清空引用，以便下次创建新实例
    }

    public async Task SendDataAsync(byte[] data)
    {
        if (_client?.Connected != true)
        {
            throw new InvalidOperationException("TCP客户端未连接");
        }

        try
        {
            await _stream.WriteAsync(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            Disconnect();
            throw new Exception("发送数据失败", ex);
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
}