using System;
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

    private static int Run(Options options)
    {
      string envFile = options.Path;
      if (!Path.IsPathRooted(options.Path))
      {
        envFile = Path.Combine(Directory.GetCurrentDirectory(), options.Path);
        envFile = Path.GetFullPath(envFile);
      }

      if (!EnvFileParser.TryParse(envFile, out var values))
      {
        Console.WriteLine($"ERROR: Unable to find file '{envFile}'");
        return 1;
      }

      bool log = options.Verbose || options.DryRun;
      string prefix = options.Verbose ? "VERBOSE" : "DRY-RUN";

      if (values.Count > 0)
      {
        if (log)
        {
          Console.WriteLine($"{prefix}: Applying {values.Count} environment variables to current {options.Target}");
        }

        foreach (var (key, value) in values)
        {
          if (log)
          {
            Console.WriteLine($"{prefix}: {key}={value}");
          }

          if (!options.DryRun)
          {
            Environment.SetEnvironmentVariable(key, value, options.Target);
          }
        }
      }
      else if (log)
      {
        Console.WriteLine($"{prefix}: No environment variables found in file '{envFile}'");
      }

      return 0;
    }
  }
}
