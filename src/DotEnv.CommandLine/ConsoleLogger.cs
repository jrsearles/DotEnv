using System;

namespace DotEnv
{
  public static class ConsoleLogger
  {
    public static void LogError(string text)
    {
      using (ConsoleLogger.SetColor(ConsoleColor.Red))
      {
        Console.Error.WriteLine(text);
      }
    }

    public static void LogMessage(string text)
    {
      using (ConsoleLogger.SetColor(ConsoleColor.Gray))
      {
        Console.WriteLine(text);
      }
    }

    internal static IDisposable SetColor(ConsoleColor color) => new ColorChanger(color);

    private class ColorChanger : IDisposable
    {
      private ConsoleColor _current = Console.ForegroundColor;

      public ColorChanger(ConsoleColor color)
      {
        Console.ForegroundColor = color;
      }

      public void Dispose() => Console.ForegroundColor = _current;
    }
  }
}
