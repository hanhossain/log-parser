using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogParser;

public class Parser
{
    private readonly Regex _regex;

    public Parser(Schema schema)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        _regex = new Regex(Schema.Regex);
    }

    public Schema Schema { get; }

    public bool TryParse(string line, out Dictionary<string, string> result)
    {
        var match = _regex.Match(line);

        if (!match.Success)
        {
            result = null;
            return false;
        }

        result = Schema.Columns.ToDictionary(x => x.Name, x => match.Groups[x.Name].Value);
        return true;
    }
}
