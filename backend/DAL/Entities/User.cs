using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            TeacherGroups = new HashSet<TeacherGroup>();
            Subjects = new HashSet<Subject>();
            Grades= new HashSet<Grade>();
        }

        public string FirstName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastOnline { get; set; }
        public bool IsDeleted { get; set; }
        public Attachment Img { get; set; }

        // student props
        public Group Group { get; set; }
        public ICollection<Grade> Grades { get; set; }

        // teacher props
        public ICollection<TeacherGroup> TeacherGroups { get; set; }
        public ICollection<Subject> Subjects { get; set; }
    }
}
