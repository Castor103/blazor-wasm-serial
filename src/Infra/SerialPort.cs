using FluentSample.Infra.Enums;
using FluentSample.Infra.Exceptions;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace FluentSample.Infra
{
    public class SerialPort : ISerialPort
    {
        public bool IsConnected { get; private set; }
        public bool IsPortChosen { get; private set; }

        private readonly IJSRuntime _jsRuntime;

        public SerialPort(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> IsSupported() => await _jsRuntime.InvokeAsync<bool>("jsSerialIsSupported");

        public async Task<RequestPortResponseEnum> RequestPort()
        {
            var result = Enum.Parse<RequestPortResponseEnum>(await _jsRuntime.InvokeAsync<string>("jsSerialGetPort"));

            if (result == RequestPortResponseEnum.Ok)
            {
                IsPortChosen = true;
            }

            return result;
        }

        public async Task<ConnectResponseEnum> Open(int baudRate)
        {
            if (!IsPortChosen)
            {
                throw new PortNotChoosenException();
            }

            if (IsConnected)
            {
                throw new AlreadyConnectedException();
            }

            var connectionResult = Enum.Parse<ConnectResponseEnum>(await _jsRuntime.InvokeAsync<string>("jsSerialOpen", baudRate));

            if (connectionResult == ConnectResponseEnum.Ok)
            {
                IsConnected = true;
            }

            return connectionResult;
        }

        public async Task Close()
        {
            if (IsConnected)
            {
                await _jsRuntime.InvokeVoidAsync("jsSerialClose");
                IsConnected = false;
            }
        }

        public async Task CloseForce()
        {
            await _jsRuntime.InvokeVoidAsync("jsSerialClose");
            IsConnected = false;
        }

        public async Task Write(string text) => await _jsRuntime.InvokeAsync<string>("jsSerialWriteText", text);

        public async Task Write(byte[] data) => await _jsRuntime.InvokeAsync<byte[]>("jsSerialWriteBinary", data);

    }
}