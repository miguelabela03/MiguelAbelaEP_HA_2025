using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollRepository
    {
        private PollDbContext myContext;

        // Contructor Injection
        public PollRepository(PollDbContext _myContext)
        {
            myContext = _myContext;
        }

        // This method will return all the list of Polls in the database
        public IQueryable<Poll> GetPolls()
        {
            return myContext.Polls;
        }

        // This method will return a specific poll by the id
        public Poll GetPoll(int pollId)
        {
            return myContext.Polls.SingleOrDefault(x => x.PollId == pollId)!;
        }

        // This method will enable the user to create a poll
        public void CreatePoll(Poll poll)
        {
            myContext.Polls.Add(poll);
            myContext.SaveChanges();
        }
    }
}
