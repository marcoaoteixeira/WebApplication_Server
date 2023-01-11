using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;

namespace Nameless.WebApplication.Autofac {
    public sealed class FactoryResolveMiddleware : IResolveMiddleware {

        #region Private Read-Only Fields

        private readonly Type _injectType;
        private readonly Func<MemberInfo, IComponentContext, object> _factory;

        #endregion

        #region Public Constructors

        public FactoryResolveMiddleware(Type injectType, Func<MemberInfo, IComponentContext, object> factory) {
            Prevent.Null(injectType, nameof(injectType));
            Prevent.Null(factory, nameof(factory));

            _injectType = injectType;
            _factory = factory;
        }

        #endregion

        #region Private Static Methods

        private static PropertyInfo[] GetProperties(Type type, Type propertyType) {
            var result = type
                .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .Select(property => new {
                    PropertyInfo = property,
                    property.PropertyType,
                    IndexParameters = property.GetIndexParameters().ToArray(),
                    property.CanWrite
                })
                .Where(property => property.PropertyType == propertyType)
                .Where(property => property.CanWrite)
                .Where(property => property.IndexParameters.Length == 0);

            return result.Select(_ => _.PropertyInfo).ToArray();
        }

        #endregion

        #region IResolveMiddleware Members

        public PipelinePhase Phase => PipelinePhase.ParameterSelection;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next) {
            context.ChangeParameters(context.Parameters.Union(
                new[] {
                    new ResolvedParameter(
                        predicate: (param, ctx) => param.ParameterType == _injectType,
                        valueAccessor: (param, ctx) => _factory(param.Member, ctx)
                    )
                }
            ));

            next(context);

            if (context.NewInstanceActivated) {
                var serviceType = context.Instance!.GetType();
                var properties = GetProperties(serviceType, _injectType);
                foreach (var property in properties) {
                    property.SetValue(context.Instance, _factory(property, context), null);
                }
            }
        }

        #endregion
    }
}
