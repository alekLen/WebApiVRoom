using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class ContentReport
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public string? AdminId { get; set; }
        public string Title { get; set; }
        public string? AdminAnswer { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int SubjectId { get; set; }
    }
}
