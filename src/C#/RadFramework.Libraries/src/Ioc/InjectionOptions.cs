using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.ConstructionMethodBuilders
{
    public class InjectionOptions
    {
        public Func<IEnumerable<CachedConstructorInfo>, CachedConstructorInfo> ChooseInjectionConstructor { get; set; }

        public Func<Dictionary<IocKey, CachedParameterInfo>, Dictionary<IocKey, CachedParameterInfo>>
            ConstructorParameterInjection;
        
        public Func<IEnumerable<CachedPropertyInfo>, IEnumerable<CachedPropertyInfo>> ChooseInjectionProperties { get; set; }
        
        public Func<IEnumerable<CachedMethodInfo>, IEnumerable<CachedMethodInfo>> ChooseInjectionMethods { get; set; }
        
        public Func<Dictionary<IocKey, CachedParameterInfo>, Dictionary<IocKey, CachedParameterInfo>>
            MethodParameterInjection;
        
        public InjectionOptions Clone()
        {
            return new InjectionOptions()
            {
                ChooseInjectionConstructor = ChooseInjectionConstructor,
                ConstructorParameterInjection = ConstructorParameterInjection,
                ChooseInjectionMethods = ChooseInjectionMethods,
                MethodParameterInjection = MethodParameterInjection,
                ChooseInjectionProperties = ChooseInjectionProperties
            };
        }
    }
}