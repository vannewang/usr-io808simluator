// ViewModels/MainViewModel.cs
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Net;
using System.Diagnostics;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly TcpClientService _tcpService = new TcpClientService();
    private readonly AppConfig _config = new AppConfig();
    private bool _isConnected;
    // 命令定义
    public ICommand SendCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand ConnectCommand { get; }
    public ICommand DisconnectCommand { get; }

    // 添加公共属性来访问配置
    public AppConfig Config => _config;

    // 连接状态
    public bool IsConnected
    {
        get => _isConnected;
        set => SetField(ref _isConnected, value);
    }

    // 修改DiStatus的实现
    private ObservableCollection<BoolWrapper> _diStatus;
    public ObservableCollection<BoolWrapper> DiStatus
    {
        get => _diStatus;
        set => SetField(ref _diStatus, value);
    }

    public MainViewModel()
    {
        _diStatus = new ObservableCollection<BoolWrapper>();
        // 初始化8个DI状态
        for (int i = 0; i < 8; i++)
        {
            _diStatus.Add(new BoolWrapper
            {
                Index = i,  
                Value = false
            });
        }

        SendCommand = new RelayCommand(async () => await SendDiStatusAsync());
        ResetCommand = new RelayCommand(ResetAllDi);
        ConnectCommand = new RelayCommand(async () => await ConnectAsync());
        DisconnectCommand = new RelayCommand(Disconnect);

        // 状态变化监听（调试用）
        DiStatus.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (BoolWrapper item in e.NewItems)
                {
                    item.PropertyChanged += (_, _) =>
                        Debug.WriteLine($"DI{item.Index + 1}状态变更为: {item.Value}");
                }
            }
        };

        // 添加属性变化通知
        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(DiStatus))
            {
                Debug.WriteLine("DiStatus集合发生变化");
                foreach (var (value, index) in DiStatus.Select((v, i) => (v, i)))
                {
                    Debug.WriteLine($"DI{index + 1}: {value}");
                }
            }
        };
    }

    public async Task SendDiStatusAsync()
    {
        var diStatus = CalculateDiByte();
        var message = new ModbusMessage
        {
            SlaveId = _config.SlaveId,
            DiStatus = diStatus
        };

        var data = message.ToByteArray();

        // 添加日志输出
        string hexString = BitConverter.ToString(data).Replace("-", " ");
        Debug.WriteLine($"发送数据: {hexString}");
        MessageBox.Show($"DI状态: {Convert.ToString(diStatus, 2).PadLeft(8, '0')}\n发送数据: {hexString}", "调试信息");

        await _tcpService.SendDataAsync(data);
    }

    private void ResetAllDi()
    {
        for (int i = 0; i < DiStatus.Count; i++)
        {
            DiStatus[i] = new BoolWrapper
            {
                Index = i,
                Value = false
            };
        }
    }

    private async Task ConnectAsync()
    {
        try
        {
            // 在连接前先断开现有连接
            if (_tcpService.IsConnected)
            {
                _tcpService.Disconnect();
            }

            // 确保IP和端口号有效
            if (string.IsNullOrWhiteSpace(_config.ServerIp))
            {
                MessageBox.Show("请输入有效的服务器IP地址", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 验证IP地址格式
            if (!IPAddress.TryParse(_config.ServerIp, out _))
            {
                MessageBox.Show("IP地址格式不正确", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await _tcpService.ConnectAsync(_config.ServerIp, _config.ServerPort);
            IsConnected = _tcpService.IsConnected;

            if (IsConnected)
            {
                MessageBox.Show($"成功连接到 {_config.ServerIp}:{_config.ServerPort}", "连接成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"连接失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            IsConnected = false;
        }
        finally
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    private void Disconnect()
    {
        _tcpService.Disconnect();
        IsConnected = false;
        CommandManager.InvalidateRequerySuggested(); // 刷新命令状态
    }

    private byte CalculateDiByte()
    {
        byte result = 0;
        byte[] diValues = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

        for (int i = 0; i < DiStatus.Count; i++)
        {
            if (DiStatus[i].Value)
            {
                result |= diValues[i];
            }
        }
        return result;
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    // 实现INotifyPropertyChanged和命令绑定
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // 删除 UpdateDiValue 方法，只保留 UpdateDiStatus
    //public void UpdateDiStatus(int index, bool value)
    //{
    //    if (index >= 0 && index < DiStatus.Count)
    //    {
    //        DiStatus[index] = value;
    //        OnPropertyChanged(nameof(DiStatus));  // 添加属性变更通知
    //        Debug.WriteLine($"更新DI{index + 1}状态为: {value}");
    //    }
    //}
}


// BoolWrapper.cs
public class BoolWrapper : INotifyPropertyChanged
{
    private bool _value;
    public bool Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged();
            }
        }
    }

    public int Index { get; init; } // 用于显示DI编号

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}