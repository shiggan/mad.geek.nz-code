namespace _202002_mediatr_awesomesauce_3
{
  using MediatR;

  using System.Threading;
  using System.Threading.Tasks;

  public class FirstBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
  {
    private readonly ILogger logger;

    public FirstBehavior( ILogger logger )
    {
      this.logger = logger;
    }

    public async Task<TResponse> Handle ( TRequest request, CancellationToken cancellationToken,
      RequestHandlerDelegate<TResponse> next )
    {
      try
      {
        logger.Messages.Add ( $"in {nameof ( FirstBehavior<TRequest, TResponse> )}" );

        return await next ( );
      }
      finally
      {
        logger.Messages.Add ( $"out {nameof ( FirstBehavior<TRequest, TResponse> )}" );
      }
    }
  }

  public class SecondBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
  {
    private readonly ILogger logger;

    public SecondBehavior ( ILogger logger )
    {
      this.logger = logger;
    }

    public async Task<TResponse> Handle ( TRequest request, CancellationToken cancellationToken,
      RequestHandlerDelegate<TResponse> next )
    {
      try
      {
        logger.Messages.Add ( $"in {nameof ( SecondBehavior<TRequest, TResponse> )}" );

        return await next ( );
      }
      finally
      {
        logger.Messages.Add ( $"out {nameof ( SecondBehavior<TRequest, TResponse> )}" );
      }
    }
  }
}