using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;
using MvcWhatsUp.Repositories;
using MvcWhatsUp.ViewModels;

namespace MvcWhatsUp.Controllers
{
    public class ChatsController : Controller
    {
        //fields and properties
        private readonly IChatsRepository _chatsRepository;

        private readonly IUsersRepository _usersRepository;

        //constructors
        public ChatsController(IChatsRepository chatsRepository, IUsersRepository usersRepository)
        {
            _chatsRepository = chatsRepository;
            _usersRepository = usersRepository;
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

            User? receivingUser = _usersRepository.GetById((int)id);
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
                _chatsRepository.AddMessage(message);


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

            User? receivingUser = _usersRepository.GetById((int)id);
            if (receivingUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            List<Message> chatMessages = _chatsRepository.GetMessages(loggedInUser.UserId, receivingUser.UserId);

            ChatViewModel chatViewModel = new ChatViewModel(chatMessages, loggedInUser, receivingUser);

            return View(chatViewModel);
        }
    }
}
