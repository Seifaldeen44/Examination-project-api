using examProj.Data;
using examProj.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace examProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public ExamController(ProjectDbContext context)
        {
            _context = context;
        }

        // POST: api/Exam/generate
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateExam(
            string courseName,
            int mcqNum,
            int tfNum)
        {
            var result = await _context.ExamQuestions
                .FromSqlRaw(
                    "EXEC dbo.SP_ExamGenerate @CourseName, @MCQ_num, @TF_num",
                    new SqlParameter("@CourseName", courseName),
                    new SqlParameter("@MCQ_num", mcqNum),
                    new SqlParameter("@TF_num", tfNum)
                )
                .ToListAsync();

            return Ok(result);
        }
    }
}
