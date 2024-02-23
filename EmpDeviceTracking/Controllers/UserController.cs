using EmpDeviceTracking.ActionFilter;
using EmpDeviceTracking.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;


namespace EmpDeviceTracking.Controllers
{
    [MyActionFilterAttribute]
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            try
            {
                string constr;
                SqlConnection conn;

                constr = @"Server=(localdb)\mssqllocaldb;Database=EmployeeDetails;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
                conn = new SqlConnection(constr);

                conn.Open();

                SqlCommand cmd;
                SqlDataReader dreader;

                string sql;
                sql = "select FirstName,LastName, Email,PhoneNo,Gender from UserRegistrationInfo";
                cmd = new SqlCommand(sql, conn);
                dreader = cmd.ExecuteReader();

                List<User> users = [];

                while (dreader.Read())
                {
                    User user = new()
                    {
                        FirstName = dreader.GetString(0),
                        LastName = dreader.GetString(1),
                        Email = dreader.GetString(2),
                        PhoneNo = dreader.GetString(3),
                        Gender = dreader.GetBoolean(4)
                       
                    };

                    users.Add(user);

                    //employees.Add(new Employee
                    //{
                    //    EmpID = Convert.ToString(dreader.GetValue(0)),
                    //    EmpName = Convert.ToString(dreader.GetValue(1)),
                    //    Site = Convert.ToString(dreader.GetValue(2)),
                    //    Dept = Convert.ToString(dreader.GetValue(3)),
                    //    Lap = Convert.ToBoolean(dreader.GetValue(4)),
                    //    Charger = Convert.ToBoolean(dreader.GetValue(5)),
                    //    Mouse = Convert.ToBoolean(dreader.GetValue(6)),
                    //    LapBag = Convert.ToBoolean(dreader.GetValue(7)),
                    //    MousePad = Convert.ToBoolean(dreader.GetValue(8))
                    //});
                }

                dreader.Close();
                cmd.Dispose();
                conn.Close();

                @ViewBag.users = users;
                //ViewBag.ErrorMessage = "";
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving user data.";
                return View("Error");
            }
        }



        public IActionResult LoginUser(string username, string password)
        {
            ClaimsIdentity identity = null;
            try
            {
                string constr = @"Server=(localdb)\mssqllocaldb;Database=EmployeeDetails;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";



                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    SqlCommand cmd;
                    SqlDataReader dreader;

                    string sql = "SELECT * FROM UserRegistrationInfo WHERE Email = @username AND Password = @password";
                    cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password); // Note: Consider hashing the password and comparing the hashed values

                    dreader = cmd.ExecuteReader();

                    User user = new User();

                    if (dreader.Read())
                    {
                        user.FirstName = dreader.GetString(0);
                        user.LastName = dreader.GetString(1);
                        user.Email = dreader.GetString(2);
                        user.PhoneNo = dreader.GetString(3);
                        user.Gender = dreader.GetBoolean(4);
                        user.Password = dreader.GetString(5);

                        //ViewData["hostname"] = user.FirstName;
                        //ViewBag.hostname = user.FirstName;
                        dreader.Close();

                        cmd.Dispose();
                        conn.Close();


                        if(!String.IsNullOrEmpty(user.FirstName))
                        {
                            _contextAccessor.HttpContext.Session.SetString("UserName", user.FirstName);

                        }

                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid Username & Password";
                        return View("Login");
                    }
                

                    

                    //if (user != null && user.Email == username && user.Password == password)
                    //{
                    //    ViewBag.hostname = user.FirstName;
                    //    return RedirectToAction("Index", "Employee");
                    //}
                    //else if (user != null && user.Email == username && user.Password != password)
                    //{
                    //    return RedirectToAction("Login");
                    //}
                    //else
                    //{
                    //    return RedirectToAction("Register");
                    //}
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving user data.";
                return RedirectToAction("Error");
            }
        }


        public RedirectToActionResult RegisterUser(string FirstName, string LastName, string Email, string PhoneNo,
    bool Gender, string Password)
        {               
                string constr;
                SqlConnection conn;

                constr = @"Server=(localdb)\mssqllocaldb;Database=EmployeeDetails;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
                conn = new SqlConnection(constr);

                conn.Open();

                //SqlCommand cmd;

                //SqlDataReader dreader;

                string sql;
                sql = "INSERT INTO UserRegistrationInfo (FirstName,LastName,Email,PhoneNo,Gender,Password) VALUES (@FirstName,@LastName,@Email,@PhoneNo,@Gender,@Password)";
                //cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.Text;

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    cmd.ExecuteNonQuery();
                }
                //else
                //{
                //    ViewBag.ErrorMessage = "Invalid Username & Password";
                //    return View("Register");
                //}
            conn.Close();
                
                return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            ViewData["username"] = null;
            return View("Login","User");
        }
        
        public RedirectToActionResult DeleteUser(string username)
        {
            //SqlDataAdapter adapter = new SqlDataAdapter();

            string constr;
            SqlConnection conn;

            constr = @"Server=(localdb)\mssqllocaldb;Database=EmployeeDetails;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            conn = new SqlConnection(constr);

            conn.Open();

            //SqlCommand cmd;

            //SqlDataReader dreader;

            string sql;
            sql = "Delete From UserRegistrationInfo Where Email=@email";
            //cmd = new SqlCommand(sql, conn);
            //cmd.CommandType = CommandType.Text;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@email", username);
                cmd.ExecuteNonQuery();
            }

            conn.Close();
            return RedirectToAction("Index");
        }
    }
}
