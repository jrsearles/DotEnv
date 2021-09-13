using System;
using System.Collections.Generic;
using System.IO;

namespace DotEnv
{
  public static class EnvFileParser
  {
    public static bool TryParse(string envFile, out IDictionary<string, string> values)
    {
      if (!File.Exists(envFile))
      {
        values = default;
        return false;
      }

      values = EnvFileParser.ParseLines(new ReadOnlySpan<string>(File.ReadAllLines(envFile)));
      return true;
    }

    internal static Dictionary<string, string> ParseLines(ReadOnlySpan<string> lines)
    {
      Dictionary<string, string> values = new(StringComparer.Ordinal);

      foreach (var line in lines)
      {
        var span = line.TrimStart().AsSpan();
        if (span.IsEmpty || span.IsComment())
        {
          continue;
        }

        int equalsIndex = span.IndexOf('=');
        if (equalsIndex == -1)
        {
          continue;
        }

        string key = new(span.Slice(0, equalsIndex).TrimEnd());
        string value = EnvFileParser.ReadValue(span.Slice(equalsIndex + 1));

        values[key] = value;
      }

      return values;
    }

    private static string ReadValue(ReadOnlySpan<char> input)
    {
      input = input.TrimStart();
      if (input.IsEmpty || input.IsComment())
      {
        return string.Empty;
      }

      return input[0] switch
      {
        ParserConstants.DoubleQuote => EnvFileParser.ReadDoubleQuotedValue(input),
        ParserConstants.SingleQuote => EnvFileParser.ReadSingleQuotedValue(input),
        _ => EnvFileParser.ReadNormalValue(input)
      };
    }

    private static string ReadDoubleQuotedValue(ReadOnlySpan<char> input)
    {
      int index = 1;
      int length = 0;
      Span<char> output = stackalloc char[input.Length - 2];

      bool done = false;
      while (!done)
      {
        char current = input[index++];
        done = index == input.Length;

        switch (current)
        {
          case ParserConstants.DoubleQuote:
            done = true;
            break;

          case ParserConstants.Escape:
            char next = input[index++];
            if (index == input.Length)
            {
              // last character should be quote and can't be escaped
              return input.AsValue();
            }

            if (ParserConstants.EscapeChars.ContainsKey(next))
            {
              next = ParserConstants.EscapeChars[next];
            }

            output[length++] = next;
            continue;

          default:
            if (index == input.Length)
            {
              // Should end on a quote
              return input.AsValue();
            }

            output[length++] = current;
            continue;
        }
      }

      if (index != input.Length)
      {
        // look for trailing text not inside inline comment
        var rest = input.Slice(index).TrimStart();
        if (!rest.IsComment())
        {
          return input.AsValue();
        }
      }

      return new string(output.Slice(0, length));
    }

    private static string ReadSingleQuotedValue(ReadOnlySpan<char> input)
    {
      int nextQuote = input.IndexOf(ParserConstants.SingleQuote, 1);
      if (nextQuote == -1)
      {
        return input.AsValue();
      }

      var rest = input.Slice(nextQuote + 1).TrimStart();
      if (!rest.IsEmpty && !rest.IsComment())
      {
        return input.AsValue();
      }

      return new string(input.Slice(1, nextQuote - 1));
    }

    private static string ReadNormalValue(ReadOnlySpan<char> input)
    {
      int commentPosition = input.IndexOf('#');
      if (commentPosition != -1)
      {
        input = input.Slice(0, commentPosition);
      }

      return input.AsValue();
    }
  }
}
