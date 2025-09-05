using System.Text;
using Microsoft.Build.Framework;

namespace RadFramework.Tools.CompilerExtension;

public class HookInTask : ITask
{
    public bool Execute()
    {
        throw new NotImplementedException();
    }

    public IBuildEngine BuildEngine { get; set; }
    public ITaskHost HostObject { get; set; }
}