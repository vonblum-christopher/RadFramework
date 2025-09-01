namespace RadFramework.Libraries.Telemetry.Channel;

public interface IRequest<TResponse>
{
    TResponse Response { get; }
}