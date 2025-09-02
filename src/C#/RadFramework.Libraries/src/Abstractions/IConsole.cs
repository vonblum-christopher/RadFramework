namespace RadFramework.Libraries.Abstractions.Console
{
    public interface IConsole
    {
        string ReadLine();
        void WriteLine(string value);
    }
}