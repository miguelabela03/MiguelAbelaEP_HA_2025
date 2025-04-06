using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Presentation.ActionFilters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        private PollRepository _pollRepository;
        private UserVoteRepository _userVoteRepository;

        // Constructor Injection
        public PollController(PollRepository pollRepository, UserVoteRepository userVoteRepository)
        {
            _pollRepository = pollRepository;
            _userVoteRepository = userVoteRepository;
        }

        // This method will get all the polls from the database via the reposiotry and pass them to the view
        [HttpGet]
        public IActionResult List()
        {
            // Getting the polls and ordering them by latest poll
            var pollList = _pollRepository.GetPolls()
                            .OrderBy(x => x.DateCreated)
                            .ToList();

            return View(pollList); // Passing the fethced polls into the view
        }

        // This method is used to create a poll and show the empty textboxes
        [HttpGet]
        public IActionResult CreatePoll()
        {
            Poll myPoll = new Poll(); 
            return View(myPoll); // Passing the poll fields to the view
        }

        // This method will add a new poll to the database once the submit button is trigerred
        [HttpPost]
        public IActionResult CreatePoll(Poll poll) 
        { 
            if(ModelState.IsValid)
            {
                // Setting the current date and time automatically for when the poll was created
                poll.DateCreated = DateTime.Now;
                // Here the poll is being saved
                _pollRepository.CreatePoll(poll);
                // Showing the success alert message
                TempData["message"] = "Poll was added successfully";
                // Retun to the polls list
                return RedirectToAction("List");
            }

            // Displaying error messages if above state is not valid
            TempData["error"] = "Check your inputs!";
            //Getting the poll fields
            Poll myPoll = new Poll();
            // Passing the fields into the view
            return View(myPoll);
        }

        // This method will display the details of a poll
        [HttpGet]
        public IActionResult ViewPollDetails(int pollId)
        {
            var pollDetails = _pollRepository.GetPolls()
                                .SingleOrDefault(x => x.PollId == pollId)!;

            // Returning the poll details to the view
            return View(pollDetails);
        }

        [Authorize]
        [ServiceFilter(typeof(VotesActionFilter))]
        [HttpGet]
        public IActionResult Vote(int pollId)
        {
            var pollDetails = _pollRepository.GetPolls()
                                .SingleOrDefault(x => x.PollId == pollId);

            return View(pollDetails);
        }


        // This method will save the user vote
        [HttpPost]
        public IActionResult Vote(int pollId, int vote)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Update the vote count, and update voting history
            _pollRepository.Vote(pollId, vote);

            UserVote voteHistory = new UserVote()
            {
                PollFk = pollId,
                UserFk = Guid.Parse(userId),
                VoteDateCreated = DateTime.Now,
            };

            // Update user votes hoistory table
            _userVoteRepository.AddUserVote(voteHistory);

            return RedirectToAction("List");
        }

        // This method will get the poll details to show the chart
        [HttpGet]
        public IActionResult Results(int pollId)
        {
            var pollDetails = _pollRepository.GetPolls()
                                .SingleOrDefault(x => x.PollId == pollId)!;

            return View(pollDetails);
        }

    }
}
