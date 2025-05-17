using System.Net;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace Endpoint
{
	[ServiceContract]
	[OpenApiBasePath("/api")]
	internal interface IEndPoint
	{
		[OperationContract]
		[WebInvoke(Method = "POST", UriTemplate = "/command")]
		[OpenApiTag("Tag")]
		[OpenApiResponse(ContentTypes = new[] { "application/json" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(Message))]
		string HttpCommand(
			[OpenApiParameter(ContentTypes = new[] { "application/json" }, Description = "param description.")]
			Message param
		);
	}
}
