using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace _202002_mediatr_awesomesauce_1
{
  public class GreeterMessage: IRequest<GreeterResult>
  {
    public string Name { get; set; }
  }

  public class GreeterResult
  {
    public string Message { get; set; }
  }

  public class GreeterHandler: IRequestHandler<GreeterMessage, GreeterResult>
  {
    public Task<GreeterResult> Handle ( GreeterMessage request, CancellationToken cancellationToken )
    {
      return Task.FromResult ( new GreeterResult { Message = $"Hello {request.Name}" } );
    }
  }
}
