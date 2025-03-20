using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Repositories;
using MvcWhatsUp.Models;

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
            //log in a user through a cookie
            //THIS IS NOT SAFE, YOU CAN EDIT COOKIES AND LOG IN WITHOUT PASSWORDS. TOO BAD!
            string? userId = Request.Cookies["UserId"];

            //pass the logged in user ID to the view
            ViewData["UserId"] = userId;

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

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
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
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
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

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
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
                return View(loginModel);
            }
            else
            {
                //THIS IS NOT THE RIGHT WAY TO DO IT. TOO BAD!
                Response.Cookies.Append("UserId", user.UserId.ToString());

                return RedirectToAction("Index", "Users");
            }

        } 

    }
}
