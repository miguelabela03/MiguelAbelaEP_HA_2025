using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Domain.Models;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        private PollRepository _pollRepository;

        // Constructor Injection
        public PollController(PollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        // This method will get all the polls from the database via the reposiotry and pass them to the view
        [HttpGet]
        public IActionResult List()
        {
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
    }
}
