using FluentSample.Infra.Enums;
using System;
using System.Threading.Tasks;

namespace FluentSample.Infra
{
    public interface ISerialPort
    {
        bool IsConnected { get; }
        bool IsPortChosen { get; }

        Task<ConnectResponseEnum> Open(int baudRate);
        Task<bool> IsSupported();
        Task<RequestPortResponseEnum> RequestPort();
        Task Write(string text);
        Task Write(byte[] data);
        Task Close();
        Task CloseForce();
    }
}