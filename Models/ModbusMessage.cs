// Models/ModbusMessage.cs
public class ModbusMessage
{
    private int _slaveId;
    public int SlaveId
    {
        get => _slaveId;
        set => _slaveId = value;
    }
    public byte FunctionCode => 0x42;
    public byte[] FixedHeader => new byte[] { 0x00, 0x20, 0x00, 0x04, 0x01 };
    public byte DiStatus { get; set; }

    public byte[] ToByteArray()
    {
        var data = new List<byte>
        {
            (byte)SlaveId,
            FunctionCode
        };
        data.AddRange(FixedHeader);
        data.Add(DiStatus);

        var crc = CrcCalculator.CalculateModbusCrc(data.ToArray());
        data.AddRange(crc);
        return data.ToArray();
    }
}