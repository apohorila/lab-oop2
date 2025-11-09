
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Parsers
{
    public class LINQParsingStrategy : IParsingStrategy
    {
        public List<Student> Parse(string filePath)
        {
            try
            {
                var document = XDocument.Load(filePath);
                
                var students = document.Descendants("Student")
                    .Select(s => new Student
                    {
                        FullName = s.Attribute("FullName")?.Value,
                        Faculty = s.Attribute("Faculty")?.Value,
                        Department = s.Attribute("Department")?.Value,
                        Course = s.Attribute("Course")?.Value,
                         
                        Grades = s.Element("Grades")?
                                  .Descendants("Grade")
                                  .Select(g => new Grade
                                  {
                                      Subject = g.Attribute("Subject")?.Value,
                                      Semester = g.Attribute("Semester")?.Value?.Replace("Семестр ", ""),
                                      Mark = g.Attribute("Mark")?.Value
                                  }).ToList() ?? new List<Grade>()
                    })
                    .Where(s => !string.IsNullOrEmpty(s.FullName))
                    .ToList();

                if (students.Count == 0)
                {
                    throw new Exception("LINQ to XML: Не знайдено дійсних даних про студентів в XML.");
                }

                return students;
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка LINQ to XML парсингу: {ex.Message}");
            }
        }
    }
}