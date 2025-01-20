using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class ContentReportDTO
    {
        public int Id { get; set; }
        
        [Required]
        public string SenderUserId { get; set; }
        public string? AdminId { get; set; }
        public string? AdminAnswer { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }
        
        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }
        
        [Required]
        public string Type { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int SubjectId { get; set; }
    }
}
