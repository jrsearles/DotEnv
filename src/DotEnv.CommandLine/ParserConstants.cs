using System.Collections.Generic;

namespace DotEnv
{
  internal static class ParserConstants
  {
    public const char SingleQuote = '\'';
    public const char DoubleQuote = '"';
    public const char Comment = '#';
    public const char Escape = '\\';

    public static readonly Dictionary<char, char> EscapeChars = new() { { 'n', '\n' }, { 'r', '\r' }, { 't', '\t' } };
  }
}
