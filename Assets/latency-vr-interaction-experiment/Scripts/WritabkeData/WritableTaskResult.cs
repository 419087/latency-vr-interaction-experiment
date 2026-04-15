using UnityEngine;
using System.Collections.Generic;

public class WritableData : IWritable
{
    public List<string> Columns { get; }
    public List<List<string>> Data { get; }

    public WritableData(List<string> columns, List<List<string>> data)
    {
        Columns = columns;
        Data = data;
    }

    public void AddData(List<string> row)
    {
        Data.Add(row);
    }
}