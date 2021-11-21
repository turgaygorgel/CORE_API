using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Department { get; set; }

        public string Registerdate { get; set; }

        public string PhotoFileName { get; set; }
    }
}
