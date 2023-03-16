// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AupliCommandLineTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.CommandLine.AcceptanceTests.Samples.Aupli;

using FluentAssertions;
using Sundew.Base.Primitives.Computation;
using Xunit;

public class AupliCommandLineTests
{
    private const string ExpectedLogPath = @"c:\temp\log.log";
    private const long ExpectedMaxLogFileSizeInBytes = 400000;

    [Fact]
    public void Given_a_commandline_with_a_complex_type_Then_the_complex_type_should_be_parsed_successfully()
    {
        var commandLine = $@"-s -fl --log-path ""{ExpectedLogPath}"" --max-size {ExpectedMaxLogFileSizeInBytes} -cl";
        var commandLineParser = new CommandLineParser<Options, int>();
        commandLineParser.WithArguments(
            new Options(false, false, new FileLogOptions(@"Logs\Aupli.log")),
            options => R.Success(options));

        var result = commandLineParser.Parse(commandLine);

        result.IsSuccess.Should().BeTrue();
        result.Value.AllowShutdown.Should().BeTrue();
        result.Value.IsLoggingToConsole.Should().BeTrue();
        result.Value.FileLogOptions?.LogPath.Should().Be(ExpectedLogPath);
        result.Value.FileLogOptions?.MaxLogFileSizeInBytes.Should().Be(ExpectedMaxLogFileSizeInBytes);
    }

    [Fact]
    public void Given_a_commandline_with_an_optional_complex_type_with_a_required_field_When_the_complex_type_is_provided_but_its_required_argument_is_not_Then_parsing_the_command_line_should_fail()
    {
        var commandLine = $@"-s -fl --max-size {ExpectedMaxLogFileSizeInBytes} -cl";
        var commandLineParser = new CommandLineParser<Options, int>();
        commandLineParser.WithArguments(
            new Options(false, false),
            options => R.Success(options));

        var result = commandLineParser.Parse(commandLine);

        result.IsSuccess.Should().BeFalse();
        result.Error.Type.Should().Be(ParserErrorType.InnerParserError);
        result.Error.InnerParserError!.Type.Should().Be(ParserErrorType.RequiredArgumentMissing);
    }

    [Fact]
    public void Given_a_commandline_with_an_optional_complex_type_with_a_required_field_When_the_complex_type_is_not_provided_Then_the_command_line_should_be_parsed_successfully()
    {
        var commandLine = $@"-s -cl";
        var commandLineParser = new CommandLineParser<Options, int>();
        commandLineParser.WithArguments(
            new Options(false, false),
            options => R.Success(options));

        var result = commandLineParser.Parse(commandLine);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Given_options_with_a_complex_type_Then_the_commandline_should_be_generated_successfully()
    {
        var commandLineGenerator = new CommandLineGenerator();

        var result = commandLineGenerator.Generate(
            new Options(
                true,
                true,
                new FileLogOptions(ExpectedLogPath, ExpectedMaxLogFileSizeInBytes)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should()
            .Be($@"-fl -lp ""{ExpectedLogPath}"" -ms {ExpectedMaxLogFileSizeInBytes} -mf 10 -cl -s");
    }

    [Fact]
    public void Given_options_with_a_complex_type_When_complex_type_is_null_Then_help_text_should_contain_indentation()
    {
        const string ExpectedHelpText = @"Help
 Arguments:            Launches Aupli
  -fl  | --file-log    | Specifies whether to use a File logger and it's options | Default: [none]
    -lp | --log-path    | Specifies the log path, in case of the File logger      | Required
    -ms | --max-size    | Specifies max log file size in bytes.                   | Default: 5000000
    -mf | --max-files   | Specifies max number of log files.                      | Default: 10
  -cl  | --console-log | Specifies whether to use a Console logger
  -s   | --shutdown    | Allows Aupli to shutdown the device when closing.
";
        var commandLineParser = new CommandLineParser<Options, int>();
        commandLineParser.WithArguments(new Options(false, false), options => R.Success(options));

        var helpText = commandLineParser.CreateHelpText();

        helpText.Should().Be(ExpectedHelpText);
    }

    [Fact]
    public void Given_options_with_a_complex_type_When_complex_type_is_not_null_Then_help_text_should_contain_indentation()
    {
        const string expectedPath = @"c:\temp\log.log";
        string expectedHelpText = $@"Help
 Arguments:            Launches Aupli
  -fl  | --file-log    | Specifies whether to use a File logger and it's options | Default: see below
    -lp | --log-path    | Specifies the log path, in case of the File logger      | Default: {expectedPath}
    -ms | --max-size    | Specifies max log file size in bytes.                   | Default: 5000000
    -mf | --max-files   | Specifies max number of log files.                      | Default: 10
  -cl  | --console-log | Specifies whether to use a Console logger
  -s   | --shutdown    | Allows Aupli to shutdown the device when closing.
";
        var commandLineParser = new CommandLineParser<Options, int>();
        commandLineParser.WithArguments(new Options(false, false, new FileLogOptions(expectedPath)), options => R.Success(options));

        var helpText = commandLineParser.CreateHelpText();

        helpText.Should().Be(expectedHelpText);
    }

    [Fact]
    public void Given_the_help_commandline_Then_ResultErrorToString_should_be_the_expected_help_text()
    {
        string expectedHelpText = $@"Help
 Arguments:            Launches Aupli
  -fl  | --file-log    | Specifies whether to use a File logger and it's options | Default: [none]
    -lp | --log-path    | Specifies the log path, in case of the File logger      | Required
    -ms | --max-size    | Specifies max log file size in bytes.                   | Default: 5000000
    -mf | --max-files   | Specifies max number of log files.                      | Default: 10
  -cl  | --console-log | Specifies whether to use a Console logger
  -s   | --shutdown    | Allows Aupli to shutdown the device when closing.
";
        var commandLineParser = new CommandLineParser<Options, int>();
        commandLineParser.WithArguments(new Options(false, false), options => R.Success(options));
        var parserResult = commandLineParser.Parse("-?");

        var result = parserResult.Error.ToString();

        result.Should().Be(expectedHelpText);
    }

    [Fact]
    public void Given_a_commandline_without_a_required_argument_Then_ResultErrorToString_should_be_verb_not_registered()
    {
        string expectedText = $@"Error:
  The verb <empty> is unknown.";
        var commandLineParser = new CommandLineParser<Options, int>();
        commandLineParser.WithArguments(new Options(false, false), options => R.Success(options));
        var parserResult = commandLineParser.Parse($@"-fl """"");

        var result = parserResult.Error.ToString();

        result.Should().Be(expectedText);
    }

    [Fact]
    public void Given_a_nested_commandline_without_a_required_argument_Then_ResultErrorToString_should_be_the_expected_text()
    {
        string expectedText = $@"Error:
  Inner required argument was missing while parsing: -fl/--file-log with argument: -ms.
    The required options were missing:
     -lp/--log-path";
        var commandLineParser = new CommandLineParser<Options, int>();
        commandLineParser.WithArguments(new Options(false, false), options => R.Success(options));
        var parserResult = commandLineParser.Parse($@"-fl -ms 3");

        var result = parserResult.Error.ToString();

        result.Should().Be(expectedText);
    }

    [Fact]
    public void Given_a_commandline_with_an_option_without_an_argument_Then_ResultErrorToString_should_be_the_expected_text()
    {
        string expectedText = $@"Error:
  The argument for the option: -lp/--log-path is missing.";
        var commandLineParser = new CommandLineParser<FileLogOptions, int>();
        commandLineParser.WithArguments(new FileLogOptions(string.Empty), options => R.Success(options));
        var parserResult = commandLineParser.Parse($@"-lp");

        var result = parserResult.Error.ToString();

        result.Should().Be(expectedText);
    }

    [Fact]
    public void Given_a_commandline_with_an_option_with_an_empty_argument_Then_Result_should_have_empty_string()
    {
        var commandLineParser = new CommandLineParser<FileLogOptions, int>();
        var fileLogOptions = commandLineParser.WithArguments(new FileLogOptions(string.Empty), options => R.Success(options));

        var parserResult = commandLineParser.Parse($@"-lp """"");

        parserResult.IsSuccess.Should().BeTrue();
        fileLogOptions.LogPath.Should().BeEmpty();
    }

    [Fact]
    public void Given_a_commandline_When_parsing_twice_Then_ResultErrorToString_should_be_the_expected_text()
    {
        const string expectedLogPath = @"c:\temp\log2.txt";
        const int expectedMaxFiles = 10;
        var commandLine = $@"-lp ""c:\temp\log.txt"" --max-size {ExpectedMaxLogFileSizeInBytes} --max-files 2";

        var commandLineParser = new CommandLineParser<FileLogOptions, int>();
        commandLineParser.WithArguments(new FileLogOptions(string.Empty), options => R.Success(options));
        commandLineParser.Parse(commandLine);

        var result = commandLineParser.Parse($@"-lp ""{expectedLogPath}""");

        result.Value.LogPath.Should().Be(expectedLogPath);
        result.Value.MaxLogFileSizeInBytes.Should().NotBe(ExpectedMaxLogFileSizeInBytes);
        result.Value.MaxNumberOfLogFiles.Should().Be(expectedMaxFiles);
    }
}