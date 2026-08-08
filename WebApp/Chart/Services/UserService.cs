using Chart.Common;
using Chart.Interface;
using Chart.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Diagnostics.Metrics;

namespace Chart.Services
{
    public class UserService : IUsers
    {
        List<Users> _oUserList = new List<Users>();
        Users _oUsers = new Users();
        Users _oUser = new Users();
        string Status = "";
        public List<Users> GetUsers()
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var oUserList = con.Query<Users>("SP_GetUsers",
                                     this.SetParameters(_oUsers, (int)Common.OperationType.View),
                                     commandType: CommandType.StoredProcedure).ToList();
                    if (oUserList != null && oUserList.Count() > 0)
                    {
                        _oUserList = oUserList;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return _oUserList;
        }
        public string Save(Users oUsers)
        {
            string message = "";
            try
            {
                bool IsExist=false;
                IsExist = CheckUser(oUsers.UserName, oUsers.Password);
                if (IsExist == true)
                {
                    message = "UserName Allready Exist...";                   
                }
                else
                {
                    int operationType = (int)Common.OperationType.Insert;
                    using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                            con.Query<Users>("SP_InsertUser",
                            this.SetParameters(oUsers,operationType),
                            commandType: CommandType.StoredProcedure);
                        message = "Register Successfully...";
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return message;
        }
        public bool CheckUser(string username, string password)
        {
            var isExist = false;
            try
            {              
       
                //int operationType = (int)Common.OperationType.GetLogin;
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var oUsers = con.Query<Users>("select * from Users where UserName='" + username + "'" + "and Password='" + password + "'");

                        //this.SetParameters(username, password, operationType),
                        //commandType: CommandType.StoredProcedure);
                    if (oUsers != null && oUsers.Count() > 0)
                    {
                        _oUsers = oUsers.SingleOrDefault();

                        if (_oUsers.UserName == username && _oUsers.Password == password)
                        {
                            isExist = true;
                        }
                        else
                        {
                            isExist = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return isExist;
        }
        public int GetUserID()
        {
            int userId = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand cmd = new SqlCommand("SP_GetUserID", (SqlConnection)con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = (SqlConnection)con;
                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                    if (userId == 0)
                    {
                        userId = 1;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return userId;
        }
        public Users Add(Users user)
        {
            try
            {
                int operationType = (int)Common.OperationType.Add;
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var oUsers = con.Query<Users>("SP_InsertUser2",

                        this.SetParameters(user, operationType),
                        commandType: CommandType.StoredProcedure);

                    if (oUsers != null && oUsers.Count() > 0)
                    {
                        _oUser = oUsers.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return _oUser;
        }
        

        public Users Get(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var oUser = con.Query<Users>("SELECT * FROM Users WHERE UserID='" + id + "'").SingleOrDefault();
                    if (oUser != null)
                    {
                        _oUser = oUser;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return _oUser;
        }

        public Users Update(Users user)
        {
            try
            {
                int operationType = (int)Common.OperationType.Update;
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var oUser = con.Query<Users>("SP_UpdateUser",

                        this.SetParameters(user, operationType),
                        commandType: CommandType.StoredProcedure);

                    if (oUser != null && oUser.Count() > 0)
                    {
                        _oUser = oUser.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return _oUser;
        }

        public string Delete(int id)
        {
            string message = "";
            try
            {
                _oUser = new Users()
                {
                   UserID = id
                };
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var oUser = con.Query<Users>("SP_DeleteUser",

                        this.SetParameters(_oUser, (int)Common.OperationType.UpdateDelete),
                        commandType: CommandType.StoredProcedure);

                    if (oUser != null && oUser.Count() > 0)
                    {
                        _oUser = oUser.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            message = "Delete Data Success...";
            return message;
        }
        public Users ChangePassword(Users oUsers)
        {
            try
            {
                int operationType = (int)Common.OperationType.ChangePassword;
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                      con.Query<Users>("SP_ChangePassword",
                        this.SetParameters(oUsers, operationType),
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return _oUser;
        }
        private DynamicParameters SetParameters(Users oUsers, int operationType)
        {
            DynamicParameters parameters = new DynamicParameters();


            if (operationType == 1)
            {
                parameters.Add("@UserName", oUsers.UserName);
                parameters.Add("@Password", oUsers.Password);
                parameters.Add("@OperationType", operationType);
            }
            else if (operationType == 2)
            {
                if (oUsers.UserType == "Admin")
                {
                    oUsers.UserType = "Admin";
                }
                else
                {
                    oUsers.UserType = "Staff";
                }

                if (oUsers.IsActive == "True" || oUsers.IsActive == "Active")
                {
                    oUsers.IsActive = "1";
                }
                else
                {
                    oUsers.IsActive = "0";
                }
                parameters.Add("@UserID", oUsers.UserID);
                parameters.Add("@UserName", oUsers.UserName);
                parameters.Add("@Password", oUsers.Password);
                parameters.Add("@UserType", oUsers.UserType);
                parameters.Add("@IsActive", oUsers.IsActive);
                parameters.Add("@OperationType", operationType);
            }
            else if (operationType == 4)
            {
                parameters.Add("@OperationType", operationType);
            }
            else if (operationType == 5)
            {
                parameters.Add("@UserID", oUsers.UserID);
                parameters.Add("@OperationType", operationType);
            }
            else if (operationType == 7)
            {
                parameters.Add("@UserName", oUsers.UserName);
                parameters.Add("@Password", oUsers.Password);
                parameters.Add("@IsActive", oUsers.IsActive);
                parameters.Add("@UserType", oUsers.UserType);
                parameters.Add("@OperationType", operationType);
            }
            else if (operationType == 8)
            {
                parameters.Add("@UserName", oUsers.UserName);
                parameters.Add("@NewPassword", oUsers.NewPassword);
                parameters.Add("@OperationType", operationType);
            }
            return parameters;
        }
    }
}
