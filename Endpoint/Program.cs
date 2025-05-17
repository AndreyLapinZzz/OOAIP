using Microsoft.AspNetCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;

namespace Endpoint;
/// <summary>
/// Program
/// </summary>
[ExcludeFromCodeCoverage]
public class Program
{
    static void Main(string[] args)
    {
        IWebHostBuilder builder = WebHost.CreateDefaultBuilder(args)
        .UseKestrel(options =>
        {
            options.ListenAnyIP(8080);
        })
        .UseStartup<Startup>();
        IWebHost app = builder.Build();
        app.Run();
    }
}
