namespace _202002_mediatr_awesomesauce_3
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class Logger: ILogger
  {
    public Logger()
    {
      this.Messages = new List<string> ( );
    }

    public List<string> Messages { get; set; }
  }

  public interface ILogger
  {
    List<string> Messages { get; set; }
  }
}
