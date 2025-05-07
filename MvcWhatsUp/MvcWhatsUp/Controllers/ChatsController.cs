using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;
using MvcWhatsUp.Services;
using MvcWhatsUp.ViewModels;

namespace MvcWhatsUp.Controllers
{
    public class ChatsController : Controller
    {
        //fields and properties
        private readonly IChatsService _chatsService;

        private readonly IUsersService _usersService;

        //constructors
        public ChatsController(IChatsService chatsService, IUsersService usersService)
        {
            _chatsService = chatsService;
            _usersService = usersService;
        }

        //methods
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddMessage(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Users");
            }

            User? loggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");

            if (loggedInUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            User? receivingUser = _usersService.GetById((int)id);
            ViewData["receivingUser"] = receivingUser;

            Message message = new Message();
            message.SenderUserId = loggedInUser.UserId;
            message.ReceiverUserId = (int)id;

            return View(message);
        }

        [HttpPost]
        public IActionResult AddMessage(Message message)
        {
            try
            {
                message.SendAt = DateTime.Now;
                _chatsService.AddMessage(message);


                return RedirectToAction("DisplayChat", new { id = message.ReceiverUserId });
            }
            catch (Exception ex)
            {
                return View(message);
            }
        }

        [HttpGet]
        public IActionResult DisplayChat (int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Users");
            }

            User? loggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            User? receivingUser = _usersService.GetById((int)id);
            if (receivingUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            List<Message> chatMessages = _chatsService.GetMessages(loggedInUser.UserId, receivingUser.UserId);

            ChatViewModel chatViewModel = new ChatViewModel(chatMessages, loggedInUser, receivingUser);

            return View(chatViewModel);
        }
    }
}
