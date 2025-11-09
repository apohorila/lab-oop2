
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parsers;

namespace Saver
{
    public class HtmlSaver : ISaver
    {
        public string GenerateContent(List<Student> students)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<title>Звіт успішності студентів</title>"); 
            sb.AppendLine("<style>");
            sb.AppendLine("table { border-collapse: collapse; width: 100%; font-family: Arial, sans-serif; }");
            sb.AppendLine("th, td { border: 1px solid black; padding: 10px; text-align: left; }");
            sb.AppendLine("th { background-color: #f2f2f2; font-weight: bold; }");
            sb.AppendLine("tr:nth-child(even) { background-color: #f9f9f9; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            

            sb.AppendLine("<h1>Успішність студентів (Фільтровані дані)</h1>");
            sb.AppendLine("<table>");
            
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>П.І.П.</th>");
            sb.AppendLine("<th>Факультет</th>");
            sb.AppendLine("<th>Кафедра</th>");
            sb.AppendLine("<th>Курс</th>");
            sb.AppendLine("<th>Успішність (Дисципліна, Оцінка, Семестр)</th>"); 
            sb.AppendLine("</tr>");
            

            foreach (var student in students)
            {

                var gradesText = student.AllGradesDisplay; 

                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{student.FullName}</td>");
                sb.AppendLine($"<td>{student.Faculty}</td>");
                sb.AppendLine($"<td>{student.Department}</td>");
                sb.AppendLine($"<td>{student.Course}</td>");
                sb.AppendLine($"<td>{gradesText}</td>"); 
                sb.AppendLine("</tr>");
            }
            
            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }
}