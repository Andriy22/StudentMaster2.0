using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class Ban
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime To { get; set; }
        public DateTime DateOfBan { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
