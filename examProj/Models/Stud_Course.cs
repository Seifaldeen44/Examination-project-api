using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examProj.Models
{
    [Table("Stud_Course", Schema = "HR")]
    public class Stud_Course
    {
        
        public int St_ID { get; set; }
        
        public int Crs_ID { get; set; }
    }

}
