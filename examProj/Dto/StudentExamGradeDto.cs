namespace examProj.Dto
{
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public class StudentExamGradeDto
    {
        public int Ex_ID { get; set; }
        public string CourseName { get; set; }
        public int StudentDegree { get; set; }
        public int TotalExamDegree { get; set; }
        public DateTime? ExamDate { get; set; }
    }


}
