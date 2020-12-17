using FluentAssertions;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace _202002_mediatr_awesomesauce_1
{
  public class UnitTest1
  {
    [Fact]
    public async Task shows_call_handler ( )
    {
      IRequestHandler<GreeterMessage, GreeterResult> handler = new GreeterHandler ( );

      var request = new GreeterMessage ( ) { Name = "The Mad Geek" };
      var response = await handler.Handle ( request, cancellationToken: CancellationToken.None );

      response.Should ( ).NotBeNull ( );
      response.Message.Should ( ).Be ( "Hello The Mad Geek" );
    }
  }
}