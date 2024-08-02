using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IUnitOfWork
    {
      
        IUserRepository Users { get; }
        Task Save();
    }
}
