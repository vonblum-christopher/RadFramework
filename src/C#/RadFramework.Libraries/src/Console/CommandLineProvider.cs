using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Console
{
    public class CommandLineProvider : IConsole
    {
        public string ReadLine()
        {
            return System.Console.ReadLine();
        }

        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }
    }
}