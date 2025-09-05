using System.Text;

namespace RadFramework.Tools.CompilerExtension;

public class HookInTask
{
    
}
public class JustWriteANamespaceGenerator: ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        // Generate a namespace
        var newTree = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("ExampleNamespace"));
     
        // Turn the namespace into text
        var source = newTree.NormalizeWhitespace().ToFullString();
        // Output it into the compilation process
        context.AddSource($"ExampleNamespace.g.cs", SourceText.From(source, Encoding.UTF8));
    }
    
    public void Initialize(GeneratorInitializationContext context)
    {
    }
}