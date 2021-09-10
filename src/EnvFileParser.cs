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

      values = EnvFileParser.ParseLines(File.ReadAllLines(envFile));
      return true;
    }

    private static Dictionary<string, string> ParseLines(IEnumerable<string> lines)
    {
      Dictionary<string, string> values = new(StringComparer.Ordinal);

      foreach (string line in lines)
      {
        if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
        {
          continue;
        }

        string[] parts = line.Split('=', 2);
        if (parts.Length != 2)
        {
          continue;
        }

        values[parts[0]] = parts[1];
      }

      return values;
    }
  }
}
