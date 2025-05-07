using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Services;
using MvcWhatsUp.Models;
using System.Text.Json;

namespace MvcWhatsUp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
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

            List<User> users = _usersService.GetAll();

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
                _usersService.Add(user);

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

            User? user = _usersService.GetById((int)id);
            return View(user);
        }

        //When you have filled in the form on the users/edit page
        [HttpPost]
        public ActionResult Edit(User user)
        {
            try
            {
                _usersService.Update(user);

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

            User? user = _usersService.GetById((int)id);
            return View(user);
        }

        //When you have filled in the form on the users/delete page
        [HttpPost]
        public ActionResult Delete(User user)
        {
            try
            {
                _usersService.Delete(user);

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
            User? user = _usersService.GetByLoginCredentials(loginModel.Username, loginModel.Password);

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

        public IActionResult SetPreferredTheme(string? theme)
        {
            if (theme != null)
            {
                CookieOptions options = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(7),
                    Path = "/",
                    Secure = true,
                    HttpOnly = true,
                    IsEssential = true
                };
                Response.Cookies.Append("PreferredTheme", theme, options);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ClearPreferredTheme()
        {
            Response.Cookies.Delete("PreferredTheme");

            return RedirectToAction("Index", "Home");
        }

    }
}
