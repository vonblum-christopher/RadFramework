using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;

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

        var container = new TypeOnlyIocContainer(builder);

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