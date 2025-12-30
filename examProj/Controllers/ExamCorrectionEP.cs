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
    }
}
