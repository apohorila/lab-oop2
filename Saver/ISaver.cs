using Parsers;
using System.Collections.Generic;
// ...
namespace Saver
{
    public interface ISaver
    {
        string GenerateContent(List<Student> students); 
    }
}