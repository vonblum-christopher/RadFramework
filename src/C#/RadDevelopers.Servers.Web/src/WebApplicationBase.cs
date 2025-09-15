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

namespace RadDevelopers.Servers.Web;

public class WebApplicationBase
{
    public void Setup(IocContainerBuilder iocBuilder)
    {
        SetupIocContainer(iocBuilder);
        
        BuildHttpPipeline(iocBuilder);

        BuildHttpErrorPipeline(iocBuilder);
            
        BuildWebsocketConnectedPipeline(iocBuilder); 
    }
    
    private static void BuildWebsocketConnectedPipeline(IocContainerBuilder containerBuilder)
    {
        containerBuilder
            .RegisterSemiAutomaticSingleton<
                ExtensionPipeline<(HttpConnection connection, Socket socket), (HttpConnection connection, Socket socket)>>(
                    iocContainer => 
                    new ExtensionPipeline<(HttpConnection connection, Socket socket), (HttpConnection connection, Socket socket)>(
                        LoadPipelineConfig("cfg/HttpErrorPipelineConfig.json"), iocContainer));
    }

    private static void BuildHttpErrorPipeline(IocContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterSemiAutomaticSingleton<ExtensionPipeline<HttpError, HttpError>>(iocContainer => 
            new ExtensionPipeline<HttpError, HttpError>(
                LoadPipelineConfig("cfg/HttpErrorPipelineConfig.json"), iocContainer));
    }

    private static void BuildHttpPipeline(IocContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterSemiAutomaticSingleton<ExtensionPipeline<HttpConnection, HttpConnection>>(iocContainer => 
            new ExtensionPipeline<HttpConnection, HttpConnection>(
            LoadPipelineConfig("cfg/HttpPipelineConfig.json"), iocContainer));
    }


    /// <summary>
    /// Registers all dependencies from the IocContainer config
    /// </summary>
    /// <param name="iocBuilder"></param>
    private static void SetupIocContainer(IocContainerBuilder iocBuilder)
    {
        IocContainerConfig config = (IocContainerConfig)JsonContractSerializer.Instance.Deserialize(
            typeof(IocContainerConfig),
            File.ReadAllBytes("cfg/IocContainerConfig.json"));

        foreach (var iocRegistration in config.Registrations)
        {
            if (iocRegistration.Lifecycle == IocLifecycles.Singleton)
            {
                iocBuilder.RegisterSingleton(
                    Type.GetType(iocRegistration.TKey), 
                    Type.GetType(iocRegistration.TImplementation));
                continue;
            }
            
            iocBuilder.RegisterTransient(
                Type.GetType(iocRegistration.TKey), 
                Type.GetType(iocRegistration.TImplementation));
        }
        
        iocBuilder.RegisterSingleton<ISimpleCache, SimpleCache>();
        
        iocBuilder.RegisterSemiAutomaticSingleton<ILogger>(container =>
            new StandardLogger(
                new ILoggerSink[]
                {
                    new ConsoleLogger(),
                    new FileLogger("logs")
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