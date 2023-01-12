using Autofac;
using Nameless.WebApplication.Autofac;
using Nameless.WebApplication.Commands.Impl;

namespace Nameless.WebApplication.Commands.Infrastructure {

    public sealed class CommandModule : ModuleBase {

        #region Public Constructors

        public Type[] CommandHandlerImplementations { get; set; } = Array.Empty<Type>();
        public Type CommandDispatcherImplementation { get; set; } = typeof(CommandDispatcher);

        #endregion

        #region Protected Override Methods

        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterType(CommandDispatcherImplementation ?? SearchForImplementation<ICommandDispatcher>() ?? typeof(CommandDispatcher))
                .As<ICommandDispatcher>()
                .SingleInstance();

            builder
                .RegisterTypes(CommandHandlerImplementations ?? SearchForImplementations(typeof(ICommandHandler<>)))
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .SingleInstance();

            base.Load(builder);
        }

        #endregion
    }
}
