using Microsoft.Extensions.DependencyInjection;

namespace FluentSample.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsSerial(this IServiceCollection services) => services.AddSingleton<ISerialPort, SerialPort>();
    }
}