using RadFramework.Libraries.Ioc;

namespace RadFramework.Libraries.Abstractions
{
    public interface ITypeOnlyIocContainer : IIocContainer<Type>, IServiceProvider
    {
    }
}