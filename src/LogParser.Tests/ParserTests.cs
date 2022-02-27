using System.Collections.Generic;
using Xunit;

namespace LogParser.Tests;

public class ParserTests
{
    [Fact]
    public void Parse_Match()
    {
        string input = "1 one uno";
        var schema = new Schema()
        {
            Regex = @"(?<decimal>\d+) (?<english>\w+) (?<spanish>\w+)",
            Columns = new List<Column>()
            {
                new Column()
                {
                    Name = "decimal"
                },
                new Column()
                {
                    Name = "english"
                },
                new Column()
                {
                    Name = "spanish"
                }
            }
        };

        var parser = new Parser(schema);
        Assert.True(parser.TryParse(input, out var result));

        var expected = new Dictionary<string, string>()
        {
            ["decimal"] = "1",
            ["english"] = "one",
            ["spanish"] = "uno"
        };
        Assert.All(result, x => Assert.Equal(expected[x.Key], x.Value));
    }

    [Fact]
    public void Parse_NoMatch()
    {
        string input = "asdfasdf";
        var schema = new Schema()
        {
            Regex = @"(?<decimal>\d+) (?<english>\w+) (?<spanish>\w+)",
            Columns = new List<Column>()
            {
                new Column()
                {
                    Name = "decimal"
                },
                new Column()
                {
                    Name = "english"
                },
                new Column()
                {
                    Name = "spanish"
                }
            }
        };

        var parser = new Parser(schema);
        Assert.False(parser.TryParse(input, out _));
    }
}
