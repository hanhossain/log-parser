using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LogParser;

public class Program
{
    public static async Task Main(string[] args)
    {
        var path = "";
        var schemaPath = "";
        var schemaRaw = await File.ReadAllTextAsync(schemaPath);

        var lines = await File.ReadAllLinesAsync(path);
        var line = lines[0];

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var schema = deserializer.Deserialize<Schema>(schemaRaw);
        var parser = new Parser(schema);

        foreach (var (key, value) in parser.Parse(line))
        {
            Console.WriteLine($"{key}: {value}");
        }
    }
}
