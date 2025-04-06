using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserVoteRepository
    {
        private PollDbContext myContext;

        // Contructor Injection
        public UserVoteRepository(PollDbContext _myContext)
        {
            myContext = _myContext;
        }

        // This method will get all the user votes
        public IQueryable<UserVote> GetUserVotes()
        {
            return myContext.UserVotes;
        }

        // This method will get a single row from the table
        public UserVote GetUserVote(int pollId, Guid userId)
        {
            return myContext.UserVotes.SingleOrDefault(x => x.PollFk == pollId && x.UserFk == userId);
        }

        // This method will add the user vote to the history
        public void AddUserVote(UserVote userVote)
        {
            myContext.UserVotes.Add(userVote);
            myContext.SaveChanges();
        }
    }
}
