using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class VoteService: IVoteService
    {
        IUnitOfWork Database { get; set; }
        //private readonly IOptionsForPostRepository _opService;
        //private readonly IPostRepository _postpService;
        //private readonly IUserRepository _userpService;

        public VoteService(IUnitOfWork database)
        {
            Database = database;
            //_opService = opService;
            //_postpService = postpService;
            //_userpService = userpService;
        }

        public async Task AddVote(int postId, string clerkId, int optionId)
        {
            try
            {
                Vote voute = new Vote();
                Post post= await Database.Posts.GetById(postId);
                User user= await Database.Users.GetByClerk_Id(clerkId);
                OptionsForPost postOptions = post.Options[optionId];

                voute.Option = postOptions;
                voute.User = user;
                voute.Post = post;

                await Database.Votes.Add(voute);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeleteVote(int id)
        {
            try
            {
                await Database.Votes.Delete(id);

            }
            catch { }
        }

       

        public async Task<VoteDTO> GetVote(int id)
        {
            var a = await Database.Votes.GetById(id);

            if (a == null)
                return null;

            VoteDTO v = new VoteDTO();
            v.Id = a.Id;
            v.OptionId=a.Option.Id;
            v.UserId=a.User.Clerk_Id;
            v.PostId=a.Post.Id;

            return v;
        }

        public async Task<VoteDTO> GetVoteByUserAndPost(string clerkId, int postId)
        {
            var a = await Database.Votes.GetVoteByUserAndPost(clerkId, postId);

            if (a == null)
                return null;

            VoteDTO v = new VoteDTO();
            v.Id = a.Id;
            v.OptionId = a.Option.Id;
            v.UserId = a.User.Clerk_Id;
            v.PostId = a.Post.Id;

            return v;
        }
        public async Task<List<OptionVotes>> GetAllVotesByPostIdAndOptionId(int postId)
        {
            try
            {
                List<OptionVotes> votes=new List<OptionVotes>();
                Post post=await Database.Posts.GetById(postId);
                if (post == null) return null;
                for(int i=0;i< post.Options.Count;++i)
                {
                    List<Vote> v = await Database.Votes.GetByPostIdAndOptionId(post.Id, post.Options[i].Id);
                    OptionVotes op=new OptionVotes();
                    if(v != null)
                       op.AllCounts = v.Count;
                    else op.AllCounts = 0;
                    op.Index = i;
                    votes.Add(op);
                }
                return votes;
            }
            catch { return null; }
        }
       
    }
}
