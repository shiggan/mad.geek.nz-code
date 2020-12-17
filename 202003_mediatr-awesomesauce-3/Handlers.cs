namespace _202002_mediatr_awesomesauce_3
{
  using MediatR;

  using System.Threading;
  using System.Threading.Tasks;

  public class Handler1Message: IRequest<Handler1Result>
  {
    public string Name { get; set; }
  }

  public class Handler1Result
  {
    public string Message { get; set; }
  }

  public class Handler1: IRequestHandler<Handler1Message, Handler1Result>
  {
    private readonly ILogger logger;

    public Handler1(ILogger logger)
    {
      this.logger = logger;
    }

    public Task<Handler1Result> Handle ( Handler1Message request, CancellationToken cancellationToken )
    {
        logger.Messages.Add ( $"handle {nameof ( Handler1 )}" );

        return Task.FromResult ( new Handler1Result { Message = $"Hello {request.Name}" } );
    }
  }

  public class Handler2Message: IRequest<Handler2Result>
  {
    public string Name { get; set; }
  }

  public class Handler2Result
  {
    public string Message { get; set; }
  }

  public class Handler2: IRequestHandler<Handler2Message, Handler2Result>
  {
    private readonly ILogger logger;

    public Handler2(ILogger logger)
    {
      this.logger = logger;
    }

    public Task<Handler2Result> Handle ( Handler2Message request, CancellationToken cancellationToken )
    {
        logger.Messages.Add ( $"handle {nameof ( Handler2 )}" );

        return Task.FromResult ( new Handler2Result { Message = $"Hello {request.Name}" } );
    }
  }
}
