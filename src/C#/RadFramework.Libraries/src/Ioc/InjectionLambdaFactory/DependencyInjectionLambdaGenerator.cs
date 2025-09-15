using System.Linq.Expressions;
using System.Reflection;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Reflection.Caching;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc.ConstructionLambdaFactory
{
    public class DependencyInjectionLambdaGenerator
    {
        private static CachedMethodInfo dependencyMethod;
        private static CachedMethodInfo dependencyMethodWithKey;
        
        static DependencyInjectionLambdaGenerator()
        {
            CachedType argType = typeof (IocContainer);

            dependencyMethod = argType
                .Query(t => t.GetMethod(nameof(IocContainer.Resolve), new Type[] {typeof(Type)}));
            
            dependencyMethodWithKey = argType
                .Query(t => t.GetMethod(nameof(IocContainer.Resolve), new Type[] { typeof(Type), typeof(string) }));
        }

        public DependencyInjectionLambdaGenerator()
        {
        }
        
        public Func<IocContainer, object> CreateConstructorInjectionLambda(IocDependency dependency, CachedConstructorInfo injectionConstructor)
        {
            Type returnType = typeof (object);
            
            ParameterExpression containerArg = Expression.Parameter(typeof(IocContainer), "container");
            ParameterExpression constructionResult = Expression.Variable(returnType, "constructionResult");

            var returnLabel = Expression.Label(returnType, "returnLabel");

            List<Expression> methodBody = new()
            {
                                              Expression.Assign(constructionResult,
                                                  Expression.New(injectionConstructor,
                                                      BuildInjectionLambdaArguments(
                                                          containerArg,
                                                          injectionConstructor.Query(MethodBaseQueries.GetParameters).Select(p => (CachedParameterInfo)p).ToArray()))),

                                              Expression.Return(returnLabel, constructionResult, returnType),
                                              Expression.Label(returnLabel, constructionResult)
                                          };

            return Expression
                .Lambda<Func<IocContainer, object>>(
                    Expression
                        .Block(
                            new List<ParameterExpression> { constructionResult },
                            methodBody), 
                    containerArg)
                .Compile();
        }

        public Action<IocContainer, object> CreateMethodInjectionLambda(Type targetType, CachedMethodInfo injectionMethod)
        {
            ParameterExpression containerArg = Expression.Parameter(typeof(IocContainer), "container");
            ParameterExpression injectionTarget = Expression.Parameter(typeof(object), "injectionTarget");
            ParameterExpression typedInjectionTarget = Expression.Variable(targetType, "typedInjectionTarget");


            List<Expression> methodBody = new()
            {
                                                Expression.Assign(typedInjectionTarget, Expression.Convert(injectionTarget, targetType)),
                                                Expression.Call(typedInjectionTarget, 
                                                    injectionMethod,
                                                    BuildInjectionLambdaArguments(
                                                        containerArg,
                                                        injectionMethod.Query(MethodBaseQueries.GetParameters).Select(p => (CachedParameterInfo)p).ToArray()))
                                          };


            return Expression
                .Lambda<Action<IocContainer, object>>(Expression.Block(new [] { typedInjectionTarget }, methodBody), containerArg, injectionTarget)
                .Compile();
        }

        public Action<IocContainer, object> CreatePropertyInjectionLambda(Type targetType, CachedPropertyInfo[] injectionProperties)
        {
            ParameterExpression containerArg = Expression.Parameter(typeof(IocContainer), "container");
            ParameterExpression injectionTarget = Expression.Parameter(typeof(object), "injectionTarget");

            ParameterExpression typedInjectionTarget = Expression.Parameter(targetType, "typedInjectionTarget");

            List<Expression> injectionExpressions = new()
            {
                                                        Expression.Assign(typedInjectionTarget, Expression.Convert(injectionTarget, targetType))
                                                    };

            foreach (PropertyInfo propertyInfo in injectionProperties)
            {
                MemberExpression propertyExpression = Expression.Property(typedInjectionTarget, propertyInfo);

                Expression argInjectionPlaceholder = DependencyPlacehlder(containerArg, propertyInfo.PropertyType);

                injectionExpressions.Add(Expression.Assign(propertyExpression, argInjectionPlaceholder));
            }

            return Expression
                    .Lambda<Action<IocContainer, object>>(
                        Expression.Block(
                            new[] { typedInjectionTarget }, 
                            injectionExpressions), 
                        containerArg, 
                        injectionTarget)
                    .Compile();
        }
        
        private static Expression[] BuildInjectionLambdaArguments(Expression containerInstance, CachedParameterInfo[] parameterInfos)
        {
            List<Expression> arguments = new();

            foreach (CachedParameterInfo parameter in parameterInfos)
            {
                IocDependencyAttribute attribute = parameter.Query(param =>
                    param.GetCustomAttributes().OfType<IocDependencyAttribute>().FirstOrDefault());

                if (attribute != null && attribute.Key != null && attribute.Key.KeyName != null)
                {
                    arguments.Add(NamedDependencPlaceholder(containerInstance, parameter.InnerMetaData.ParameterType, attribute.Key.KeyName));
                    continue;
                }
                
                arguments.Add(DependencyPlacehlder(containerInstance, parameter.InnerMetaData.ParameterType));
            }

            return arguments.ToArray();
        }

        private static Expression NamedDependencPlaceholder(Expression instance, Type placeholderType, string iocKeyString)
        {
            return Expression
                .Convert(Expression
                    .Call(instance, 
                        dependencyMethodWithKey, 
                        Expression.Constant(placeholderType),
                        Expression.Constant(iocKeyString)), 
                    placeholderType);
        }
        
        private static Expression DependencyPlacehlder(Expression instance, Type placeholderType)
        {
            return Expression
                .Convert(Expression
                    .Call(instance,
                        dependencyMethod, 
                        Expression.Constant(placeholderType)), 
                    placeholderType);
        }
    }
}