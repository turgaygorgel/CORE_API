using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public StudentController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]//http://localhost:49146/api/student
        public JsonResult Get()
        {
            string query = @"
                            select StudentId, StudentName,Department,
                            convert(varchar(10),Registerdate,120) as Registerdate,PhotoFileName
                            from
                            dbo.Student
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MSSQLConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Student std)
        {
            string query = @"
                           insert into dbo.Student
                           (StudentName,Department,Registerdate,PhotoFileName)
                    values (@StudentName,@Department,@Registerdate,@PhotoFileName)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MSSQLConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@StudentName", std.StudentName);
                    myCommand.Parameters.AddWithValue("@Department", std.Department);
                    myCommand.Parameters.AddWithValue("@Registerdate", std.Registerdate);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", std.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully, Subscribe and Like the channel plase");
        }


        [HttpPut]//http://localhost:49146/api/student
        public JsonResult Put(Student std)
        {
            string query = @"
                           update dbo.Student
                           set StudentName= @StudentName,
                            Department=@Department,
                            Registerdate=@Registerdate,
                            PhotoFileName=@PhotoFileName
                            where StudentId=@StudentId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MSSQLConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@StudentId", std.StudentId);
                    myCommand.Parameters.AddWithValue("@StudentName", std.StudentName);
                    myCommand.Parameters.AddWithValue("@Department", std.Department);
                    myCommand.Parameters.AddWithValue("@Registerdate", std.Registerdate);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", std.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully, Subscribe and Like the channel plase");
        }

        [HttpDelete("{id}")]//http://localhost:49146/api/student
        public JsonResult Delete(int id)
        {
            string query = @"
                           delete from dbo.Student
                            where StudentId=@StudentId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MSSQLConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@StudentId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully, Subscribe and Like the channel plase");
        }


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("studentphoto.png");
            }
        }

    }
}
