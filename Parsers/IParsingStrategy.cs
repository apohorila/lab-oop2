using System.Collections.Generic;
// using MauiApp26;
namespace Parsers
{
    public interface IParsingStrategy
    {
        List<Student> Parse(string filePath); // Змінено тип
    }
}

// Saver/ISaver.cs
