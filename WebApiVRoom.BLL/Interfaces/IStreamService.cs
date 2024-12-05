using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IStreamService
    {
        Task HandleMessageAsync(string streamId, string message);
    }
}
