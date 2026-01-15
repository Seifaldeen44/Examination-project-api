using examProj.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace examProj.Controllers
{
    [ApiController]
    [Route("api/instructor")]
    public class InstructorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InstructorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPut("update-exam-config")]
        public IActionResult UpdateExamConfig(UpdateExamConfigDto dto)
        {
            using SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            string query = @"
        IF NOT EXISTS (
            SELECT 1 
            FROM HR.Ins_Course
            WHERE Ins_ID = @Ins_ID AND Crs_ID = @Crs_ID
        )
        BEGIN
            RAISERROR('Instructor is not assigned to this course',16,1);
            RETURN;
        END

        UPDATE HR.Course
        SET 
            mcq_num = @MCQ_Num,
            TF_num  = @TF_Num
        WHERE Crs_ID = @Crs_ID;
        ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Ins_ID", dto.Ins_ID);
            cmd.Parameters.AddWithValue("@Crs_ID", dto.Crs_ID);
            cmd.Parameters.AddWithValue("@MCQ_Num", dto.MCQ_Num);
            cmd.Parameters.AddWithValue("@TF_Num", dto.TF_Num);

            con.Open();
            cmd.ExecuteNonQuery();

            return Ok("Exam configuration updated successfully");
        }
    }

}
