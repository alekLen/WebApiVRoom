using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{
    public class SubtitleDTO
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public bool IsPublished { get; set; }
        public string? PuthToFile { get; set; }
    }
}
