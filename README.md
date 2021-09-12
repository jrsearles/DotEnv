# dotenv-tool

[![Nuget](https://img.shields.io/nuget/v/dotenv-tool.svg)](https://www.nuget.org/packages/dotenv-tool)

`dotenv-tool` is a dotnet tool which reads key-value pairs from `.env` files and sets them as environment variables.

## How To Install

`dotenv-tool` can be installed as a dotnet tool. When installed, a `dotenv` command will be added to the path.

```shell
dotnet tool install -g dotenv-tool
```

## Usage

### Examples

| Command                      | Description                                                                                   |
| ---------------------------- | --------------------------------------------------------------------------------------------- |
| `dotenv`                     | Looks for a `.env` file in the current directory and loads the values in the current process. |
| `dotenv --file=.env.staging` | Loads environment variables from the specified file                                           |
| `dotenv --target=Machine`    | Applies environment variables at the Machine level                                            |

### Options

- `--target`: sets the target for the variables. Either Process (default), Machine, or User
- `--dry-run`: displays the variables that would be loaded, but does not apply them
- `--verbose`: displays information to the stdout as variables are loaded

## How To Uninstall

```shell
dotnet tool uninstall -g dotenv-tool
```
