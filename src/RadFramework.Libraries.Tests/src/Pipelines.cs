using RadFramework.Libraries.Abstractions.Console;
using RadFramework.Libraries.GenericUi.Console.Interaction;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Core;
using IocContainer = RadFramework.Libraries.Ioc.Core.IocContainer;

namespace RadFramework.Libraries.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void GenericUiConsole()
    {
        ConsoleInteractionProvider prov = new ConsoleInteractionProvider(new CommandLineProvider());
        
        prov.RenderService(typeof(TypeA), new TypeA());
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