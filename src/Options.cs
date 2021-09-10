using System;
using CommandLine;

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
  }
}
