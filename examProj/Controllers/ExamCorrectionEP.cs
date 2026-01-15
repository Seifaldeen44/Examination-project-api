using examProj.Data;
using examProj.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace examProj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamCorrectionEP : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public ExamCorrectionEP(ProjectDbContext context)
        {
            _context = context;
        }

        // POST: api/ExamCorrectionEP/correct
        [HttpPost("correct")]
        public async Task<IActionResult> CorrectExam(int exId)
        {
            if (exId <= 0)
                return BadRequest("Invalid exam ID.");

            try
            {
                var result = await _context.ExamResults
                    .FromSqlRaw(
                        "EXEC dbo.SP_ExamCorrection @Ex_ID",
                        new SqlParameter("@Ex_ID", exId)
                    )
                    .ToListAsync();

                return Ok(new
                {
                    ExamId = exId,
                    TotalDegree = result.FirstOrDefault()?.TotalDegree ?? 0
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }

        // GET: api/Exam/student-grades/{studentId}
        [HttpGet("student-grades/{studentId}")]
        public async Task<IActionResult> GetStudentGrades(int studentId)
        {
            var result = await _context.StudentExamGrades
                .FromSqlRaw(@"
            SELECT 
                e.Ex_ID,
                c.Crs_Name AS CourseName,
                e.TotalDegree AS StudentDegree,
                e.total_exam_degree AS TotalExamDegree,
                e.Ex_Date AS ExamDate
            FROM Instructor.Exam e
            JOIN HR.Course c ON e.Crs_ID = c.Crs_ID
            WHERE e.St_ID = @St_ID
              AND e.TotalDegree <> -1
            ORDER BY e.Ex_Date DESC
        ",
                new SqlParameter("@St_ID", studentId))
                .ToListAsync();

            return Ok(result);
        }




    }
}
