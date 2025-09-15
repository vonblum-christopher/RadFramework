using System.Linq.Expressions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Reflection.Caching;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc.ConstructionLambdaFactory
{
    public class ServiceFactoryLambdaGenerator
    {
        public static ServiceFactoryLambdaGenerator DefaultInstance = new();
        
        private DependencyInjectionLambdaGenerator lambdaGenerator = new();

        public Func<IocContainer, object> CreateTypeFactoryLambda(
            IocDependency dependency)
        {
            Func<IocContainer, object> constructorLambda = BuildConstructorLambdas(dependency);

            List<Action<IocContainer, object>> injectionLambdas = new(BuildMethodInjectionLambdas(dependency));

            BuildPropertyInjectionLambdas(dependency, injectionLambdas);

            return CombineConstructorInjectionAndMemberInjectionLambdas(constructorLambda, injectionLambdas.ToArray());
        }

        private Func<IocContainer, object> BuildConstructorLambdas(IocDependency dependency)
        {
            CachedConstructorInfo constructor =
                dependency.InjectionOptions.ChooseInjectionConstructor(
                    dependency.ImplementationType
                        .Query(ClassQueries.GetPublicConstructors)
                        .Select(c => (CachedConstructorInfo) c));

            return
                constructor
                    .Query(info => lambdaGenerator.CreateConstructorInjectionLambda(dependency, constructor));
        }

        private void BuildPropertyInjectionLambdas(
            IocDependency dependency,
            List<Action<IocContainer, object>> injectionLambdas)
        {
            Func<IEnumerable<CachedPropertyInfo>, IEnumerable<CachedPropertyInfo>> chooseInjectionProperties =
                dependency.InjectionOptions.ChooseInjectionProperties;

            if (chooseInjectionProperties != null)
            {
                CachedPropertyInfo[] injectionProperties = 
                    chooseInjectionProperties(dependency.ImplementationType
                            .Query(ClassQueries.GetPublicImplementedProperties)
                            .Select(p => (CachedPropertyInfo)p))
                        .ToArray();

                if (injectionProperties.Length > 0)
                {
                    injectionLambdas.Add(lambdaGenerator.CreatePropertyInjectionLambda(
                        dependency.ImplementationType,
                        injectionProperties));
                }
            }
        }

        private List<Action<IocContainer, object>> BuildMethodInjectionLambdas(
            IocDependency dependency)
        {
            List<Action<IocContainer, object>> injectionLambdas = new();

            Func<IEnumerable<CachedMethodInfo>, IEnumerable<CachedMethodInfo>> chooseInjectionMethods =
                dependency.InjectionOptions.ChooseInjectionMethods;
            
            if (chooseInjectionMethods != null)
            {
                IEnumerable<CachedMethodInfo> injectionMethods = chooseInjectionMethods(dependency.ImplementationType
                    .Query(ClassQueries.GetPublicImplementedMethods).Select(m => (CachedMethodInfo) m));

                foreach (CachedMethodInfo cachedMethodInfo in injectionMethods)
                {
                    Action<IocContainer, object> methodInjectionLambda = cachedMethodInfo.Query(m =>
                        lambdaGenerator.CreateMethodInjectionLambda(dependency.ImplementationType, cachedMethodInfo));
                    
                    injectionLambdas.Add(methodInjectionLambda);
                }
            }

            return injectionLambdas;
        }

        public Func<IocContainer, object> CombineConstructorInjectionAndMemberInjectionLambdas(
            Func<IocContainer, object> constructorInjectionLambda,
            Action<IocContainer, object>[] memberInjectionLambdas)
        {
            if (!memberInjectionLambdas.Any())
            {
                return constructorInjectionLambda;
            }

            Type returnType = typeof(object);

            ParameterExpression containerInstance = Expression.Variable(typeof(IocContainer), "containerArg");
            ParameterExpression constructionResult = Expression.Variable(returnType, "constructionResult");

            LabelTarget returnLabel = Expression.Label(returnType, "returnLabel");

            List<Expression> methodBody = new()
            {
                Expression.Assign(constructionResult, Expression.Invoke(Expression.Constant(constructorInjectionLambda), containerInstance))
            };

            foreach (Action<IocContainer, object> injectionLambda in memberInjectionLambdas)
            {
                methodBody.Add(Expression.Invoke(Expression.Constant(injectionLambda), containerInstance, constructionResult));
            }

            methodBody.Add(Expression.Return(returnLabel, constructionResult, returnType));
            methodBody.Add(Expression.Label(returnLabel, constructionResult));

            return Expression
                .Lambda<Func<IocContainer, object>>(Expression.Block(new List<ParameterExpression> { constructionResult }, methodBody), containerInstance)
                .Compile();
        }

        InjectionOptions CombineInjectionOptions(InjectionOptions original, InjectionOptions overrides)
        {
            return new InjectionOptions
            {
                ChooseInjectionConstructor =
                    original.ChooseInjectionConstructor ?? overrides.ChooseInjectionConstructor,
                ChooseInjectionMethods = 
                    original.ChooseInjectionMethods ?? overrides.ChooseInjectionMethods,
                ChooseInjectionProperties = 
                    original.ChooseInjectionProperties ?? overrides.ChooseInjectionProperties,
                ConstructorParameterInjection =
                    original.ConstructorParameterInjection ?? overrides.ConstructorParameterInjection,
                MethodParameterInjection = 
                    original.MethodParameterInjection ?? overrides.MethodParameterInjection
            };
        }
    }
}