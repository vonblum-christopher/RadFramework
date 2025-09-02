using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;
using IocContainer = RadFramework.Libraries.Ioc.Core.IocContainer;

namespace RadFramework.Libraries.Tests;

public class Ioc_Factory
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RegisterAndResolveIocKeyInAnyConstellation()
    {
        IocContainer container = new IocContainer();
        
        container.RegisterTransient<A>();
        container.RegisterTransient<B>();
        
        ServiceFactoryLambdaGenerator serviceFactoryLambdaGenerator = new ServiceFactoryLambdaGenerator();
        
        IocKey onlyType = new IocKey()
        {
            KeyType = typeof(B),
            KeyString = null
        };

        var Binstance = container.Resolve(onlyType);
        
        //serviceFactoryLambdaGenerator.CreateInstanceFactoryMethod()
    }
}

public class A
{
}

public class B
{
    private readonly A a;

    public B(A a)
    {
        this.a = a;
    }
}