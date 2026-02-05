namespace RadFramework.Libraries.Tests.Ioc;

public class TestDep1
{
    public TestDep2 Dep2 { get; set; }
    private readonly TestDep2 dep2;

    public TestDep1(TestDep2 dep2)
    {
        this.dep2 = dep2;
    }

    private TestDep2 fromMethod;

    public void InjectHere(TestDep2 dep2)
    {
        fromMethod = dep2;
    }
}