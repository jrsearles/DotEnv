using System.IO;
using Xunit;

namespace DotEnv.Tests
{
  public class EnvFileParserTests
  {
    [Theory]
    [InlineData("BASIC", "basic")]
    [InlineData("AFTER_LINE", "after_line")]
    [InlineData("EMPTY", "")]
    [InlineData("SINGLE_QUOTES", "single_quotes")]
    [InlineData("SINGLE_QUOTES_SPACED", "    single quotes    ")]
    [InlineData("DOUBLE_QUOTES", "double_quotes")]
    [InlineData("DOUBLE_QUOTES_SPACED", "    double quotes    ")]
    [InlineData("EXPAND_NEWLINES", "expand\nnew\nlines")]
    [InlineData("DONT_EXPAND_UNQUOTED", "dontexpand\\nnewlines")]
    [InlineData("DONT_EXPAND_SQUOTED", "dontexpand\\nnewlines")]
    [InlineData("EQUAL_SIGNS", "equals==")]
    [InlineData("RETAIN_INNER_QUOTES", "{\"foo\": \"bar\"}")]
    [InlineData("RETAIN_LEADING_DQUOTE", "\"retained")]
    [InlineData("RETAIN_LEADING_SQUOTE", "'retained")]
    [InlineData("RETAIN_TRAILING_DQUOTE", "retained\"")]
    [InlineData("RETAIN_TRAILING_SQUOTE", "retained'")]
    [InlineData("RETAIN_INNER_QUOTES_AS_STRING", "{\"foo\": \"bar\"}")]
    [InlineData("TRIM_SPACE_FROM_UNQUOTED", "some spaced out string")]
    [InlineData("USERNAME", "therealnerdybeast@example.tld")]
    [InlineData("SPACED_KEY", "parsed")]
    [InlineData("IGNORE_TRAILING_COMMENT", "ignore_trailing_comment")]
    [InlineData("IGNORE_TRAILING_COMMENT_DQUOTE", "ignore_trailing_comment")]
    [InlineData("IGNORE_TRAILING_COMMENT_SQUOTE", "ignore_trailing_comment")]
    [InlineData("RETAIN_COMMENT_DQUOTE", "# retained")]
    [InlineData("RETAIN_COMMENT_SQUOTE", "# retained")]
    public void TryParseFile_TestCases(string key, string expected)
    {
      string envFile = Path.Combine(Directory.GetCurrentDirectory(), "test.env");
      bool parsed = EnvFileParser.TryParse(envFile, out var values);

      Assert.True(parsed);
      Assert.Equal(expected, values[key]);
    }

    [Fact]
    public void TryParseFile_IgnoresCommentsAndWhiteSpace()
    {
      string envFile = Path.Combine(Directory.GetCurrentDirectory(), "test.env");
      bool parsed = EnvFileParser.TryParse(envFile, out var values);

      Assert.True(parsed);
      Assert.Equal(25, values.Count);
      Assert.False(values.ContainsKey("COMMENTS"));
    }

    [Theory]
    [InlineData("SERVER=localhost\rPASSWORD=password\rDB=tests\r")]
    [InlineData("SERVER=localhost\nPASSWORD=password\nDB=tests\n")]
    [InlineData("SERVER=localhost\r\nPASSWORD=password\r\nDB=tests\r\n")]

    public void TryParseFile_WithDifferentLineEndings(string content)
    {
      string envFile = Path.GetTempFileName();

      try
      {
        File.WriteAllText(envFile, content);
        
        bool parsed = EnvFileParser.TryParse(envFile, out var values);

        Assert.True(parsed);
        Assert.Equal("localhost", values["SERVER"]);
        Assert.Equal("password", values["PASSWORD"]);
        Assert.Equal("tests", values["DB"]);
      }
      finally
      {
        File.Delete(envFile);
      }
    }
  }
}
