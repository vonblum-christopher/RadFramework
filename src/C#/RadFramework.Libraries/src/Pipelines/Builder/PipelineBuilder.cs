using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Pipelines.Builder
{
    public class PipelineBuilder : ICloneable<PipelineBuilder>
    {
        public IEnumerable<PipeDefinition> PipeDefinitions => pipeRegistry.PipeDefinitions;

        private PipeRegistry pipeRegistry = new PipeRegistry();

        private List<PipeDefinition> definitions => pipeRegistry.PipeDefinitions;
        
        public PipelineBuilder(PipeRegistry pipeRegistry)
        {
            this.pipeRegistry = pipeRegistry.Clone();
        }

        public PipelineBuilder()
        {
        }
        
        public PipelineBuilder InsertAfter<TPipe>(string afterKey, string key = null)
        {
            int afterIndex = definitions.FindIndex(definition => definition.Key == afterKey);
            definitions.Insert(afterIndex + 1, new PipeDefinition(typeof(TPipe), key));
            return this;
        }
        
        public PipelineBuilder InsertAfter<TAfter, TPipe>(string key = null)
        {
            int afterIndex = definitions.FindIndex(definition => definition.Type == typeof(TAfter));
            definitions.Insert(afterIndex + 1, new PipeDefinition(typeof(TPipe), key));
            return this;
        }

        public PipelineBuilder InsertBefore<TPipe>(string beforeKey, string key = null)
        {
            int afterIndex = definitions.FindIndex(definition => definition.Key == beforeKey);
            definitions.Insert(afterIndex - 1, new PipeDefinition(typeof(TPipe), key));
            return this;
        }
        
        public PipelineBuilder InsertBefore<TBefore, TPipe>(string key = null)
        {
            int afterIndex = definitions.FindIndex(definition => definition.Type == typeof(TBefore));
            definitions.Insert(afterIndex - 1, new PipeDefinition(typeof(TPipe), key));
            return this;
        }

        public PipelineBuilder Replace<TReplace, TPipe>(string key = null)
        {
            int replaceIndex = definitions.FindIndex(definition => definition.Type == typeof(TReplace));
            definitions.RemoveAt(replaceIndex);
            definitions.Insert(replaceIndex, new PipeDefinition(typeof(TPipe), key));
            return this;
        }
        
        public PipelineBuilder Prepend<TPipe>(string key = null)
        {
            definitions.Insert(0, new PipeDefinition(typeof(TPipe), key));
            return this;
        }
        
        public PipelineBuilder Append<TPipe>(string key = null)
        {
            definitions.Add(new PipeDefinition(typeof(TPipe), key));
            return this;
        }
        public PipelineBuilder Append(Type tPipe, string key = null)
        {
            definitions.Add(new PipeDefinition(tPipe, key));
            return this;
        }

        public PipelineBuilder Clone()
        {
            return new PipelineBuilder()
            {
                pipeRegistry = pipeRegistry.Clone()
            };
        }
    }
}