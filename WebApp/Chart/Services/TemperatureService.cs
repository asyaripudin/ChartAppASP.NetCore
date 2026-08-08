using Chart.Common;
using Chart.Interface;
using Chart.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;
using System.Data.OleDb;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Session;

namespace Chart.Services
{
    public class TemperatureService : ITemperature
    {
       List<Temperature> _oTemperatures = new List<Temperature>();
       List<object>data=new List<object>();
       Users _oUsers=new Users();
      
       const string SessionName = "";
       const string SessionPassword = "";
       const string SessionUserType = "";
       private IConfiguration configuration;
       private IWebHostEnvironment webHostEnvironment;
       private HttpContext httpContext;
        public TemperatureService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        public List<Temperature> Gets(string tanggal)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                   
                    var oTemperatures = con.Query<Temperature>("SP_GetTemperatur",
                                     this.SetParameters(tanggal, "", "", (int)Common.OperationType.View),
                                     commandType: CommandType.StoredProcedure).ToList();
                    if (oTemperatures != null && oTemperatures.Count() > 0)
                    {
                        _oTemperatures = oTemperatures;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return _oTemperatures;           
        }
        public List<object> GetTemperature(string tanggal)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var oTemperatures = con.Query<Temperature>("SP_GetTemperatur",
                                     this.SetParameters(tanggal, "", "", (int)Common.OperationType.View),
                                     commandType: CommandType.StoredProcedure).ToList();
                    if (oTemperatures != null && oTemperatures.Count() > 0)
                    {
                        _oTemperatures = oTemperatures;
                        List<string> labels = _oTemperatures.Select(t => t.Time).ToList();
                        List<string> radiation= _oTemperatures.Select(r => r.Radiasi).ToList();
                        List<string> temperature = _oTemperatures.Select(s => s.Temperatur).ToList();
                        data.Add(labels);
                        data.Add(radiation);
                        data.Add(temperature);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return data;
        }

        public object ImportExcel(IFormFile file)
        {
            object result = null;
            
            int j = 0;
            DataTable dt = new DataTable();
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Upload");
            string filePath = "";
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    if (file != null)
                    {

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //Save the uploaded Excel file.
                        string fileName = Path.GetFileName(file.FileName);
                        filePath = Path.Combine(path, fileName);
                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        string conString = configuration.GetConnectionString("ExcelConString");

                        conString = string.Format(conString, filePath);
                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }
                    }
                                                            
                    foreach (DataRow row in dt.Rows)
                    {                                                 
                        string i = Convert.ToString(row["i"]);                                
                        DateTime tanggal = Convert.ToDateTime(row["Date"]);
                        string Date = tanggal.ToString("yyyy-MM-dd");
                        DateTime Jam = Convert.ToDateTime(row["Time"]);
                        string Time = Jam.ToString("hh:mm");
                        double Radiasi;
                        object value = row["Radiation"];
                        if (value == DBNull.Value)
                        {
                            Radiasi = 0; 
                        }
                        else
                        {
                            Radiasi = Convert.ToDouble(row[3]);
                        }
                                
                        double Temperatur = Convert.ToDouble(row["Temperature"]);
                        string Code = Convert.ToString(row["Code"]);

                        SqlCommand cmd = new SqlCommand("SP_ImportTemperature", (SqlConnection)con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@I", i);
                        cmd.Parameters.AddWithValue("@Date", Date);
                        cmd.Parameters.AddWithValue("@Time", Time);
                        cmd.Parameters.AddWithValue("@Radiasi", Radiasi);
                        cmd.Parameters.AddWithValue("@Temperatur", Temperatur);
                        cmd.Parameters.AddWithValue("@Code", Code);

                        cmd.Connection = (SqlConnection)con;
                        try
                        {
                            //string ImportFile = Path.Combine(webHostEnvironment.WebRootPath, "Upload");
                            j = cmd.ExecuteNonQuery();

                            if (j > 0)
                            {

                                //result = "Import Data Success";
                                System.IO.File.Delete(path);
                            }
                            else
                            {
                                //result = "Import Data Failed";
                                System.IO.File.Delete(path);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ToString();
                        }
                       

                    }
                   
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            
            return result;
        }

        public string Login(string username, string password, HttpContext httpContext)
        {
            string message = "";
            if (username == null && password == null)
            {
                message = "Username and Password is required...";
            }
            else if (username == "" || username == null)
            {
                message = "Username is required...";
            }
            else if (password == "" || password == null)
            {
                message = "Password is required...";
            }
            else
            {
                try
                {
                    bool isLogin = false;
                    isLogin = GetLogin(username, password);

                    if (isLogin == false)
                    {

                        message = "Invalid Username or Password...";
                    }
                    else
                    {
                        using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                        {
                            if (con.State == ConnectionState.Closed) con.Open();
                            var objUsers = con.Query<Users>("SELECT * FROM Users WHERE UserName='" + username + "'" + "and Password='" + password + "'" + "and IsActive='" + 1 + "'").ToList();
                            if(objUsers.Count() == 0)
                            {

                                message = "You are Suspend";
                            }
                            else if (objUsers.Count() > 0)
                            {
                                _oUsers = objUsers.SingleOrDefault();
                                httpContext.Session.SetString(SessionName, _oUsers.UserName);
                                httpContext.Session.SetString(SessionPassword, _oUsers.Password);
                                httpContext.Session.SetString(SessionUserType, _oUsers.UserType);
                                string User_Type = httpContext.Session.GetString(SessionUserType);
                                string User_Name = httpContext.Session.GetString(SessionName);
                                if (User_Name != null || User_Name != "")
                                {
                                    if (User_Type == "Admin")
                                    {
                                        message = "Admin";
                                    }
                                    else if (User_Type == "Staff")
                                    {
                                        message = "Staff";
                                    }

                                   
                                  
                                }
                                

                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            return message;

        }
        public bool GetLogin(string username, string password)
        {
            var isLogin = false;
            try
            {
                int operationType = (int)Common.OperationType.GetLogin;
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var oUsers = con.Query<Users>("SP_GetLogin",

                        this.SetParameters("",username, password, operationType),
                        commandType: CommandType.StoredProcedure);
                    if (oUsers != null && oUsers.Count() > 0)
                    {
                        _oUsers = oUsers.SingleOrDefault();

                        if (_oUsers.UserName == username && _oUsers.Password == password)
                        {
                            isLogin = true;
                        }
                        else
                        {
                            isLogin = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return isLogin;
        }

        public string Logout(HttpContext httpContext)
        {
            string message = "";
            try
            {
                httpContext.Session.Clear();
                string User_Name = httpContext.Session.GetString(SessionName);
                if (User_Name == null || User_Name == "")
                {
                    message = "success";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return message;
        }              
        private DynamicParameters SetParameters(string tanggal, string username, string password, int operationType)
        {
            DynamicParameters parameters = new DynamicParameters();
            if (operationType == 4)
            {
                parameters.Add("@Tanggal", tanggal);
                parameters.Add("@OperationType", operationType);
            }
            else if (operationType == 6)
            {
                parameters.Add("@UserName", username);
                parameters.Add("@Password", password);
                parameters.Add("@OperationType", operationType);
            }
            else if (operationType == 7)
            {
                parameters.Add("@OperationType", operationType);
            }
            return parameters;
        }

        
    }
}
