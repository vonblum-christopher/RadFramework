using RadFramework.Libraries.DataTypes;

namespace RadFramework.Libraries.Threading.ThreadPools.Internals
{
    public interface IThreadPoolMixinsConsumer
    {
        Action<Thread, Exception> OnError { get; }
        ThreadPriority ProcessingThreadPriority { get; }
        string ThreadDescription { get; }
        ObjectReferenceRegistry<PoolThread> ProcessingThreadRegistry { get; }
    }
}