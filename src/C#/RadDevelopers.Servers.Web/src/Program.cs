using System.Net;
using System.Net.Sockets;
using RadDevelopers.Servers.Web.Config;
using RadDevelopers.Servers.Web.Pipelines.Definitions;
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
            
            SetupIocContainer(iocBuilder);
            
            iocBuilder.RegisterSingleton<IContractSerializer, JsonContractSerializer>();
            
            new Application().Setup(iocBuilder);

            IocContainer container = iocBuilder.CreateContainer();
            
            var httpPipeline = BuildHttpPipeline(container);

            var httpErrorPipeline = BuildHttpErrorPipeline(container);
            
            var websocketConnectedPipeline = BuildWebsocketConnectedPipeline(container);
            
            PipelineDrivenHttpServer pipelineDrivenHttpServer = new PipelineDrivenHttpServer(
                new List<IPEndPoint>()
                {
                    IPEndPoint.Parse("127.0.0.1:80")
                },
                httpPipeline,
                httpErrorPipeline, 
                websocketConnectedPipeline);
            
            ManualResetEvent shutdownEvent = new ManualResetEvent(false);
            
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                pipelineDrivenHttpServer.Dispose();
                shutdownEvent.Set();
            };
            
            shutdownEvent.WaitOne();
        }

        private static ExtensionPipeline<(HttpConnection connection, Socket socket), (HttpConnection connection, Socket socket)> BuildWebsocketConnectedPipeline(IocContainer container)
        {
            var websocketConnectedPipeline = new ExtensionPipeline<(HttpConnection connection, Socket socket), (HttpConnection connection, Socket socket)>(
                LoadPipelineConfig(""), container);
            return websocketConnectedPipeline;
        }

        private static ExtensionPipeline<HttpError, HttpError> BuildHttpErrorPipeline(IocContainer container)
        {
            PipelineBuilder httpErrorPipeineBuilder = new PipelineBuilder();

            ExtensionPipeline<HttpError, HttpError> httpErrorPipeline =
                new ExtensionPipeline<HttpError, HttpError>(
                    LoadPipelineConfig("Config/HttpErrorPipelineConfig.json"), container);
            return httpErrorPipeline;
        }

        private static ExtensionPipeline<HttpConnection, HttpConnection> BuildHttpPipeline(IocContainer container)
        {
            PipelineBuilder httpPipelineBuilder = new PipelineBuilder();

            ExtensionPipeline<HttpConnection, HttpConnection> httpPipeline =
                new ExtensionPipeline<HttpConnection, HttpConnection>(
                    LoadPipelineConfig("Config/HttpPipelineConfig.json"), container);
            return httpPipeline;
        }


        /// <summary>
        /// Registers all dependencies from the IocContainer config
        /// </summary>
        /// <param name="iocContainer"></param>
        private static void SetupIocContainer(IocContainerBuilder iocContainer)
        {
            IocContainerConfig config = (IocContainerConfig)JsonContractSerializer.Instance.Deserialize(
                typeof(IocContainerConfig),
                File.ReadAllBytes("Config/IocContainerConfig.json"));

            foreach (var iocRegistration in config.Registrations)
            {
                if (iocRegistration.Singleton)
                {
                    iocContainer.RegisterSingleton(
                        Type.GetType(iocRegistration.TKey), 
                        Type.GetType(iocRegistration.TImplementation));
                    continue;
                }
                
                iocContainer.RegisterTransient(
                    Type.GetType(iocRegistration.TKey), 
                    Type.GetType(iocRegistration.TImplementation));
            }
            
            iocContainer.RegisterSingleton<ISimpleCache, SimpleCache>();
            
            iocContainer.RegisterSemiAutomaticSingleton<ILogger>(container =>
                new StandardLogger(
                    new ILoggerSink[]
                    {
                        new ConsoleLogger(),
                        new FileLogger("Logs")
                    }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFilePath"></param>
        /// <returns></returns>
        private static PipelineBuilder LoadPipelineConfig(string configFilePath)
        {
            PipelineBuilder httpPipelineBuilder = new();

            HttpPipelineConfig config = (HttpPipelineConfig)JsonContractSerializer.Instance.Deserialize(
                typeof(HttpPipelineConfig),
                File.ReadAllBytes(configFilePath));

            var types = config
                .Pipes
                .ToList();
            
            foreach (var pipeType in types) 
            {
                httpPipelineBuilder
                    .Append(Type.GetType(pipeType));
            }

            return httpPipelineBuilder;
        }
    }
}