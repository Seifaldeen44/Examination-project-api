using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examProj.Models
{
    [Table("Course", Schema = "HR")]
    public class Course
    {
        [Key]
        public int? Crs_ID { get; set; }
        public string? Crs_Name { get; set; }
    }

}
