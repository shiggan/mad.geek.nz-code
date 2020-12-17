namespace _202002_mediatr_awesomesauce_3
{
  using Autofac;

  using System;

  public interface IBehaviorBuilder
  {
    int Order { get; }
    void Build ( ContainerBuilder builder, Type request, Type response );
  }

  [AttributeUsage ( AttributeTargets.Class )]
  public class FirstBehaviorAttribute: Attribute, IBehaviorBuilder
  {
    public FirstBehaviorAttribute ( int order = 0 )
    {
      this.Order = order;
    }

    /// <inheritdoc />
    public int Order { get; }

    /// <inheritdoc />
    public void Build ( ContainerBuilder builder, Type request, Type response )
    {
      var generic = typeof(FirstBehavior<,>);
      var args = new [ ] { request, response };

      var constructed = generic.MakeGenericType ( args );

      builder.RegisterType ( constructed ).AsImplementedInterfaces ( );
    }
  }

  [AttributeUsage ( AttributeTargets.Class )]
  public class SecondBehaviorAttribute: Attribute, IBehaviorBuilder
  {
    public SecondBehaviorAttribute ( int order = 0 )
    {
      this.Order = order;
    }

    /// <inheritdoc />
    public int Order { get; }

    /// <inheritdoc />
    public void Build ( ContainerBuilder builder, Type request, Type response )
    {
      var generic = typeof(SecondBehavior<,>);
      var args = new [ ] { request, response };

      var constructed = generic.MakeGenericType ( args );

      builder.RegisterType ( constructed ).AsImplementedInterfaces ( );
    }
  }
}