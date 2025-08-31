using RadFramework.Libraries.Extensibility.Pipeline.Extension;
using RadFramework.Libraries.Ioc;

namespace RadFramework.Libraries.Extensibility.Pipeline.Synchronous
{
    public class SynchronousPipeline<TIn, TOut> : IPipeline<TIn, TOut>
    {
        private readonly IIocContainer _serviceProvider;
        public LinkedList<ISynchronousPipe> pipes;

        public SynchronousPipeline(PipelineDefinition definition, IIocContainer serviceProvider)
        {
            _serviceProvider = serviceProvider;
            pipes = new LinkedList<ISynchronousPipe>(definition.Definitions.Select(CreatePipe));
        }
        
        public SynchronousPipeline(IEnumerable<ISynchronousPipe> definitions)
        {
            definitions = new LinkedList<ISynchronousPipe>(definitions);
        }

        private ISynchronousPipe CreatePipe(PipeDefinition def)
        {
            return (ISynchronousPipe) _serviceProvider.Activate(def.Type);
        }

        public TOut Process(TIn input)
        {
            ExtensionPipeContext pipeContext = new ExtensionPipeContext();
        
            foreach (var pipe in pipes)
            {
                pipe.Process(input, pipeContext);

                if (pipeContext.ShouldReturn)
                {
                    break;
                }
            }

            return pipeContext.ShouldReturn;
        }
    }
}