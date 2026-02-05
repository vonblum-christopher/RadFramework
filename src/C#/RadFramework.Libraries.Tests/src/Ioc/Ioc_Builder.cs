using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Tests.Ioc;

public partial class Ioc_Builder
{
    [Test]
    public void OneBuilderManyContainers()
    {
        IocContainerBuilder<Type> builder = 
            new IocContainerBuilder<Type>()
                .RegisterTransient(typeof(TestDep1))
                .RegisterSingleton(typeof(TestDep2));
        
        TypeOnlyIocContainer container = new TypeOnlyIocContainer(builder);

        TestDep1 res = container.Resolve<TestDep1>();
        
        TypeOnlyIocContainer container2 = new TypeOnlyIocContainer(builder);

        TestDep1 res2 = container2.Resolve<TestDep1>();
        
        Assert.That(res != res2);
        Assert.That(container != container2);
        Assert.That(container.BuilderRegistry != container2.BuilderRegistry);
    }
}