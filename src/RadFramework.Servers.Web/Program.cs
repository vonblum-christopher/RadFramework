using System.Reflection;
using RadFramework.Libraries.Caching;
using RadFramework.Libraries.Extensibility.Pipeline;
using RadFramework.Libraries.Extensibility.Pipeline.Synchronous;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Logging;
using RadFramework.Libraries.Net.Http;
using RadFramework.Libraries.Net.Socket;
using RadFramework.Libraries.Serialization;
using RadFramework.Libraries.Serialization.Json.ContractSerialization;
using RadFramework.Servers.Web.Config;

namespace RadFramework.Servers.Web
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

            // the two core pipelines of the webserver
            PipelineDefinition httpPipelineDefinition = LoadHttpPipelineConfig("Config/HttpPipelineConfig.json");
            PipelineDefinition httpErrorPipelineDefinition = LoadHttpPipelineConfig("Config/HttpErrorPipelineConfig.json");            
            
            iocContainer.RegisterSingleton<IContractSerializer, JsonContractSerializer>();
            
            // when a web socket connection gets established this class takes care of the socket connection
            iocContainer.RegisterSingleton<TelemetrySocketManager>();

            TelemetrySocketManager socketManager = iocContainer.Resolve<TelemetrySocketManager>();
            
            // the server that passes the requests to the pipelines
            HttpServerWithPipeline pipelineDrivenHttpServer = new HttpServerWithPipeline(
                80,
                httpPipelineDefinition,
                httpErrorPipelineDefinition,
                iocContainer,
                (request, socket) => socketManager.RegisterNewClientSocket(socket));
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            
            pipelineDrivenHttpServer.Dispose();
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
        private static PipelineDefinition LoadHttpPipelineConfig(string configFilePath)
        {
            PipelineDefinition httpPipelineDefinition = new();

            HttpPipelineConfig config = (HttpPipelineConfig)JsonContractSerializer.Instance.Deserialize(
                typeof(HttpPipelineConfig),
                File.ReadAllBytes(configFilePath));
            
            config
                .Pipes
                .ToList()
                .ForEach(pipeType => 
                    httpPipelineDefinition.
                        Append(Type.GetType(pipeType)));

            return httpPipelineDefinition;
        }
    }
}