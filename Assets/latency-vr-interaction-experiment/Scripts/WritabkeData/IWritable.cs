using UnityEngine;
using System.Collections.Generic;

public interface IWritable
{
    public List<string> Columns { get; }
    public List<List<string>> Data { get; }
    public void AddData(List<string> row);
}
