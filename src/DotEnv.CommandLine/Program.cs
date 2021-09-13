using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace DotEnv
{
  public class Program
  {
    public static int Main(string[] args)
    {
      return Parser.Default.ParseArguments<Options>(args)
        .MapResult(Program.Run, _ => 1);
    }

    internal static Dictionary<string, string> Read(string path)
    {
      IEnumerable<KeyValuePair<string, string>> values = DotNetEnv.Env.LoadMulti(new[] { path });
      return new Dictionary<string, string>(values);
    }

    private static int Run(Options options)
    {
      string envFile = options.Path;
      if (!Path.IsPathRooted(options.Path))
      {
        envFile = Path.Combine(Directory.GetCurrentDirectory(), options.Path);
        envFile = Path.GetFullPath(envFile);
      }

      if (!File.Exists(envFile))
      {
        ConsoleLogger.LogError($"ERROR: Unable to find file '{envFile}'");
        return 1;
      }

      Dictionary<string, string> values = Program.Read(envFile);
      bool log = options.Verbose || options.DryRun;
      string prefix = options.Verbose ? "VERBOSE" : "DRY-RUN";

      if (values.Count > 0)
      {
        if (log)
        {
          ConsoleLogger.LogInfo($"{prefix}: Applying {values.Count} environment variables to current {options.Target}");
        }

        foreach (var (key, value) in values)
        {
          if (log)
          {
            ConsoleLogger.LogInfo($"{prefix}: {key}={value}");
          }

          if (!options.DryRun)
          {
            Environment.SetEnvironmentVariable(key, value, options.Target);
          }
        }
      }
      else if (log)
      {
        ConsoleLogger.LogInfo($"{prefix}: No environment variables found in file '{envFile}'");
      }

      return 0;
    }
  }
}
