using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Chat
{
    public class ChatMessageVM
    {
        public string Message { get; set; }
        public string FullName { get; set; }
        public string SenderId { get; set; }
        public string Date { get; set; }
        public string Color { get; set; }
    }
}
