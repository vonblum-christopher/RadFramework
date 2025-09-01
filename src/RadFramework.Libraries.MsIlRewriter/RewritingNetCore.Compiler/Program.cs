using Newtonsoft.Json;
using RewritingNetCore.Compiler.Contracts;

namespace RewritingNetCore.Compiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string inputJsonPath = args[0];

            string outputJsonPath = args[1];

            CompilerArgs arguments = JsonConvert
                                        .DeserializeObject<CompilerArgs>(
                                            File.ReadAllText(inputJsonPath));
            
            File.WriteAllText(
                outputJsonPath, 
                JsonConvert.SerializeObject(
                    new MiddlewareRunner()
                        .RunMiddlewarePlugins(arguments)));
        }
    }
}