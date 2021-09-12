using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace DotEnv
{
  public sealed class Options
  {
    [Option(shortName: 'f', longName: "file", Default = ".env", HelpText = "Location of the .env file. Defaults to .env file in current working directory.")]
    public string Path { get; set; }

    [Option(shortName: 't', longName: "target", Default = EnvironmentVariableTarget.Process, HelpText = "Indicates whether the environment variable should be applied to the current process (default), the current user, or the machine. Valid values 'Process', 'Machine', or 'User'.")]
    public EnvironmentVariableTarget Target { get; set; }

    [Option(shortName: 'v', longName: "verbose", Default = false)]
    public bool Verbose { get; set; }

    [Option(longName: "dry-run", Default = false, HelpText = "Loads and parses the .env file, but does not apply environment variable changes.")]
    public bool DryRun { get; set; }

    [Usage(ApplicationAlias = "dotenv")]
    public static IEnumerable<Example> Examples
    {
      get
      {
        var settings = UnParserSettings.WithUseEqualTokenOnly();
        yield return new Example("Run using local .env file", settings, new Options());
        yield return new Example("Run using specified file", settings, new Options { Path = ".env.local" });
        yield return new Example("Apply variables to Machine level", settings,
          new Options { Target = EnvironmentVariableTarget.Machine });
        yield return new Example("Run without applying changes", settings, new Options { DryRun = true });
      }
    }
  }
}
