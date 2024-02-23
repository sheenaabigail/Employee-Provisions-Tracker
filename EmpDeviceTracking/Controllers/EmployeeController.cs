using EmpDeviceTracking.ActionFilter;
using EmpDeviceTracking.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace EmpDeviceTracking.Controllers
{
    [MyActionFilterAttribute]
    public class EmployeeController : Controller
    {
        public List<mSite> getSite()
        {
            List<mSite> site = new List<mSite>();

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
                sql = "EXEC udp_getSite";
                cmd = new SqlCommand(sql, conn);
                dreader = cmd.ExecuteReader();


                while (dreader.Read())
                {
                    mSite sites = new()
                    {
                        SiteID = dreader.GetInt32(0),
                        SiteName = dreader.GetString(1),

                    };

                    site.Add(sites);


                }

                dreader.Close();
                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {
              
            }

            return site;
        }

        public List<mDept> getDept()
        {
            List<mDept> dept = new List<mDept>();

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
                sql = "EXEC udp_getDept";
                cmd = new SqlCommand(sql, conn);
                dreader = cmd.ExecuteReader();


                while (dreader.Read())
                {
                    mDept depts = new()
                    {
                        DeptID = dreader.GetInt32(0),
                        DeptName = dreader.GetString(1),

                    };

                    dept.Add(depts);


                }

                dreader.Close();
                cmd.Dispose();
                conn.Close();


            }
            catch (Exception ex)
            {

            }

            return dept;
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
                sql = "EXEC udp_getEmployeeProvisionInfo ";
                cmd = new SqlCommand(sql, conn);
                dreader = cmd.ExecuteReader();

                List<Employee> employees = [];

                while (dreader.Read())
                {
                    Employee emp = new()
                    {
                        HostName = dreader.GetString(10),
                        EmpID = dreader.GetString(0),
                        EmpName = dreader.GetString(1),
                        SiteName = dreader.GetString(2),
                        DeptName = dreader.GetString(3),
                        Lap = dreader.GetBoolean(4),
                        Charger = dreader.GetBoolean(5),
                        Mouse = dreader.GetBoolean(6),
                        LapBag = dreader.GetBoolean(7),
                        MousePad = dreader.GetBoolean(8),
                        Remarks = dreader.GetString(9)
                    };

                    employees.Add(emp);

                }

                dreader.Close();
                cmd.Dispose();
                conn.Close();

                @ViewBag.employees = employees;
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving employee data.";
                return View("Error"); 
            }
            
        }

        public IActionResult NewEntry()
        {
            List<mSite> site = new List<mSite>();
            List<mDept> dept = new List<mDept>();
            try
            {

                site = getSite();
                dept = getDept();
                ViewBag.Site = site;
                ViewBag.Dept = dept;

                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving employee data.";
                return View("Error");
            }

          

            //site.Add(new mSite
            //{
            //    SiteID = 1,
            //    SiteName = "DTA"
            //}) ;

            //site.Add(new mSite
            //{
            //    SiteID = 2,
            //    SiteName = "SEZ"
            //});

            //site.Add(new mSite
            //{
            //    SiteID = 3,
            //    SiteName = "DTA-2"
            //});

            
        }

    [HttpPost]
    public RedirectToActionResult SubmitNewEntry(string hostname, string empid, string empname, string sitename, string deptname,
    string laptop, string charger, string mouse, string laptopbag, string mousepad, string optionalremark)
        {
            DeleteEntry(empid);
            string constr;
            SqlConnection conn;

            constr = @"Server=(localdb)\mssqllocaldb;Database=EmployeeDetails;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            conn = new SqlConnection(constr);

            conn.Open();

            //SqlCommand cmd;
           
            //SqlDataReader dreader;

            string sql;
            sql = "INSERT INTO EmpProvisionInfo (EmpID, EmpName, Site, Dept, Lap, Charger, Mouse, LapBag, MousePad, Remarks, HostName) VALUES (@empid, @empname, @sitename, @deptname, @laptop, @charger, @mouse, @laptopbag, @mousepad , @remark, @hostname)";
            //cmd = new SqlCommand(sql, conn);
            //cmd.CommandType = CommandType.Text;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empid", empid);
                cmd.Parameters.AddWithValue("@empname", empname);
                cmd.Parameters.AddWithValue("@sitename", sitename);
                cmd.Parameters.AddWithValue("@deptname", deptname);
                cmd.Parameters.AddWithValue("@laptop", laptop=="on"?true:false);
                cmd.Parameters.AddWithValue("@charger", charger == "on" ? true : false);
                cmd.Parameters.AddWithValue("@mouse", mouse == "on" ? true : false);
                cmd.Parameters.AddWithValue("@laptopbag", laptopbag == "on" ? true : false);
                cmd.Parameters.AddWithValue("@mousepad", mousepad == "on" ? true : false);
                cmd.Parameters.AddWithValue("@remark", optionalremark);
                cmd.Parameters.AddWithValue("@hostname",hostname);

                cmd.ExecuteNonQuery();
            }
            
            conn.Close();
            return RedirectToAction("NewEntry");            
        }

        public RedirectToActionResult UpdateEntry(string hostname, string empid, string empname, string site, string dept,
    string laptop, string charger, string mouse, string laptopbag, string mousepad, string optionalremark)
        {
            
            string constr;
            SqlConnection conn;

            constr = @"Server=(localdb)\mssqllocaldb;Database=EmployeeDetails;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            conn = new SqlConnection(constr);

            conn.Open();

            //SqlCommand cmd;

            //SqlDataReader dreader;

            string sql;
            sql = "EXEC udp_updateEntry @empid, @empname, @site, @dept, @laptop, @charger, @mouse, @laptopbag, @mousepad, @remark, @hostname";
            //cmd = new SqlCommand(sql, conn);
            //cmd.CommandType = CommandType.Text;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empid", empid);
                cmd.Parameters.AddWithValue("@empname", empname);
                cmd.Parameters.AddWithValue("@site", int.Parse(site));
                cmd.Parameters.AddWithValue("@dept", int.Parse(dept));
                cmd.Parameters.AddWithValue("@laptop", laptop == "on" ? true : false);
                cmd.Parameters.AddWithValue("@charger", charger == "on" ? true : false);
                cmd.Parameters.AddWithValue("@mouse", mouse == "on" ? true : false);
                cmd.Parameters.AddWithValue("@laptopbag", laptopbag == "on" ? true : false);
                cmd.Parameters.AddWithValue("@mousepad", mousepad == "on" ? true : false);
                cmd.Parameters.AddWithValue("@remark", optionalremark);
                cmd.Parameters.AddWithValue("@hostname", hostname);

                cmd.ExecuteNonQuery();
            }

            ssconn.Close();
            return RedirectToAction("Index");
        }

        public RedirectToActionResult DeleteEntry(string empid)
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
            sql = "Delete From EmpProvisionInfo Where EmpID=@empid";
            //cmd = new SqlCommand(sql, conn);
            //cmd.CommandType = CommandType.Text;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empid", empid);
                cmd.ExecuteNonQuery();
            }
            
            conn.Close();
            return RedirectToAction("Index");
        }

        public ActionResult EditEntry(string empid)
        {
            
            //SqlDataAdapter adapter = new SqlDataAdapter();

            string constr;
            SqlConnection conn;

            constr = @"Server=(localdb)\mssqllocaldb;Database=EmployeeDetails;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            conn = new SqlConnection(constr);

            conn.Open();

            SqlCommand cmd;

            SqlDataReader dreader;

            string sql;
            sql = "select * from EmpProvisionInfo where EmpID=@empid";
            cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@empid", empid);

            Employee emp = new Employee();

            dreader = cmd.ExecuteReader();
           while(dreader.Read())
            {
                    emp.HostName = dreader.GetString(10);
                    emp.EmpID = dreader.GetString(0);
                    emp.EmpName = dreader.GetString(1);
                    emp.Site = dreader.GetInt32(2);
                    emp.Dept = dreader.GetInt32(3);
                    emp.Lap = dreader.GetBoolean(4);
                    emp.Charger = dreader.GetBoolean(5);
                    emp.Mouse = dreader.GetBoolean(6);
                    emp.LapBag = dreader.GetBoolean(7);
                    emp.MousePad = dreader.GetBoolean(8);
                    emp.Remarks = dreader.GetString(9);
                
              
            }
            //cmd.ExecuteNonQuery();
            //DeleteEntry(empid);

            dreader.Close();
            cmd.Dispose();
            conn.Close();

            List<mDept> dept = new List<mDept>();

            dept = getDept();
            ViewBag.Dept = dept;


            

            List<mSite> site = new List<mSite>();

            site = getSite();
            ViewBag.Site = site;


            ViewBag.emp = emp;

            return View();
        }

        
    }    
}

