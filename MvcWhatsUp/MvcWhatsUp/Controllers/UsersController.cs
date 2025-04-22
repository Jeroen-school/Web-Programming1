using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Repositories;
using MvcWhatsUp.Models;
using System.Text.Json;

namespace MvcWhatsUp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IActionResult Index()
        {
            /*
             //retrieve logged in user JSON through a session
             User? loggedInUser = null;
             string? userJson = HttpContext.Session.GetString("LoggedInUser");

             //If a user string is retrieved, deserialize it
             if (userJson != null)
             {
                 loggedInUser = JsonSerializer.Deserialize<User>(userJson);
             }
            */

            User? loggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");

            //pass the logged in user ID to the view
            ViewData["LoggedInUser"] = loggedInUser;

            List<User> users = _usersRepository.GetAll();

            return View(users);
        }

        //When first opening the users/create page
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //When you have filled in the form on the users/create page
        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
                _usersRepository.Add(user);

                TempData["Success"] = "User succesfully created!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error creating the user: {ex.Message} Please try again.";

                return View(user);
            }
        }

        //When opening the users/Edit page from the users page
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User? user = _usersRepository.GetById((int)id);
            return View(user);
        }

        //When you have filled in the form on the users/edit page
        [HttpPost]
        public ActionResult Edit(User user)
        {
            try
            {
                _usersRepository.Update(user);

                TempData["Success"] = "User succesfully edited!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error editing the user: {ex.Message} Please try again.";

                return View(user);
            }
        }

        //When opening the users/Edit page from the users page
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User? user = _usersRepository.GetById((int)id);
            return View(user);
        }

        //When you have filled in the form on the users/delete page
        [HttpPost]
        public ActionResult Delete(User user)
        {
            try
            {
                _usersRepository.Delete(user);

                TempData["Success"] = "User succesfully deleted!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"{ex.Message} Please try again.";
                return View(user);
            }
        }


        //Logging in
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            User? user = _usersRepository.GetByLoginCredentials(loginModel.Username, loginModel.Password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Wrong username or password!";
                return View(loginModel);
            }
            else
            {
                /*
                //serialize logged in user, so you can put it in the session
                string userJson = JsonSerializer.Serialize(user);

                //remember logged in user, using the session
                HttpContext.Session.SetString("LoggedInUser", userJson);
                */
                HttpContext.Session.SetObject("LoggedInUser", user);

                TempData["Success"] = "Succesfully logged in!";

                return RedirectToAction("Index", "Users");
            }

        } 

    }
}
