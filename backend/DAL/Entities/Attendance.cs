using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        // Composite key: StudentId + SubjectId 
        public string StudentId { get; set; }
        public int SubjectId { get; set; }

        // Other properties 
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        // Navigation properties 
        [ForeignKey("StudentId")]
        public User Student { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
    }
}
