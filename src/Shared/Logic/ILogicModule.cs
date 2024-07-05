namespace FluentSample.Shared.Logic;

public interface ILogicModule
{
    bool IsOn { get; }

    Task Loop();
    Task<bool> Start();
    Task<bool> Stop();
    Task Reset();

    Task SetManual(string cmd);
    Task<byte[]> GetResponse(byte[] cmd);
    Task<byte[]> GetAsyncResponse();
    Task<string> GetResponseString(string cmd);
    Task<string> GetAsyncResponseString();
}
