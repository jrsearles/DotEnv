using System;
using System.Collections;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace DotEnv
{
  public sealed class Options
  {
    [Value(0, Default = ".env", MetaName = "path", HelpText = "Path to the .env file to load")]
    public string Path { get; set; }

    [Option(shortName: 't', longName: "target", Default = EnvironmentVariableTarget.Process, HelpText = "Indicates whether the environment variable should be applied to the current process (default), the current user, or the machine.")]
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
        yield return new Example("Run using local .env file", new UnParserSettings(), new Options());
        yield return new Example("Run using specific file", new UnParserSettings(), new Options { Path = ".env.local" });
        yield return new Example("Run applying variables to Machine level", new UnParserSettings(),
          new Options { Target = EnvironmentVariableTarget.Machine });
        yield return new Example("Run without applying changes", new UnParserSettings(), new Options { DryRun = true });
      }
    }
  }
}
