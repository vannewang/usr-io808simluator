# IO控制器模拟器 - 使用说明文档

## 目录
- [项目概述](#项目概述)
- [功能特性](#功能特性)
- [系统要求](#系统要求)
- [安装指南](#安装指南)
- [使用说明](#使用说明)
- [开发指南](#开发指南)
- [常见问题](#常见问题)
- [协议说明](#协议说明)

## 项目概述
本项目是一个基于WPF开发的IO控制器模拟软件，主要用于模拟具有8个数字输入(DI)通道的工业控制器设备。通过标准的Modbus-TCP协议与服务器进行通信，支持DI状态监控、远程触发和数据上报功能。

## 功能特性
### 核心功能
- 8路独立DI通道控制
- 多通道同时触发/复位
- 实时状态可视化展示
- 双向数据绑定

### 通信协议
- 基于Modbus-TCP协议扩展
- 自定义报文格式：从机ID(1B) | 功能码0x42(1B) | 固定头(5B) | DI状态(1B) | CRC(2B)


### 配置管理
- 服务器IP/端口配置
- 从机ID设置(1-255)
- 连接状态指示

## 系统要求
### 最低配置
| 组件 | 要求 |
|------|------|
| 操作系统 | Windows 10 64位 |
| .NET版本 | .NET 8.0 Desktop Runtime |
| 内存 | 1GB以上 |
| 磁盘空间 | 50MB可用空间 |

### 推荐配置
| 组件 | 建议 |
|------|------|
| 操作系统 | Windows 11 22H2 |
| CPU | 四核2.0GHz+ |
| 内存 | 4GB+ |
| 网络 | 千兆以太网 |

### 源码编译
```bash
# 克隆仓库
git clone https://github.com/your-repo/io-controller-simulator.git

# 还原NuGet包
dotnet restore

# 编译发布版本
dotnet publish -c Release -o ./publish

### 项目结构
src/
├── Models/
│   ├── ModbusMessage.cs
│   └── AppConfig.cs
├── Services/
│   ├── TcpClientService.cs
│   └── CrcCalculator.cs
├── ViewModels/
│   └── MainViewModel.cs
├── Views/
│   └── MainWindow.xaml
└── Converters/
    ├── DiIndexConverter.cs
    └── BoolToStatusConverter.cs

