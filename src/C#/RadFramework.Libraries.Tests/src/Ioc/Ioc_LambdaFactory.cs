using RadFramework.Libraries.Ioc.Builder;
using IocContainer = RadFramework.Libraries.Ioc.IocContainer;

namespace RadFramework.Libraries.Tests.Ioc;

public class Ioc_Factory
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RegisterAndResolveIocKeyInAnyConstellation()
    {
        IocContainerBuilder builder = new IocContainerBuilder();
        
        builder.RegisterTransient<A>();
        builder.RegisterTransient<B>();

        var container = new IocContainer(builder);

        B Binstance = container.Resolve<B>();
        
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