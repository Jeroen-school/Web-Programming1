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
        public ActionResult Delete(int id)
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

    }
}
