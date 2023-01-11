namespace Nameless.WebApplication {

    public static class TypeExtension {

        #region Public Static Methods

        public static bool IsSingleton(this Type? self) => SingletonAttribute.IsSingleton(self);
        public static object? GetSingletonInstance(this Type? self) => SingletonAttribute.GetInstance(self);

        #endregion
    }
}
