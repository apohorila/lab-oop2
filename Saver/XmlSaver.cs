
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parsers;

namespace Saver
{

    public class XmlSaver : ISaver
    {
        public string GenerateContent(List<Student> students)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<Students>");
            
            foreach (var student in students)
            {
                sb.AppendLine($"  <Student FullName=\"{student.FullName}\" Faculty=\"{student.Faculty}\" Department=\"{student.Department}\" Course=\"{student.Course}\">");

                sb.AppendLine("    <Grades>");

                foreach (var grade in student.Grades)
                {
                    sb.AppendLine($"      <Grade Subject=\"{grade.Subject}\" Semester=\"{grade.Semester}\" Mark=\"{grade.Mark}\"/>");
                }
                
                sb.AppendLine("    </Grades>");
                sb.AppendLine("  </Student>");
            }
            
            sb.AppendLine("</Students>");
            return sb.ToString();
        }
    }
}