using System.Text;
using Microsoft.Build.Framework;

namespace RadFramework.Tools.CompilerExtension;

public class HookInTask : ITask
{    
    public IBuildEngine BuildEngine { get; set; }
    public ITaskHost HostObject { get; set; }
    
    public bool Execute()
    {
        throw new NotImplementedException();
    }
}