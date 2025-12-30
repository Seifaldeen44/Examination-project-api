using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examProj.Models
{
    [Table("Exam", Schema = "Instructor")]
    public class Exam
    {
        [Key]
        public int? Ex_ID { get; set; }
        public int? Crs_ID { get; set; }
        public string? Ex_Name { get; set; }
    }

}
