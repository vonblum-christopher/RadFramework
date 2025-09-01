using System.Collections.Generic;

namespace RewritingNetCore.Compiler.Contracts
{
    public class CompilerResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }
}