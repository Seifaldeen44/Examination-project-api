using Azure.Core;
using examProj.Data;
using examProj.Dto;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace examProj.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public LoginController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpPost("check")]
        public IActionResult CheckUser(LoginRequestDto Request)
        {
            if (string.IsNullOrEmpty(Request.Role) || Request.Id == null)
                return BadRequest("Role and Id are required");

            if (Request.Role.ToLower() == "student")
            {
                var student = _context.Student
                                      .FirstOrDefault(s => s.St_ID == Request.Id);

                if (student == null)
                    return NotFound("Student not found");

                return Ok(new
                {
                    Role = "student",
                    page = "students",
                    Id = student.St_ID,
                    name = student.St_Fname
                });
            }

            if (Request.Role.ToLower() == "instructor")
            {
                var instructor = _context.Instructor
                                         .FirstOrDefault(i => i.Ins_ID == Request.Id);

                if (instructor == null)
                    return NotFound("Instructor not found");

                return Ok(new
                {
                    role = "instructor",
                    page = "instructors",
                    id = instructor.Ins_ID,
                    name = instructor.Ins_Name,
                });
            }

            return BadRequest("Invalid role");
        }
    }

}
