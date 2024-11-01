using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class OptionsForPostService : IOptionsForPostService
    {
        IUnitOfWork Database { get; set; }
       

        public OptionsForPostService(IUnitOfWork database )
        {
            Database = database;
        }

        public async Task AddOptionsForPost(OptionsForPostDTO vDTO)
        {
            try
            {
                OptionsForPost option = new OptionsForPost();               
                option.Name=vDTO.Name;

                await Database.Options.Add(option);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeleteOptions(int id)
        {
            try
            {
                await Database.Options.Delete(id);

            }
            catch { }
        }
    }
}
