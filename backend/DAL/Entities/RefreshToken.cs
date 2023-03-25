using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class RefreshToken
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        [Required, StringLength(128)]
        public string Token { get; set; }
        public User @User { get; set; }
        public DateTime ToLife { get; set; }
    }
}
