using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32.SafeHandles;
using Mono.Cecil;
using RewritingApi.middleware;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //RuntimeMetadataSerializationStorage storage = new RuntimeMetadataSerializationStorage(typeof(ITestDto).Assembly);

            ;
        }
        
        [Fact]
        public void Rewrite()
        {
            var asm = AssemblyDefinition.ReadAssembly("/home/anon/Documents/repos/RewritingNetCore/TestTarget/obj/Debug/netcoreapp3.1/", new ReaderParameters(ReadingMode.Immediate));
            ;
        }
    }
}