using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.EF;


namespace WebApiVRoom.BLL.Infrastructure
{
    public static class VRoomContextExtensions
    {
        public static void AddVRoomContext(this IServiceCollection services, string connection)
        {
            services.AddDbContext<VRoomContext>(options => options.UseSqlServer(connection));
        }
    }
}
