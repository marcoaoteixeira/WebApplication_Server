using System.Reflection;
using AutoMapper;
using Nameless.WebApplication.UnitTest.Fixtures;

namespace Nameless.WebApplication.UnitTest {

    public abstract class TestCaseBase {

        protected IMapper Mapper { get; set; } = NullMapper.Instance;


        protected void ConfigureMapper(Assembly[] assemblies) {
            var profileTypes = SearchForImplementations(typeof(Profile), assemblies ?? Array.Empty<Assembly>());

            ConfigureMapper(profileTypes);
        }

        protected void ConfigureMapper(Type[] profileTypes) {
            // configure mapper
            var config = new MapperConfiguration(cfg => {
                if (profileTypes != null) {
                    foreach (var profile in profileTypes) {
                        cfg.AddProfile(profile);
                    }
                }
            });

            Mapper = config.CreateMapper();
        }

        private static Type[] SearchForImplementations(Type serviceType, IEnumerable<Assembly> assemblies) {
            var result = assemblies
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface)
                .Where(type => serviceType.IsAssignableFrom(type) || type.IsAssignableToGenericType(serviceType))
                .Where(type => type.GetCustomAttribute<SingletonAttribute>(inherit: false) == null)
                .ToArray();

            return result!;
        }
    }
}
