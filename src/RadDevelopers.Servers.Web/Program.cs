using RadDevelopers.Servers.Web.Config;
using RadDevelopers.Servers.Web.Pipelines.HttpError;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Caching;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Logging;
using RadFramework.Libraries.Pipelines.Builder;
using RadFramework.Libraries.Serialization;
using RadFramework.Libraries.Serialization.Json;
using RadFramework.Libraries.Web;
using IocContainer = RadFramework.Libraries.Ioc.Core.IocContainer;

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
            // the ioc container that wires everything up.
            IocContainer iocContainer = new IocContainer();
            
            SetupIocContainer(iocContainer);

            SetupPipelines(iocContainer);
            
            // build and register HttPPipeline
            PipelineBuilder httpPipelineBuilder = LoadHttpPipelineConfig("Config/HttpPipelineConfig.json");
            
            iocContainer.RegisterSingletonInstance<IHttpPipe>(new ExtensionPipeline<IHttpPipe>(httpPipelineBuilder, iocContainer));
            
            
            PipelineBuilder httpErrorPipelineBuilder = LoadHttpPipelineConfig("Config/HttpErrorPipelineConfig.json");            

            iocContainer.RegisterSingletonInstance<IHttpErrorPipeline>(new ExtensionPipeline<HttpConnection>(httpPipelineBuilder, iocContainer));
            
            iocContainer.RegisterSingleton<IContractSerializer, JsonContractSerializer>();
            
            // when a web socket connection gets established this class takes care of the socket connection
            /*iocContainer.RegisterSingleton<TelemetrySocketManager>();

            TelemetrySocketManager socketManager = iocContainer.Resolve<TelemetrySocketManager>();*/
            
            // the server that passes the requests to the pipelines
            HttpServerWithPipeline pipelineDrivenHttpServer = new HttpServerWithPipeline(
                80,
                httpPipelineBuilder,
                httpErrorPipelineBuilder,
                iocContainer);
                //(request, socket) => socketManager.RegisterNewClientSocket(socket));
            
            ManualResetEvent shutdownEvent = new ManualResetEvent(false);
            
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                //socketManager.Dispose();
                pipelineDrivenHttpServer.Dispose();
                shutdownEvent.Set();
            };
            
            shutdownEvent.WaitOne();
        }

        private static void SetupPipelines(IocContainer iocContainer)
        {
            iocContainer.
        }

        /// <summary>
        /// Registers all dependencies from the IocContainer config
        /// </summary>
        /// <param name="iocContainer"></param>
        private static void SetupIocContainer(IocContainer iocContainer)
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
            
            
            iocContainer.RegisterSingletonInstance<ISimpleCache>(new SimpleCache());
            iocContainer.RegisterSingletonInstance<ILogger>(
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