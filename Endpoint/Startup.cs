using CoreWCF;
using CoreWCF.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Endpoint;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal sealed class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddServiceModelWebServices(o =>
		{
			o.Title = "SpaceBattle API";
			o.Version = "1.0";
			o.Description = "Sends commands to game objects";
		});
		services.AddSingleton(new SwaggerOptions());
	}
	public void Configure(IApplicationBuilder app)
	{
		app.UseMiddleware<SwaggerMiddleware>();
		app.UseSwaggerUI();
		app.UseServiceModel(builder =>
		{
			builder.AddService<EndPoint>();
			builder.AddServiceWebEndpoint<EndPoint, IEndPoint>(new WebHttpBinding
			{
				MaxReceivedMessageSize = 5242880,
				MaxBufferSize = 65536,
			}, "api", behavior =>
			{
				behavior.HelpEnabled = true;
				behavior.AutomaticFormatSelectionEnabled = true;
			});
		});
	}
}
