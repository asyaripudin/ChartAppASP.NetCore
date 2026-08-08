using Chart.Models;
using System.Data;
using System.Xml.Serialization;

namespace Chart.Interface
{
    public interface ITemperature
    {
        List<Temperature> Gets(string tanggal);
        List<object> GetTemperature(string tanggal);
        object ImportExcel(IFormFile formFile);
        string Login(string username, string password, HttpContext httpContext);
        string Logout(HttpContext httpContext);
       
        

    }
}
