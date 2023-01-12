namespace Nameless.WebApplication {

    public static class TypeExtension {

        #region Public Static Methods

        public static bool IsSingleton(this Type? self) => SingletonAttribute.IsSingleton(self);
        public static object? GetSingletonInstance(this Type? self) => SingletonAttribute.GetInstance(self);

        /// <summary>
        /// Determines whether the <paramref name="genericType"/> is assignable from
        /// <paramref name="self"/> taking into account generic definitions
        /// </summary>
        /// <param name="self">The given type.</param>
        /// <param name="genericType">The generic type.</param>
        /// <returns><c>true</c> if <paramref name="genericType"/> is assignable from <paramref name="self"/>, otherwise, <c>false</c>.</returns>
        public static bool IsAssignableToGenericType(this Type? self, Type genericType) {
            if (self == null || genericType == null) { return false; }

            return self == genericType ||
                MapsToGenericTypeDefinition(self, genericType) ||
                HasInterfaceThatMapsToGenericTypeDefinition(self, genericType) ||
                self.BaseType.IsAssignableToGenericType(genericType);
        }

        #endregion

        #region Private Static Methods

        private static bool MapsToGenericTypeDefinition(Type self, Type genericType) => genericType.IsGenericTypeDefinition &&
            self.IsGenericType &&
            self.GetGenericTypeDefinition() == genericType;

        private static bool HasInterfaceThatMapsToGenericTypeDefinition(Type self, Type genericType) => self
            .GetInterfaces()
            .Where(_ => _.IsGenericType)
            .Any(_ => _.GetGenericTypeDefinition() == genericType);

        #endregion
    }
}
