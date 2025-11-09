
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LogLibrary;

namespace Parsers
{
    public class DOMParsingStrategy : IParsingStrategy
    {
        public List<Student> Parse(string filePath)
        {
            var students = new List<Student>();
            var document = new XmlDocument();
            document.Load(filePath);

            try
            {
                var studentNodes = document.GetElementsByTagName("Student");
                if (studentNodes.Count == 0)
                {
                    throw new Exception("XML-файл не містить елементів <Student> (Успішність студентів).");
                }

                foreach (XmlNode studentNode in studentNodes)
                {
                    var student = new Student
                    {
                        FullName = studentNode.Attributes?["FullName"]?.Value,
                        Faculty = studentNode.Attributes?["Faculty"]?.Value,
                        Department = studentNode.Attributes?["Department"]?.Value,
                        Course = studentNode.Attributes?["Course"]?.Value 
                    };
                    

                    if (string.IsNullOrEmpty(student.FullName))
                    {
                        continue; 
                    }


                    var gradesNode = studentNode["Grades"];
                    if (gradesNode != null)
                    {

                        foreach (XmlNode gradeNode in gradesNode.ChildNodes)
                        {
                            if (gradeNode.Name == "Grade")
                            {
                                var grade = new Grade
                                {

                                    Subject = gradeNode.Attributes?["Subject"]?.Value,
                                    Semester = gradeNode.Attributes?["Semester"]?.Value?.Replace("Семестр ", ""),
                                    Mark = gradeNode.Attributes?["Mark"]?.Value
                                };
                                
                                if (!string.IsNullOrEmpty(grade.Subject))
                                {
                                    student.Grades.Add(grade);
                                }
                            }
                        }
                    }

                    students.Add(student);
                }

                if (students.Count == 0)
                {
                    throw new Exception("XML-файл не містить дійсних даних про студентів.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка структури XML при DOM-парсингу: {ex.Message}");
            }

            return students;
        }
    }
}