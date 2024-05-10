using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Data;
using hospitalproject.Models;
using HospitalProject.Models;

namespace hospitalproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public HospitalController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            SELECT HospitalId, HospitalName, Address
                            FROM dbo.Hospitals
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalProject");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Hospitals hospitals)
        {
            string query = @"
                   INSERT INTO dbo.Hospitals (HospitalName, Address)
                   VALUES (@HospitalName, @Address)
                   ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalProject");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@HospitalName", hospitals.HospitalName);
                    myCommand.Parameters.AddWithValue("@Address", hospitals.Address);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Hospitals hospitals)
        {
            string query = @"
           UPDATE dbo.Hospitals
           SET HospitalName = @HospitalName,
               Address = @Address
           WHERE HospitalId = @HospitalId
           ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalProject");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@HospitalId", hospitals.HospitalId);
                    myCommand.Parameters.AddWithValue("@HospitalName", hospitals.HospitalName);
                    myCommand.Parameters.AddWithValue("@Address", hospitals.Address);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                           DELETE FROM dbo.Hospitals
                           WHERE HospitalId = @HospitalId
                           ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalProject");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@HospitalId", id);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

    }
}
