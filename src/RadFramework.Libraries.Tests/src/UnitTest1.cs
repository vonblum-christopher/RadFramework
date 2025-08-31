using RadFramework.Libraries.Ioc;

namespace RadFramework.Libraries.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        IocContainer container = new IocContainer();

        container.RegisterSingleton<TypeA>();
        container.RegisterTransient<TypeB>();
        
        Assert.Pass();
    }

    public class TypeA
    {
        [IocNamedDependency]
        public TypeB b { get; set; }
    }
    public class TypeB
    {
        
    }
    public class TypeC
    {
        
    }
    public class TypeD
    {
        
    }
}