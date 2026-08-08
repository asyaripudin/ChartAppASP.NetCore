using Chart.Interface;
using Chart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Chart.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsers _users;
        const string SessionName = "";

        public UsersController(IUsers users)
        {
            _users = users;
        }

        public IActionResult UserList()
        {
            return View();
        }

        [HttpGet]
        public List<Users> GetUser()
        {
            return _users.GetUsers();
        }

        [HttpGet]
        public int GetUserID()
        {
            return _users.GetUserID();
        }
        [HttpPost]
        public Users Add([FromBody] Users user)
        {
            return _users.Add(user);
        }
        [HttpGet]
        public Users Get(int id)
        {
            return _users.Get(id);
        }
        [HttpPut]
        public Users Update([FromBody] Users user)
        {
            return _users.Update(user);
        }
        [HttpPut]
        public string Delete(int id)
        {
            return _users.Delete(id);
        }
        [HttpGet]
        public IActionResult AdminPage()
        {
            string User_Name = HttpContext.Session.GetString(SessionName);//new Code
            if (User_Name == null || User_Name == "" || User_Name != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

    }
}
