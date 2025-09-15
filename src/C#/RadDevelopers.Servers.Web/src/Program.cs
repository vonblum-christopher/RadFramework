using System.Net;
using System.Net.Sockets;
using RadDevelopers.Servers.Web.Config;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Caching;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Logging;
using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Builder;
using RadFramework.Libraries.Serialization.Json;
using RadFramework.Libraries.Web;
using RadFramework.Libraries.Web.Models;
using IocContainer = RadFramework.Libraries.Ioc.IocContainer;

namespace RadDevelopers.Servers.Web
{
    public static class Program
    {
        /// <summary>
        /// main entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            IocContainerBuilder iocBuilder = new();
            
            iocBuilder.RegisterSingleton<IContractSerializer, JsonContractSerializer>();
            
            iocBuilder.RegisterSingleton<HttpServerContext>();
            
            new WebApplicationBase().Setup(iocBuilder);
            
            iocBuilder.RegisterSemiAutomaticSingleton<PipelineDrivenHttpServer>(iocContainer =>
            {
                return new PipelineDrivenHttpServer(
                    new List<IPEndPoint>()
                    {
                        IPEndPoint.Parse("127.0.0.1:80")
                    },
                    iocContainer
                        .Resolve<ExtensionPipeline<HttpConnection, HttpConnection>>(),
                    iocContainer
                        .Resolve<ExtensionPipeline<HttpError, HttpError>>(),
                    iocContainer
                        .Resolve<ExtensionPipeline<(HttpConnection connection, Socket socket), (HttpConnection connection, Socket socket)>>(),
                    iocContainer.Resolve<HttpServerContext>());
            });
            
            IocContainer container = iocBuilder.CreateContainer();
            
            var server = container.Resolve<PipelineDrivenHttpServer>();
            
            ManualResetEvent shutdownEvent = new ManualResetEvent(false);
            
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                server.Dispose();
                shutdownEvent.Set();
            };
            
            shutdownEvent.WaitOne();
        }

    }
}