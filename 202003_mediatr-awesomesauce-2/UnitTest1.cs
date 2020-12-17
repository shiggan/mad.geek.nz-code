using Autofac;

using FluentAssertions;

using MediatR;

using System.Threading.Tasks;

using Xunit;

namespace _202002_mediatr_awesomesauce_2
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

    [Fact]
    public async Task shows_handler_via_autofac_single_behavior ( )
    {
      var builder = new ContainerBuilder ( );

      builder.RegisterType<Mediator> ( ).As<IMediator> ( ).InstancePerLifetimeScope ( );
      builder.Register<ServiceFactory> ( ctx =>
        {
          var c = ctx.Resolve<IComponentContext> ( );
          return t => c.Resolve ( t );
        }
      );

      var logger = new Logger ( );
      builder.RegisterInstance ( logger ).As<ILogger> ( ).SingleInstance ( );

      // simply register the handler
      builder.RegisterType<Handler1> ( ).AsImplementedInterfaces ( );

      // next register the Behavior
      builder.RegisterGeneric ( typeof ( FirstBehavior<,> ) ).AsImplementedInterfaces ( );

      var container = builder.Build ( );

      var mediatr = container.Resolve<IMediator> ( );
      mediatr.Should ( ).NotBeNull ( );

      var request = new Handler1Message ( ) { Name = "The Mad Geek" };
      var response = await mediatr.Send ( request );

      response.Should ( ).NotBeNull ( );
      response.Message.Should ( ).Be ( "Hello The Mad Geek" );

      logger.Messages.Should ( ).BeEquivalentTo ( new [ ]
        {
          "in FirstBehavior",
          "handle Handler1", 
          "out FirstBehavior"
        }
      );
    }

    [Fact]
    public async Task shows_handler_via_autofac_multiple_behavior ( )
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
      builder.RegisterType<Handler2> ( ).AsImplementedInterfaces ( );

      // next register the Behaviors
      builder.RegisterGeneric ( typeof ( FirstBehavior<,> ) ).AsImplementedInterfaces ( );
      builder.RegisterGeneric ( typeof ( SecondBehavior<,> ) ).AsImplementedInterfaces ( );

      var container = builder.Build ( );

      var mediatr = container.Resolve<IMediator> ( );
      mediatr.Should ( ).NotBeNull ( );

      var handler1message = new Handler1Message ( ) { Name = "The Mad Geek" };
      var handler1response = await mediatr.Send ( handler1message );

      handler1response.Should ( ).NotBeNull ( );
      handler1response.Message.Should ( ).Be ( "Hello The Mad Geek" );

      logger.Messages.Should ( ).BeEquivalentTo ( new [ ]
        {
          "in FirstBehavior", 
          "in SecondBehavior", 
          "handle Handler1",
          "out SecondBehavior", 
          "out FirstBehavior"
        }  
      );
      logger.Messages.Clear();

      var handler2message = new Handler2Message ( ) { Name = "The Mad Geek" };
      var handler2response = await mediatr.Send ( handler2message );

      handler2response.Should ( ).NotBeNull ( );
      handler2response.Message.Should ( ).Be ( "Hello The Mad Geek" );

      logger.Messages.Should ( ).BeEquivalentTo ( new [ ]
        {
          "in FirstBehavior", 
          "in SecondBehavior", 
          "handle Handler2",
          "out SecondBehavior", 
          "out FirstBehavior"
        }  
      );
    }

    [Fact]
    public async Task shows_handler_opt_in_to_behavior ( )
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

      // handler 1 gets second behavior only
      builder.RegisterType<Handler1> ( ).AsImplementedInterfaces ( );
      builder.RegisterType<SecondBehavior<Handler1Message, Handler1Result>> ( ).AsImplementedInterfaces ( );

      // handler 2 gets second behavior, then first behavior
      builder.RegisterType<Handler2> ( ).AsImplementedInterfaces ( );
      builder.RegisterType<SecondBehavior<Handler2Message, Handler2Result>> ( ).AsImplementedInterfaces ( );
      builder.RegisterType<FirstBehavior<Handler2Message, Handler2Result>> ( ).AsImplementedInterfaces ( );

      var container = builder.Build ( );

      var mediatr = container.Resolve<IMediator> ( );
      mediatr.Should ( ).NotBeNull ( );

      var request1 = new Handler1Message ( ) { Name = "The Mad Geek" };
      var response1 = await mediatr.Send ( request1 );

      response1.Should ( ).NotBeNull ( );
      response1.Message.Should ( ).Be ( "Hello The Mad Geek" );

      logger.Messages.Should ( ).BeEquivalentTo ( new [ ]
        {
          "in SecondBehavior", 
          "handle Handler1",
          "out SecondBehavior"
        }  
      );
      logger.Messages.Clear();
      
      var request2 = new Handler2Message ( ) { Name = "The Mad Geek" };
      var response2 = await mediatr.Send ( request2 );

      response2.Should ( ).NotBeNull ( );
      response2.Message.Should ( ).Be ( "Hello The Mad Geek" );

      logger.Messages.Should ( ).BeEquivalentTo ( new [ ]
        {
          "in SecondBehavior", 
          "in FirstBehavior", 
          "handle Handler2",
          "out FirstBehavior", 
          "out SecondBehavior"
        }
      ); 
    }
  }
}