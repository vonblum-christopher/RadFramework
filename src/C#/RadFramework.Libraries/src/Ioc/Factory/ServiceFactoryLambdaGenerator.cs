using System.Linq.Expressions;
using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc.Factory
{
    public class ServiceFactoryLambdaGenerator
    {
        public static ServiceFactoryLambdaGenerator DefaultInstance = new ServiceFactoryLambdaGenerator();
        
        private DependencyInjectionLambdaGenerator lambdaGenerator = new DependencyInjectionLambdaGenerator();

        public Func<Core.IocContainer, object> CreateInstanceFactoryMethod(
            IocServiceRegistration serviceRegistration)
        {
            // chose the best constructor
            CachedConstructorInfo constructor =
                (serviceRegistration.InjectionOptions.ChooseInjectionConstructor ?? serviceRegistration.InjectionOptions.ChooseInjectionConstructor)(
                    serviceRegistration.ImplementationType
                        .Query(ClassQueries.GetPublicConstructors)
                        .Select(c => (CachedConstructorInfo) c));

            var constructLamda = constructor.Query(info => lambdaGenerator.CreateConstructorInjectionLambda(info));
            
            List<Action<Core.IocContainer, object>> injectionLambdas = new List<Action<Core.IocContainer, object>>();

            var chooseInjectionMethods = serviceRegistration.InjectionOptions.ChooseInjectionMethods;
            
            if (chooseInjectionMethods != null)
            {
                var injectionMethods = chooseInjectionMethods(serviceRegistration.ImplementationType
                    .Query(ClassQueries.GetPublicImplementedMethods).Select(m => (CachedMethodInfo) m));

                foreach (var cachedMethodInfo in injectionMethods)
                {
                    var methodInjectionLambda = cachedMethodInfo.Query(m =>
                        lambdaGenerator.CreateMethodInjectionLambda(serviceRegistration.ImplementationType, cachedMethodInfo));
                    
                    injectionLambdas.Add(methodInjectionLambda);
                }
            }

            var chooseInjectionProperties = serviceRegistration.InjectionOptions.ChooseInjectionProperties;
            
            if (chooseInjectionProperties != null)
            {
                var injectionProperties = chooseInjectionProperties(serviceRegistration.ImplementationType.Query(ClassQueries.GetPublicImplementedProperties).Select(p =>(CachedPropertyInfo)p)).ToArray();

                if (injectionProperties.Length > 0)
                {
                    injectionLambdas.Add(lambdaGenerator.CreatePropertyInjectionLambda(serviceRegistration.ImplementationType, injectionProperties));
                }
            }

            return CombineConstructorInjectionAndMemberInjectionLambdas(constructLamda, injectionLambdas.ToArray());
        }
        public Func<Core.IocContainer, object> CombineConstructorInjectionAndMemberInjectionLambdas(Func<Core.IocContainer, object> constructorInjectionLambda, Action<Core.IocContainer, object>[] memberInjectionLambdas)
        {
            if (!memberInjectionLambdas.Any())
            {
                return constructorInjectionLambda;
            }

            Type returnType = typeof(object);

            ParameterExpression containerInstance = Expression.Variable(typeof(Core.IocContainer), "containerArg");
            ParameterExpression constructionResult = Expression.Variable(returnType, "constructionResult");

            var returnLabel = Expression.Label(returnType, "returnLabel");

            List<Expression> methodBody = new List<Expression>
            {
                Expression.Assign(constructionResult, Expression.Invoke(Expression.Constant(constructorInjectionLambda), containerInstance))
            };

            foreach (Action<Core.IocContainer, object> injectionLambda in memberInjectionLambdas)
            {
                methodBody.Add(Expression.Invoke(Expression.Constant(injectionLambda), containerInstance, constructionResult));
            }

            methodBody.Add(Expression.Return(returnLabel, constructionResult, returnType));
            methodBody.Add(Expression.Label(returnLabel, constructionResult));

            return Expression
                .Lambda<Func<Core.IocContainer, object>>(Expression.Block(new List<ParameterExpression> { constructionResult }, methodBody), containerInstance)
                .Compile();
        }
    }
}