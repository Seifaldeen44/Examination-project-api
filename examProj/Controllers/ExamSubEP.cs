using examProj.Data;
using examProj.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace examProj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamSubEP : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public ExamSubEP(ProjectDbContext context)
        {
            _context = context;
        }

        // POST: api/ExamSubEP/submit
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitExamAnswers([FromBody] SubmitExamDto dto)
        {
            if (dto == null || dto.Answers == null || dto.Answers.Count == 0)
                return BadRequest("Answers are required.");

            var answersJson = JsonSerializer.Serialize(dto.Answers);

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.SP_SubmitExamAnswers @Ex_ID, @St_ID, @AnswersJSON",
                    new SqlParameter("@Ex_ID", dto.Ex_ID),
                    new SqlParameter("@St_ID", dto.St_ID),
                    new SqlParameter("@AnswersJSON", answersJson)
                );

                return Ok(new
                {
                    message = "Exam answers submitted successfully."
                });
            }
            catch (SqlException ex)
            {
                // Errors coming from RAISERROR in SQL
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }
    }
}
