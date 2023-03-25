using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class ConfirmCode
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User user { get; set; }
        public int Code { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsUsed { get; set; }
    }
}
