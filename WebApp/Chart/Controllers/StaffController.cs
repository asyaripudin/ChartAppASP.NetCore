using Chart.Interface;
using Chart.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chart.Controllers
{
    public class StaffController : Controller
    {
        private readonly ITemperature _temperature;
        Users _oUsers = new Users();
        const string SessionName = "";

        public StaffController(ITemperature temperature)
        {
            _temperature = temperature;
        }
        public IActionResult Index()
        {
            string User_Name = HttpContext.Session.GetString(SessionName);//new Code
            if (User_Name == null || User_Name == "" || User_Name != "Staff")
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
            if (User_Name == null || User_Name == "" || User_Name != "Staff")
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
        public IActionResult StaffPage()
        {
            string User_Name = HttpContext.Session.GetString(SessionName);//new Code
            if (User_Name == null || User_Name == "" || User_Name != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public string Login(string username, string password)//di front end
        {
            return _temperature.Login(username, password, HttpContext);
        }

        [HttpPost]
        public string Logout()
        {
            return _temperature.Logout(HttpContext);
        }
    }
}
