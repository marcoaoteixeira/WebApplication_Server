using AutoMapper;

namespace Nameless.WebApplication.Domain {

    internal static class MappingExpressionExtension {

        #region Internal Static Methods

        internal static IMappingExpression<TSource, TDest>? IgnoreAll<TSource, TDest>(this IMappingExpression<TSource, TDest> expression) {
            if (expression == default) { return default; }

            expression.ForAllMembers(opt => opt.Ignore());

            return expression;
        }

        internal static IMappingExpression? IgnoreAll(this IMappingExpression expression) {
            if (expression == default) { return default; }

            expression.ForAllMembers(opt => opt.Ignore());

            return expression;
        }

        #endregion
    }
}
