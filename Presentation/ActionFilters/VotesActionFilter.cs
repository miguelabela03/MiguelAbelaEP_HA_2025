using DataAccess.DataContext;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Presentation.ActionFilters
{
    public class VotesActionFilter : ActionFilterAttribute
    {
        private PollDbContext _dbContext;

        // Constructor Injection
        public VotesActionFilter(PollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Getting the current logged-in user
            var currentUser = context.HttpContext.User;

            string userId = "";
            var pollId = context.ActionArguments["pollId"] as int?;

            // Proceed only if user and pollId are valid
            if (currentUser != null && currentUser.Identity.IsAuthenticated && pollId.HasValue)
            {
                // Fetch the userId from the claim
                var userIdClaim = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    userId = userIdClaim.Value;

                    // Convert userId to Guid
                    if (Guid.TryParse(userId, out Guid userGuid))
                    {
                        // Use UserVoteRepository to check if the user has already voted
                        var userVoteRepository = new UserVoteRepository(_dbContext); // Assuming repository is initialized
                        var existingVote = userVoteRepository.GetUserVote(pollId.Value, userGuid);

                        if (existingVote != null)
                        {
                            // User has already voted, redirecting them to the poll list
                            if (context.Controller is Controller controller)
                            {
                                controller.TempData["error"] = "You have already voted on this poll.";
                            }

                            context.Result = new RedirectToActionResult("List", "Poll", null);
                            return;
                        }
                    }
                    else
                    {
                        // Invalid userId format
                        // context.Result = new BadRequestObjectResult("Invalid userId format.");
                        if (context.Controller is Controller controller)
                        {
                            controller.TempData["error"] = "Error: Invalid user ID format.";
                        }

                        context.Result = new RedirectToActionResult("List", "Poll", null);
                        return;
                    }
                }
                else
                {
                    // Missing userId claim
                    // context.Result = new BadRequestObjectResult("UserId claim is missing.");
                    if (context.Controller is Controller controller)
                    {
                        controller.TempData["error"] = "Error: User ID claim is missing.";
                    }

                    context.Result = new RedirectToActionResult("List", "Poll", null);
                    return;
                }
            }
            else
            {
                // Missing or invalid pollId or unauthenticated user
                // context.Result = new BadRequestObjectResult("Poll ID is required and user must be authenticated.");
                if (context.Controller is Controller controller)
                {
                    controller.TempData["error"] = "Error: Poll ID is required and user must be authenticated.";
                }

                context.Result = new RedirectToActionResult("List", "Poll", null);
                return;
            }

            base.OnActionExecuting(context);
        }

    }
}
