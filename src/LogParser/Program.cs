using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LogParser;

public class Program
{
    /// <param name="schema">Path to the schema definition</param>
    /// <param name="path">Path to the log file</param>
    public static async Task Main(string schema, string path)
    {
        var schemaRaw = await File.ReadAllTextAsync(schema);
        var lines = await File.ReadAllLinesAsync(path);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var schemaDefinition = deserializer.Deserialize<Schema>(schemaRaw);
        var parser = new Parser(schemaDefinition);
        var parsedLines = new List<Dictionary<string, string>>();

        foreach (var line in lines)
        {
            if (parser.TryParse(line, out var parsedLine))
            {
                parsedLines.Add(parsedLine);
            }
            else
            {
                var lastLine = parsedLines.LastOrDefault();
                if (lastLine != null)
                {
                    var lastColumn = schemaDefinition.Columns.Last();

                    // TODO: use a string builder to prevent unnecessary allocations
                    lastLine[lastColumn.Name] += $"{Environment.NewLine}{line}";
                }
            }
        }

        Console.WriteLine(JsonSerializer.Serialize(parsedLines));
    }
}
