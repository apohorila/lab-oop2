using System.Collections.Generic;
using System.Linq;

namespace Parsers
{
    public class Grade
    {
        public string Subject { get; set; }
        public string Semester { get; set; }
        public string Mark { get; set; }
    }
    public class Student
{
        public string FullName { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public string Course { get; set; } 
        public List<Grade> Grades { get; set; } = new List<Grade>();
        public string AllGradesDisplay => string.Join("; ", Grades.Select(g => $"{g.Subject}: {g.Mark}"));
    }
}