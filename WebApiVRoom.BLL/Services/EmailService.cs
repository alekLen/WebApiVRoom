using AutoMapper;
using Google.Apis.YouTube.v3.Data;
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
    public class EmailService :IEmailService
    {

        IUnitOfWork Database { get; set; }
        private readonly IMapper _mapper;

        public EmailService(IUnitOfWork database)
        {
            Database = database;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Email, EmailDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))                   
                    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                    .ForMember(dest => dest.UserClerkId, opt => opt.MapFrom(src => src.User.Clerk_Id))
                    .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary));
            });
            _mapper = new Mapper(config);
        }
        public async Task<EmailDTO> GetEmail(int id)
        {
            try
            {
                Email em = await Database.Emails.GetById(id);
                return _mapper.Map<Email,EmailDTO>(em);
            }
            catch (Exception ex) { return null;  }
        }
        public async Task<EmailDTO> GetEmailByUserPrimary(string clerkId)
        {
            try
            {
                Email em = await Database.Emails.GetByUserPrimary(clerkId);
                return _mapper.Map<Email, EmailDTO>(em);
            }
            catch (Exception ex) { return null; }
        }
        public async Task<List<EmailDTO>> GetAllEmailsByUser(string clerkId)
        {
            try
            {
               List<Email> em = await Database.Emails.GetByUser(clerkId);
                return _mapper.Map<IEnumerable<Email>, IEnumerable<EmailDTO>>(em).ToList();
            }
            catch (Exception ex) { return null; }
        }
        public async Task AddEmail(EmailDTO emDTO){
            Email em = new Email();
            em.EmailAddress = emDTO.EmailAddress;
            em.IsPrimary = emDTO.IsPrimary;
            em.User= await Database.Users.GetByClerk_Id(emDTO.UserClerkId);
            await Database.Emails.Add(em);
        }
        public async Task<EmailDTO> UpdateEmail(EmailDTO emDTO){
            Email em = await Database.Emails.GetById(emDTO.Id);
            em.IsPrimary = emDTO.IsPrimary;
            await Database.Emails.Update(em);
            return emDTO;
        }
        public async Task DeleteEmail(int id)
        {
            await Database.Emails.Delete(id);
        }
    }
}
