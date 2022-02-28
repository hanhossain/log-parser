using System;
using System.IO;
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

        if (parser.TryParse(lines[0], out var result))
        {
            foreach (var (key, value) in result)
            {
                Console.WriteLine($"{key}: {value}");
            }
        }
    }
}
