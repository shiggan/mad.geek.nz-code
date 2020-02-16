using System;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Xunit;

namespace _202002_mediatr_awesomesauce
{
  public class RequestHandlerTests
  {
    public class Ping : IRequest<Pong>
    {
      public string Message { get; set; }
    }

    public class Pong
    {
      public string Message { get; set; }
    }

    public class PingHandler : RequestHandler<Ping, Pong>
    {
      protected override Pong Handle(Ping request)
      {
        return new Pong { Message = request.Message + " Pong" };
      }
    }

    [Fact]
    public async Task Should_call_handler()
    {
      IRequestHandler<Ping, Pong> handler = new PingHandler();

      var response = await handler.Handle(new Ping() { Message = "Ping" }, default);

      response.Message.Should().Be("Ping Pong");
    }

    [Fact]
    public async Task Should_call_handler_with_pipeline()
    {

    }
  }
}
