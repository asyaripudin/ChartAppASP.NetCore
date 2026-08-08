using Chart.Interface;
using Chart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using Chart.Common;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace Chart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITemperature _temperature;
        private readonly IUsers _users;

        Users _oUsers = new Users();
        const string SessionName = "";
        const string SessionPassword = "";
        const string SessionUserType = "";

        public HomeController(ILogger<HomeController> logger, ITemperature temperature,
            IUsers users
        )
        {
            _logger = logger;
            _temperature = temperature;
            _users= users;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public string Register([FromBody] Users oUsers)
        {
            return _users.Save(oUsers);
        }
        [HttpPut]
        public Users ChangePassword([FromBody] Users oUsers)
        {
            return _users.ChangePassword(oUsers);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}