using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;
using MvcWhatsUp.Services;

namespace MvcWhatsUp.ViewComponents
{
    public class TopMessagesViewComponent : ViewComponent
    {
        private readonly IChatsService _chatsService;

        public TopMessagesViewComponent(IChatsService chatsService)
        {
            this._chatsService = chatsService;
        }

        public IViewComponentResult Invoke(int numberOfMessages)
        {
            List<Message>? messages = null;

            User? loggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");
            if (loggedInUser != null)
            {
                messages = _chatsService.GetLatestMessages(loggedInUser.UserId, numberOfMessages);
            }
            return View(messages);
        }
    }
}
