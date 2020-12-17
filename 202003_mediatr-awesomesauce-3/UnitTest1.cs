using Autofac;

using FluentAssertions;

using MediatR;

using System;

using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using Xunit;

namespace _202002_mediatr_awesomesauce_3
{
    public class UnitTest1
    {
      [Fact]
        public async Task shows_handler_behavior_discovery ( )
        {
          var builder = new ContainerBuilder ( );

          builder.RegisterType<Mediator> ( ).As<IMediator> ( ).InstancePerLifetimeScope ( );
          builder.Register<ServiceFactory> ( ctx =>
          {
            var c = ctx.Resolve<IComponentContext> ( );
            return t => c.Resolve ( t );
          } );

          var logger = new Logger ( );
          builder.RegisterInstance ( logger ).As<ILogger> ( ).SingleInstance ( );

          // discover all request handlers
          // for each handler we discover
          // - discover type arguments
          // - 

          var requestHandlers = DiscoverRequestHandlers ( );

          // simply register the handler
          builder.RegisterType<Handler1> ( ).AsImplementedInterfaces ( );

          var container = builder.Build ( );

          var mediatr = container.Resolve<IMediator> ( );
          mediatr.Should ( ).NotBeNull ( );

          var request = new Handler1Message ( ) { Name = "The Mad Geek" };
          var response = await mediatr.Send ( request );

          response.Should ( ).NotBeNull ( );
          response.Message.Should ( ).Be ( "Hello The Mad Geek" );

          logger.Messages.Should ( ).BeEquivalentTo ( new [ ]
            {
              "handle Handler1"
            }
          );
        }

    private IEnumerable<TypeInfo> DiscoverRequestHandlers ( )
    {
      var query = new [] { typeof(UnitTest1).Assembly }
        .Where ( a => !a.IsDynamic )
        .Distinct ( )
        .SelectMany ( a => a.DefinedTypes )
        .ToArray ( );

      var registerTypes = new [ ] {
        typeof(IRequestHandler<,>)
      };

      var mediatrHandlers = registerTypes.SelectMany ( openType =>
        query.Where (
          t => t.IsClass
            && !t.IsAbstract
            && ImplementsGenericInterface ( t.AsType ( ), openType )
        ) )
        .ToList (  );

      return mediatrHandlers;
    }

    private static bool ImplementsGenericInterface ( Type type, Type interfaceType )
      => IsGenericType ( type, interfaceType ) || type.GetTypeInfo ( ).ImplementedInterfaces.Any ( @interface => IsGenericType ( @interface, interfaceType ) );

    private static bool IsGenericType ( Type type, Type genericType )
      => type.GetTypeInfo ( ).IsGenericType && type.GetGenericTypeDefinition ( ) == genericType;
  }
}
