using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollRepository : IPollRepository
    {
        private PollDbContext myContext;

        // Contructor Injection
        public PollRepository(PollDbContext _myContext)
        {
            myContext = _myContext;
        }

        // This method will return all the list of Polls in the database
        public List<Poll> GetPolls()
        {
            return myContext.Polls.ToList();
        }

        // This method will enable the user to create a poll
        public void CreatePoll(Poll poll)
        {
            myContext.Polls.Add(poll);
            myContext.SaveChanges();
        }

        // This method will enabel the user to vote within a poll
        public void Vote(int pollId, int voteChosen)
        {
            // Getting the chosen poll by the id
            var poll = myContext.Polls.FirstOrDefault(x => x.PollId == pollId);

            if (poll != null)
            {
                // Incrementing the votes count variable according to the chosen vote
                switch(voteChosen)
                {
                    case 1:
                        poll.Option1VotesCount++;
                        break;
                    case 2:
                        poll.Option2VotesCount++;
                        break;
                    case 3:
                        poll.Option3VotesCount++;
                        break;
                    default:
                        throw new ArgumentException("Invalid option selected");
                }
            }

            // Saving the changes in the database
            myContext.SaveChanges();
        }
    }
}
