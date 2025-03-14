using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class PinnedVideoDTO
    {
        public int Id { get; set; }
        
        public int VideoId { get; set; }
        public int ChannelSettingsId { get; set; }

        
    }
}
