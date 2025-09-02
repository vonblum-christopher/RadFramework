using RadFramework.Libraries.Console;
using RadFramework.Libraries.GenericUi.Console.Interaction;
using RadFramework.Libraries.Ioc;
using IocContainer = RadFramework.Libraries.Ioc.IocContainer;

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
        Assert.Pass();
    }

    public class TypeA
    {
        [IocDependency]
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