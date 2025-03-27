// Models/AppConfig.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class AppConfig : INotifyPropertyChanged
{
    private string _serverIp = "192.168.1.107";
    private int _slaveId = 17;

    public string ServerIp
    {
        get => _serverIp;
        set => SetField(ref _serverIp, string.IsNullOrWhiteSpace(value) ? "192.168.1.107" : value);
    }

    public int ServerPort { get; set; } = 5030;

    public int SlaveId
    {
        get => _slaveId;
        set => SetField(ref _slaveId, value);
    }

    // INotifyPropertyChanged 实现
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

