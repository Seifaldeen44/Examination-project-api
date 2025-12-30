using examProj.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace examProj.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public StudentController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpGet("{studentId}/courses")]
        public IActionResult GetStudentCourses(int studentId)
        {
            var courses = from sc in _context.Stud_Courses
                          from c in _context.Courses
                          where sc.Crs_ID == c.Crs_ID
                          && sc.St_ID == studentId
                          select new
                          {
                              c.Crs_ID,
                              c.Crs_Name
                          };

            if (!courses.Any())
                return NotFound("Student has no enrolled courses");

            return Ok(courses.ToList());
        }
    }

}
