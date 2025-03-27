# IO������ģ���� - ʹ��˵���ĵ�

## Ŀ¼
- [��Ŀ����](#��Ŀ����)
- [��������](#��������)
- [ϵͳҪ��](#ϵͳҪ��)
- [��װָ��](#��װָ��)
- [ʹ��˵��](#ʹ��˵��)
- [����ָ��](#����ָ��)
- [��������](#��������)
- [Э��˵��](#Э��˵��)

## ��Ŀ����
����Ŀ��һ������WPF������IO������ģ���������Ҫ����ģ�����8����������(DI)ͨ���Ĺ�ҵ�������豸��ͨ����׼��Modbus-TCPЭ�������������ͨ�ţ�֧��DI״̬��ء�Զ�̴����������ϱ����ܡ�

## ��������
### ���Ĺ���
- 8·����DIͨ������
- ��ͨ��ͬʱ����/��λ
- ʵʱ״̬���ӻ�չʾ
- ˫�����ݰ�

### ͨ��Э��
- ����Modbus-TCPЭ����չ
- �Զ��屨�ĸ�ʽ���ӻ�ID(1B) | ������0x42(1B) | �̶�ͷ(5B) | DI״̬(1B) | CRC(2B)


### ���ù���
- ������IP/�˿�����
- �ӻ�ID����(1-255)
- ����״ָ̬ʾ

## ϵͳҪ��
### �������
| ��� | Ҫ�� |
|------|------|
| ����ϵͳ | Windows 10 64λ |
| .NET�汾 | .NET 8.0 Desktop Runtime |
| �ڴ� | 1GB���� |
| ���̿ռ� | 50MB���ÿռ� |

### �Ƽ�����
| ��� | ���� |
|------|------|
| ����ϵͳ | Windows 11 22H2 |
| CPU | �ĺ�2.0GHz+ |
| �ڴ� | 4GB+ |
| ���� | ǧ����̫�� |

### Դ�����
```bash
# ��¡�ֿ�
git clone https://github.com/your-repo/io-controller-simulator.git

# ��ԭNuGet��
dotnet restore

# ���뷢���汾
dotnet publish -c Release -o ./publish

### ��Ŀ�ṹ
src/
������ Models/
��   ������ ModbusMessage.cs
��   ������ AppConfig.cs
������ Services/
��   ������ TcpClientService.cs
��   ������ CrcCalculator.cs
������ ViewModels/
��   ������ MainViewModel.cs
������ Views/
��   ������ MainWindow.xaml
������ Converters/
    ������ DiIndexConverter.cs
    ������ BoolToStatusConverter.cs

