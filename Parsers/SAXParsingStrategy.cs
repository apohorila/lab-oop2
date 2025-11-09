using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Parsers
{
    public class SAXParsingStrategy : IParsingStrategy
    {
 
        public List<Student> Parse(string filePath)
        {
            var students = new List<Student>();
            Student currentStudent = null; 

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Student":
                                currentStudent = new Student
                                {
                                    FullName = reader.GetAttribute("FullName"),
                                    Faculty = reader.GetAttribute("Faculty"),
                                    Department = reader.GetAttribute("Department"),
                                    Course = reader.GetAttribute("Course") 
                                };
                                
                                if (!string.IsNullOrEmpty(currentStudent.FullName))
                                {
                                    students.Add(currentStudent);
                                }
                                break;

                            case "Grade":
                                if (currentStudent != null)
                                {
                                    var grade = new Grade
                                    {
                                        Subject = reader.GetAttribute("Subject"),
                                        Semester = reader.GetAttribute("Semester")?.Replace("Семестр ", ""),
                                        Mark = reader.GetAttribute("Mark")
                                    };
                                    
                                    if (!string.IsNullOrEmpty(grade.Subject))
                                    {
                                        currentStudent.Grades.Add(grade);
                                    }
                                }
                                break;
                            
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                         if (reader.Name == "Student")
                         {
                             currentStudent = null;
                         }
                    }
                }
            }

            if (students.Count == 0 || students.All(s => string.IsNullOrEmpty(s.FullName)))
            {
                 throw new Exception("SAX Parsing: Не знайдено дійсних даних про студентів в XML.");
            }
            
            return students;
        }
    }
}