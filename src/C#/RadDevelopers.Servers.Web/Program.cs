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

            PipelineBuilder httpPipeineBuilder = new PipelineBuilder();
            
            ExtensionPipeline<HttpConnection, HttpConnection> httpPipeline =
                    new ExtensionPipeline<HttpConnection, HttpConnection>(httpPipeineBuilder, );
            
            PipelineBuilder httpErrorPipeineBuilder = new PipelineBuilder();
            
            ExtensionPipeline<HttpError, HttpError> httpErrorPipeline =
                    new ExtensionPipeline<HttpError, HttpError>(httpErrorPipeineBuilder, );
            
            // when a web socket connection gets established this class takes care of the socket connection
            /*iocContainer.RegisterSingleton<TelemetrySocketManager>();

            TelemetrySocketManager socketManager = iocContainer.Resolve<TelemetrySocketManager>();*/
            
            // the server that passes the requests to the pipelines
            HttpServerWithPipeline pipelineDrivenHttpServer = new HttpServerWithPipeline(
                80,
                new HttpServerEvents()
                {
                    OnRequest = connection => httpPipeline.Process(connection),
                    OnError = error => httpErrorPipeline.Process(error),
                    OnFatalError = error => httpErrorPipeline.Process(error),
                    OnWebsocketConnected = con => socketManager.RegisterNewClientSocket(socket))
                });
            
            ManualResetEvent shutdownEvent = new ManualResetEvent(false);
            
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                pipelineDrivenHttpServer.Dispose();
                shutdownEvent.Set();
            };
            
            shutdownEvent.WaitOne();
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
        private static PipelineBuilder LoadHttpPipelineConfig(string configFilePath)
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