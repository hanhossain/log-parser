using System.Collections.Generic;

namespace LogParser;

public class Schema
{
    public string Regex { get; set; }

    public List<Column> Columns { get; set; }
}
