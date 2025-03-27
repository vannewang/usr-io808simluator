// Services/CrcCalculator.cs
public static class CrcCalculator
{
    public static byte[] CalculateModbusCrc(byte[] data)
    {
        ushort crc = 0xFFFF;

        foreach (byte b in data)
        {
            crc ^= b;
            for (int i = 0; i < 8; i++)
            {
                bool lsb = (crc & 0x0001) != 0;
                crc >>= 1;
                if (lsb) crc ^= 0xA001;
            }
        }
        return new[] { (byte)(crc & 0xFF), (byte)(crc >> 8) };
    }
}