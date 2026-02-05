using System.Net.Mime;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Tests.Ioc;

public class Ioc_Container
{
    private TypeOnlyIocContainer Container;

    [SetUp]
    public void Setup()
    {
        
    }

    public void ResolveTransient()
    {
        Container = new TypeOnlyIocContainer(
            new IocContainerBuilder()
                .RegisterTransient<TestDep1>()
                .RegisterTransient<TestDep2>());

        TestDep1 dep1 = Container.Resolve<TestDep1>();
        
        TestDep2 dep2 = Container.Resolve<TestDep2>();
        
        Assert.That(dep1.Dep2 != dep2);
        Assert.That(dep1.Dep2 != dep2);
    }
}