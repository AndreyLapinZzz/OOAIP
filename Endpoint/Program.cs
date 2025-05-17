using Microsoft.AspNetCore;
using CoreWCF;
using CoreWCF.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace Endpoint;
/// <summary>
/// comment
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
