using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class Subject
    {
        public Subject()
        {
            Teachers = new HashSet<User>();
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<User> Teachers { get; set; }
        public ICollection<Work> Works { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
