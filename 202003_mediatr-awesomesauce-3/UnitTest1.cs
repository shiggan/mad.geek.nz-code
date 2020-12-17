using Autofac;

using FluentAssertions;

using MediatR;

using System.Threading.Tasks;

using Xunit;

namespace _202002_mediatr_awesomesauce_3
{
    public class UnitTest1
    {
      [Fact]
        public async Task shows_handler_via_autofac ( )
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
    }
}
