using hospitalproject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;

namespace hospitalproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select DoctorId, DoctorName, DoctorTc from
                            dbo.Doctors
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalProject");
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
        public JsonResult Post(Doctors doctor)
        {
            string query = @"
                   INSERT INTO dbo.Doctors (DoctorName, DoctorTc)
                   VALUES (@DoctorName, @DoctorTc);
                   SELECT SCOPE_IDENTITY();
                   ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalProject");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DoctorName", doctor.DoctorName);
                    myCommand.Parameters.AddWithValue("@DoctorTc", doctor.DoctorTc);

                    doctor.DoctorId = Convert.ToInt32(myCommand.ExecuteScalar());
                }
            }

            return new JsonResult(doctor);
        }


        [HttpPut]
        public JsonResult Put(Doctors doc)
        {
            string query = @"
                           update dbo.Doctors
                           set DoctorName = @DoctorName,
                           DoctorTc = @DoctorTc
                           where DoctorId = @DoctorId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalProject");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DoctorId", doc.DoctorId);
                    myCommand.Parameters.AddWithValue("@DoctorName", doc.DoctorName);
                    myCommand.Parameters.AddWithValue("@DoctorTc", doc.DoctorTc);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                           delete from dbo.Doctors
                            where DoctorId = @DoctorId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HospitalProject");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DoctorId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }


    }
}
