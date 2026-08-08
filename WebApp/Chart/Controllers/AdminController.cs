using Chart.Interface;
using Chart.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chart.Controllers
{
    public class AdminController : Controller
    {
        private readonly ITemperature _temperature;
        private readonly IUsers _users;
        Users _oUsers = new Users();
        const string SessionName = "";

        public AdminController(ITemperature temperature, IUsers users)
        {
            _temperature = temperature;
            _users = users;
        }
        public IActionResult Index()
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
        public IActionResult Temperature()
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
        [HttpGet]
        public List<Temperature> GetDataTemperature(string tanggal)
        {
            return _temperature.Gets(tanggal);
        }

        [HttpGet]
        public List<object> GetGraphicTemperature(string tanggal)
        {
            return _temperature.GetTemperature(tanggal);
        }
        public IActionResult ImportFile()
        {
            return View();
        }

        [HttpPost]
        public object ImportFile(IFormFile importFile)
        {
            return _temperature.ImportExcel(importFile);
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

        public IActionResult UserList()
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
    }
}
