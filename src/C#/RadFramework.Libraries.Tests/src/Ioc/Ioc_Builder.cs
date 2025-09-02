using NUnit.Framework.Internal;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Tests;

public class Ioc_Builder
{
    [Test]
    public void OneBuilderManyContainers()
    {
        IocContainerBuilder builder = 
            new IocContainerBuilder()
                .RegisterTransient(typeof(Test))
                .RegisterSingleton(typeof(TestDep));
        
        IocContainer container = new IocContainer(builder);

        Test res = container.Resolve<Test>();
        
        IocContainer container2 = new IocContainer(builder);

        Test res2 = container2.Resolve<Test>();
        
        Assert.That(res != res2);
        Assert.That(container != container2);
        Assert.That(container.Registry != container2.Registry);
    }
    
    public class Test
    {
        public TestDep Dep { get; set; }
        private readonly TestDep dep;

        public Test(TestDep dep)
        {
            this.dep = dep;
        }

        private TestDep fromMethod;

        public void InjectHere(TestDep dep)
        {
            fromMethod = dep;
        }
    }

    public class TestDep
    {
        public TestDep()
        {
        }
    }
}