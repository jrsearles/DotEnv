using System;

namespace DotEnv
{
  internal static class ParserExtensions
  {
    public static bool IsComment(this char @this) => @this == ParserConstants.Comment;

    public static bool IsComment(this ReadOnlySpan<char> span) => !span.IsEmpty && span[0].IsComment();

    public static int IndexOf(this ReadOnlySpan<char> input, char c, int startIndex)
    {
      int nextIndex = input.Slice(startIndex).IndexOf(c);
      if (nextIndex == -1)
      {
        return nextIndex;
      }

      return nextIndex + startIndex;
    }

    public static string AsValue(this ReadOnlySpan<char> input) => new(input.TrimEnd());
  }
}
