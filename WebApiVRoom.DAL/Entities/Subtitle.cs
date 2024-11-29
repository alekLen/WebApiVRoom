using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class Subtitle
    {
        public int Id { get; set; }
        public Video Video { get; set; }
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public bool IsPublished { get; set; }
        public string PuthToFile { get; set; }
    }
}
