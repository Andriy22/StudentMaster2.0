using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; }
        public User Owner { get; set; }
        public User Teacher { get; set; }
        public Subject Subject { get; set; }
        public WorkType Type { get; set; }
        public DateTime Date { get; set; }
    }
}
